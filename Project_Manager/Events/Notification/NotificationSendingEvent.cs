namespace Project_Manager.Events.Notification
{
    public class NotificationSendingEvent : INotificationEvent
    {
        public string? SenderId { get; }
        public string? RecipientId { get; }
        public int ProjectId { get; }
        public NotificationType Type { get; }
        public DateTime Timestamp { get; } = DateTime.Now;

        public static NotificationSendingEvent CreateWithSender(string senderId, int projectId, NotificationType type)
        {
            return new NotificationSendingEvent(projectId, type, senderId);
        }

        public static NotificationSendingEvent CreateWithRecipient(string recipientId, int projectId, NotificationType type)
        {
            return new NotificationSendingEvent(projectId, type, null, recipientId);
        }

        private NotificationSendingEvent(int projectId, NotificationType type, string? senderId = null, string? recipientId = null)
        {
            SenderId = senderId;
            RecipientId = recipientId;
            ProjectId = projectId;
            Type = type;
        }
    }
}
