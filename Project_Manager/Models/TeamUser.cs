using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Manager.Models
{
    [Table("TeamUser")]
    public class TeamUser
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public string? UserId { get; set; }
        public AppUser AppUser { get; set; }
        public UserRoles Role { get; set; }
    }
}
