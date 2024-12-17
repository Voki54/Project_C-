using Project_Manager.Models.Enums;

namespace Project_Manager.Models.Dictionary
{
    public static class UserRolesDict
    {
        public static Dictionary<UserRoles, string> userRoles = new Dictionary<UserRoles, string>
        {
            { UserRoles.Manager, "Менеджер" },
            { UserRoles.Executor, "Исполнитель" }
        };
    }
}
