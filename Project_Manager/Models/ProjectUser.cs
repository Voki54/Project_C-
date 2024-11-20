using System.ComponentModel.DataAnnotations.Schema;
using Project_Manager.Models.Enums;

namespace Project_Manager.Models
{
    [Table("ProjectUser")]
    public class ProjectUser
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
        public UserRoles Role { get; set; }
    }
}
