namespace RollingShutterMVC.Models.DTOs
{
    public class SensorDataDto
    {
        public string? SensorType { get; set; }
        public float Value { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
