using Microsoft.CodeAnalysis;

namespace Project_Manager.DTO.JoinProject
{
    public class RespondDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int ProjectId { get; set; }

        public RespondDTO(string userId, string userName, string userEmail, int projectId)
        {
            UserId = userId;
            UserName = userName;
            UserEmail = userEmail;
            ProjectId = projectId;
        }
    }


}
