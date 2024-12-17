using Project_Manager.Models.Enums;

namespace Project_Manager.DTO.AppUser
{
    public class AppUserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRoles Role { get; set; }
    }
}
