namespace RollingShutterProject.Models
{
    public class UserCommand
    {
        public int Id { get; set; } 
        public int UserId { get; set; } 
        public int DeviceId { get; set; }  
        public string? Command { get; set; }  
        public DateTime TimeStamp { get; set; } 
    }
}
