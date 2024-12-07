namespace Project_Manager.ObjectStates.Notification.Interfaces
{
    public interface INotificationState
    {
        //void next(Models.Notification notification);
        //void prev(Models.Notification notification);
        //void printStatus();
        void Handle(Models.Notification notification);
    }
}
