using MQTTnet;
using RollingShutterProject.Interfaces;
using RollingShutterProject.Models;
using RollingShutterProject.UnitOfWork;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RollingShutterProject.Services
{
    public class MqttService : IMqttService
    {
        private readonly IMqttClient _mqttClient;
        private readonly ILogger<MqttService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MqttService(ILogger<MqttService> logger, IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            var factory = new MqttClientFactory();
            _mqttClient = factory.CreateMqttClient();
            _serviceScopeFactory = serviceScopeFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ConnectAsycn()
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId("RollingShutterProject")
                .WithTcpServer("192.168.3.76", 1883)
                .WithCleanSession()
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                _logger.LogInformation($"MQTT Mesajı Alındı: {topic}");
                _logger.LogInformation($"MQTT Payload: {payload}");

                if (topic == "sensor/data")
                {
                    _logger.LogInformation("API 'sensor/data' mesajını işliyor...");
                    await HandleSensorData(payload);
                }
            };

            try
            {
                await _mqttClient.ConnectAsync(options);
                await _mqttClient.SubscribeAsync("sensor/data");
                _logger.LogInformation("MQTT Client bağlandı ve 'sensor/data' dinleniyor");
            }
            catch (Exception ex)
            {
                _logger.LogError($"MQTT Client bağlanamadı... Hata: {ex.Message}");
            }
        }

        private async Task HandleSensorData(string payload)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                try
                {
                    _logger.LogInformation($"Gelen JSON: {payload}");

                    // JSON'u Deserialize Et
                    EnvironmentalSensorData? sensorData;
                    try
                    {
                        sensorData = JsonSerializer.Deserialize<EnvironmentalSensorData>(payload);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"JSON deserialize hatası: {ex.Message}");
                        return;
                    }

                    if (sensorData == null)
                    {
                        _logger.LogError("sensorData NULL döndü.");
                        return;
                    }

                    int currentUserId = 1;// GetCurrentUserId();
                    var userSettings = await unitOfWork.UserSettings.GetUserSettings(currentUserId) ?? new UserSettings
                    {
                        LoggingIntervalHours = 3, // Varsayılan 3 saat
                        DetectAnomalies = true
                    };

                    int intervalHours = userSettings.LoggingIntervalHours;
                    float threshold = userSettings.DetectAnomalies ? 3 : float.MaxValue;

                    // **Sensör verilerini işle**
                    var sensorValues = new Dictionary<string, float>
            {
                { "Sıcaklık", sensorData.Temperature },
                { "Hava Kalitesi", sensorData.AirQuality }
            };

                    //if (userSettings.AutoOpenShutter)
                    //{
                    //    if ((sensorData.Temperature >= 35 && userSettings.NotifyOnHighTemperature) ||
                    //       (sensorData.AirQuality >= 100 && userSettings.NotifyOnPoorAirQuality))
                    //    {
                    //        await PublishMessageAsync("device/command", "OPEN");
                    //        await SaveUserCommand(0, "Ortam koşulları nedeniyle otomatik olarak açıldı.");
                    //        _logger.LogInformation("Ortam koşullarına göre panjur otomatik açıldı.");
                    //    }
                    //}

                    // Tüm sensör değerlerini tek seferde kontrol edelim.
                    bool shouldOpenShutter = false;
                    bool shouldCloseShutter = false;

                    if (userSettings.AutoOpenShutter)
                    {
                        // Açılması için herhangi bir kritik durum varsa TRUE dönecek
                        if ((sensorData.Temperature >= 35 && userSettings.NotifyOnHighTemperature) ||
                            (sensorData.AirQuality >= 100 && userSettings.NotifyOnPoorAirQuality))
                        {
                            shouldOpenShutter = true;
                        }

                        // Kapatılması için tüm koşullar normale dönerse TRUE dönecek
                        if ((sensorData.Temperature <= 25 && sensorData.AirQuality <= 60))
                        {
                            shouldCloseShutter = true;
                        }

                        // Komut gönderme işlemini optimize et (her seferinde aç-kapa yapma)
                        if (shouldOpenShutter)
                        {
                            await PublishMessageAsync("device/command", "OPEN");
                            await SaveUserCommand(0, $"Ortam koşulları nedeniyle otomatik açıldı (Sıcaklık: {sensorData.Temperature}, Hava Kalitesi: {sensorData.AirQuality}).",true);
                            _logger.LogInformation($"Otomatik açıldı (Sıcaklık: {sensorData.Temperature}, Hava Kalitesi: {sensorData.AirQuality})");
                        }
                        else if (shouldCloseShutter)
                        {
                            await PublishMessageAsync("device/command", "CLOSE");
                            await SaveUserCommand(0, $"Ortam koşulları normale döndüğünden otomatik kapandı (Sıcaklık: {sensorData.Temperature}, Hava Kalitesi: {sensorData.AirQuality}).");
                            _logger.LogInformation($"Otomatik panjur kapandı.");
                        }
                        else
                        {
                            _logger.LogInformation("Ortam koşullarında değişiklik yok, komut gönderilmedi.");
                        }
                    }




                    foreach (var sensorType in sensorValues.Keys)
                    {
                        // **Her sensör için ayrı `deviceId` bul**
                        int? deviceId = sensorData.DeviceId > 0 ? sensorData.DeviceId : null;

                        if (deviceId == null)
                        {
                            var device = await unitOfWork.Devices.GetDeviceBySensorType(sensorType);
                            deviceId = device?.Id;
                        }

                        // Eğer hala `deviceId` bulunamadıysa, hata ver ve işlemi devam ettirme
                        if (deviceId == null)
                        {
                            _logger.LogError($"HATA: `{sensorType}` için bağlı bir cihaz bulunamadı.");
                            continue;
                        }

                        var lastData = await unitOfWork.SensorData.GetLastSensorData(sensorType, deviceId);
                        bool shouldSave = lastData == null;

                        float currentValue = sensorValues[sensorType];

                       

                        if (!shouldSave)
                        {
                            TimeSpan timeDifference = DateTime.Now - lastData!.TimeStamp;
                            bool isTimeToSave = timeDifference.TotalHours >= intervalHours;

                            float valueDifference = Math.Abs(currentValue - lastData.Value);
                            float changePercentage = lastData.Value != 0 ? (valueDifference / lastData.Value) * 100 : 0;
                            bool isSignificantChange = changePercentage >= threshold;

                            shouldSave = isTimeToSave || isSignificantChange;
                        }

                        if (shouldSave)
                        {
                            var newSensorData = new SensorData
                            {
                                DeviceId = deviceId.Value,
                                DeviceIdString = deviceId.ToString(),
                                SensorType = sensorType,
                                Value = currentValue,
                                TimeStamp = DateTime.Now
                            };

                            await unitOfWork.SensorData.AddAsync(newSensorData);
                            await unitOfWork.CompleteAsync();
                            _logger.LogInformation($"Yeni Sensör Verisi Kaydedildi: {sensorType} - {currentValue} - Device ID: {deviceId}");
                        }
                        else
                        {
                            _logger.LogInformation($"Veri kaydedilmedi: {sensorType} süresi dolmadı veya değişim %3’ten az.");
                        }

                        // **Tehlikeli değerlerde bildirim gönder**
                        bool shouldNotify = (sensorType == "Sıcaklık" && currentValue >= 35 && userSettings.NotifyOnHighTemperature) ||
                                            (sensorType == "Hava Kalitesi" && currentValue > 100 && userSettings.NotifyOnPoorAirQuality);

                        if (shouldNotify)
                        {
                            await SendNotification(deviceId.Value, sensorType, currentValue);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"MQTT verisi işlenirken hata oluştu: {ex.Message}");
                }
            }
        }


        public async Task PublishMessageAsync(string topic, string message)
        {
            try
            {
                var mqttMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(message)
                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();

                await _mqttClient.PublishAsync(mqttMessage);
                _logger.LogInformation($"MQTT Mesaj Gönderildi: {topic} - {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"MQTT Mesaj Gönderme Hatası: {ex.Message}");
            }
        }

        private async Task SendNotification(int deviceId, string sensorType, float value)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                string subject = "Dikkat! Ortam koşulları kötüleşti.";
                string message = $"{sensorType} değeri kritik seviyeye ulaştı: {value}";

                await notificationService.SendEmailAsync(subject, message);
                _logger.LogInformation($"Kullanıcıya bildirim gönderildi: {message}");
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        private async Task SaveUserCommand(int deviceId, string command, bool isAuto=false)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                try
                {
                    int currentUserId = GetCurrentUserId();

                    var userCommand = new UserCommand
                    {
                        UserId = currentUserId, 
                        DeviceId = deviceId,
                        Command = command,
                        IsAuto = isAuto,
                        TimeStamp = DateTime.Now
                    };

                    await unitOfWork.UserCommands.AddAsync(userCommand);
                    await unitOfWork.CompleteAsync();

                    _logger.LogInformation($"Kullanıcı komutu kaydedildi: {command} - Kullanıcı ID: {currentUserId}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Kullanıcı komutu kaydedilirken hata oluştu: {ex.Message}");
                }
            }
        }


    }

    public class EnvironmentalSensorData
    {
        [JsonPropertyName("deviceId")]
        public int DeviceId { get; set; }

        [JsonPropertyName("temperature")]
        public float Temperature { get; set; }

        [JsonPropertyName("airQuality")]
        public float AirQuality { get; set; }

        [JsonPropertyName("shutterPercentage")]
        public float? ShutterPercentage { get; set; }
    }
}
