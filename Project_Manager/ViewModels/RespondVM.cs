namespace Project_Manager.ViewModels
{
    public class RespondVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int ProjectId { get; set; }

        public RespondVM(string userId, string userName, string userEmail, int projectId)
        {
            UserId = userId;
            UserName = userName;
            UserEmail = userEmail;
            ProjectId = projectId;
        }
    }
}
