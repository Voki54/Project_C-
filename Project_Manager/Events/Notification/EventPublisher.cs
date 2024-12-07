namespace Project_Manager.Events.Notification
{
    public class EventPublisher
    {
        private readonly List<Func<IEvent, Task>> _subscribers = new List<Func<IEvent, Task>>();

        // Подписка на события
        public void Subscribe(Func<IEvent, Task> handler)
        {
            _subscribers.Add(handler);
        }

        // Публикация события
        public async Task PublishAsync(IEvent @event)
        {
            foreach (var subscriber in _subscribers)
            {
                await subscriber(@event);
            }
        }
    }
}
