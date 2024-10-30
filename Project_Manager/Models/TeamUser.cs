namespace Project_Manager.Models
{
    public class TeamUser
    {
        public Guid IdTeam { get; set; }
        public Guid IdUser { get; set; }
        public UserRoles Role { get; set; }
    }
}
