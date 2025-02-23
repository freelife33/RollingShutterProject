namespace RollingShutterProject.Interfaces
{
    public interface IMqttService
    {
        Task ConnectAsycn();
        Task PublishMessageAsync(string topic,  string message);
    }
}
