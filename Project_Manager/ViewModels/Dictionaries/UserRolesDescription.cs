using Project_Manager.Models.Enums;

namespace Project_Manager.ViewModels.Dictionaries
{
    public static class UserRolesDescription
    {
        public static Dictionary<UserRoles, string> userRoles = new Dictionary<UserRoles, string>
        {
            { UserRoles.Manager, "Менеджер" },
            { UserRoles.Executor, "Исполнитель" }
        };
    }
}
