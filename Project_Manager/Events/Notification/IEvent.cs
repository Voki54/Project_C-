namespace Project_Manager.Events.Notification
{
    public interface IEvent
    {
        string SenderId { get; }
        int ProjectId { get; }
        DateTime Timestamp { get; }
    }
}
