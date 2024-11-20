using Project_Manager.Models.Enums;

namespace Project_Manager.Models.Dictionary
{
    public static class UserRolesDict
    {
        public static Dictionary<int, string> userRoles = new Dictionary<int, string>
        {
            { (int)UserRoles.Manager, "Менеджер" },
            { (int)UserRoles.Executor, "Исполнитель" }
        };
    }
}
