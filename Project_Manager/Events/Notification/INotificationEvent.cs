namespace Project_Manager.Events.Notification
{
    public interface INotificationEvent
    {
        string? SenderId { get; }
        string? RecipientId { get; }
        int ProjectId { get; }
        NotificationType Type { get; }
        DateTime Timestamp { get; }
    }
}
