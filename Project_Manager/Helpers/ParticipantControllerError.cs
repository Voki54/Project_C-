namespace Project_Manager.Helpers
{
    public static class ParticipantControllerError
    {
        public enum Errors
        {
            UserNotFound,
            UserNotProject,
            UpdateError,
            ExcludeError
        }

        public static Dictionary<Errors, string> errorsDescription = new Dictionary<Errors, string>
        {
            { Errors.ExcludeError, "Ошибка исключения пользователя из команды." },
            { Errors.UpdateError, "Ошибка изменения роли пользователя." },
            { Errors.UserNotFound, "Не найден пользователь для изменения роли." },
            { Errors.UserNotProject, "Пользователь не состоит в проекте." }
        };
    }
}
