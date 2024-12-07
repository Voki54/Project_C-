namespace Project_Manager.Events.Notification
{
    public class NotificationSendingEvent : IEvent
    {
        public string SenderId { get; }
        public int ProjectId { get; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        public NotificationSendingEvent(string senderId, int projectId)
        {
            SenderId = senderId;
            ProjectId = projectId;
        }
    }
}
