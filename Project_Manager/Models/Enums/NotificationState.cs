namespace Project_Manager.Models.Enums
{
    public enum NotificationState
    {
        Created,
        Waiting,
        Sent,
        Read,
        Deleted
    }
}
/*    public static class NotificationStateExtensions
    {
        *//*        public static bool IsFinalState(this NotificationState state)
                {
                    return state == NotificationState.Delivered || state == NotificationState.Failed;
                }*//*

        public static bool ChangeState(this NotificationState state)
        {


            return state == NotificationState.Delivered || state == NotificationState.Failed;
        }
    }
}*/
