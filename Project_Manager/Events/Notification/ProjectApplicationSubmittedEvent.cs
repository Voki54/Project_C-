namespace Project_Manager.Events.Notification
{
    public class ProjectApplicationSubmittedEvent
    {
        public string ApplicantName { get; }
        public string ProjectName { get; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        public ProjectApplicationSubmittedEvent(string applicantName, string projectName)
        {
            ApplicantName = applicantName;
            ProjectName = projectName;
        }
    }
}
