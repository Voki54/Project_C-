using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.ViewModels;

namespace Project_Manager.Services.Interfaces
{
    public interface IProjectTasksService
    {
        Task<CreateReadTaskVM> GetCreateReadTaskVMAsync(int projectId, int? taskId);
        Task<ProjectTask> CreateTaskAsync(ProjectTask task);
        Task<ProjectTask> UpdateTaskAsync(ProjectTask task);
        Task<ProjectTask> FindTaskByIdOrNullAsync(int taskId);
        Task DeleteTaskAsync(int taskId);
        Task ChangeTaskStatusAsync(int taskId, ProjectTaskStatus taskStatus);
        Task<Comment> AddCommentAsync(int taskId, string content);
        Task<CreateReadTaskVM> ToCreateReadTaskVM(int projectId, int? taskId);
        Task<ViewTaskVM?> GetViewTaskVMAsync(int taskId, int projectId);
        Task<TaskCategoryVM> GetTaskCategoryVMAsync(int projectId, int? categoryId, string? sortColumn,
            string? filterStatus, string? filterExecutor, DateTime? filterDate);
    }
}
