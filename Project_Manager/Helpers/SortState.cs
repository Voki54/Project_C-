using Project_Manager.DTO.ProjectTasks;

namespace Project_Manager.Helpers
{
    public class SortState
    {
        static public Dictionary<string, bool> isColumnInProjectTaskViewSorted = new Dictionary<string, bool>
        {
            { nameof(ProjectTaskDTO.Title), false },
            { nameof(ProjectTaskDTO.Status), false },
            { nameof(ProjectTaskDTO.Category), false },
            { nameof(ProjectTaskDTO.ExecutorName), false },
            { nameof(ProjectTaskDTO.DueDateTime), false },
            { nameof(ProjectTaskDTO.Description), false }
        };
    }
}
