using Project_Manager.DTO.ProjectTasks;

namespace Project_Manager.Helpers
{
    public class SortState
    {
        static public Dictionary<string, bool> isColumnInProjectTaskViewSorted = new Dictionary<string, bool>
        {
            { nameof(ProjectTaskDTO.Title), false },
            { nameof(ProjectTaskDTO.Status), false },
            { "Category.Name", false },
            { "AppUser.UserName", false },
            { nameof(ProjectTaskDTO.DueDateTime), false },
            { nameof(ProjectTaskDTO.Description), false }
        };
    }
}
