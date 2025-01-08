namespace Project_Manager.Events.Notification
{
    public class EventPublisher
    {
        private event Func<INotificationEvent, Task> _onEventPublished;

        public void Subscribe(Func<INotificationEvent, Task> handler)
        {
            _onEventPublished += handler;
        }

        public async Task PublishAsync(INotificationEvent notificationEvent)
        {
            if (_onEventPublished != null)
            {
                var invocationList = _onEventPublished.GetInvocationList();
                foreach (var handler in invocationList)
                {
                    var func = (Func<INotificationEvent, Task>)handler;
                    await func(notificationEvent);
                }
            }
        }
    }
}
