namespace Project_Manager.Events.Notification
{
    public class EventPublisher
    {
        private readonly List<Func<INotificationEvent, Task>> _subscribers = new List<Func<INotificationEvent, Task>>();

        public void Subscribe(Func<INotificationEvent, Task> handler)
        {
            _subscribers.Add(handler);
        }

        public async Task PublishAsync(INotificationEvent notificationEvent)
        {
            foreach (var subscriber in _subscribers)
            {
                await subscriber(notificationEvent);
            }
        }
    }
}
