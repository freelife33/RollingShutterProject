
using RollingShutterProject.Interfaces;
using RollingShutterProject.Models;
using RollingShutterProject.UnitOfWork;

namespace RollingShutterProject.Services
{
    public class AutomationService : BackgroundService
    {
        private readonly ILogger<AutomationService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AutomationService(ILogger<AutomationService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var mqttService = scope.ServiceProvider.GetRequiredService<IMqttService>();

                    var now = DateTime.Now.TimeOfDay;
                    var userSettings = await unitOfWork.UserSettings.GetUserSettings();

                    if (userSettings == null)
                    {
                        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                        continue;
                    }
                    if (userSettings.AutoOpenShutterOnTime && userSettings.OpenTime.HasValue && userSettings.OpenTime.Value.TotalMinutes == now.TotalMinutes)
                    {
                        await mqttService.PublishMessageAsync("device/command", "OPEN");
                        await SaveAutomationLog(0, "OPEN");
                        _logger.LogInformation("Otomasyon nedeniyle açıldı {time}", now);
                    }

                    if (userSettings.AtoCloseShutterOnTime && userSettings.CloseTime.HasValue && userSettings.CloseTime.Value.TotalMinutes == now.TotalMinutes)
                    {
                        await mqttService.PublishMessageAsync("device/command", "CLOSE");
                        await SaveAutomationLog(0, "CLOSE");
                        _logger.LogInformation("Otomasyon nedeniyle kapatıldı {time}", now);
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);


            }
        }

        private async Task SaveAutomationLog(int userId, string command)
        {
            using (var scope=_scopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var userCommand= new UserCommand 
                {
                    Command = $"Otomasyon nedeniyle {command} komutu gönderildi.", 
                    UserId = 0,
                    IsAuto = true,
                    TimeStamp = DateTime.Now 
                };

                await unitOfWork.UserCommands.AddAsync(userCommand);
                await unitOfWork.CompleteAsync();
            }
        }
    }
}
