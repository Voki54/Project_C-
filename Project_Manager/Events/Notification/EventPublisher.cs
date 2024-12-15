namespace Project_Manager.Events.Notification
{
    public class EventPublisher
    {
        private readonly List<Func<IEvent, Task>> _subscribers = new List<Func<IEvent, Task>>();

        public void Subscribe(Func<IEvent, Task> handler)
        {
            _subscribers.Add(handler);
        }

        public async Task PublishAsync(IEvent notificationEvent)
        {
            foreach (var subscriber in _subscribers)
            {
                await subscriber(notificationEvent);
            }
        }
    }
}
