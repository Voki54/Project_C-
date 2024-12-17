using Project_Manager.DTO.ProjectTasks;
using Project_Manager.Models;
using Project_Manager.Models.Enums;

namespace Project_Manager.Data.DAO.Interfaces
{
    public interface IProjectTaskRepository
    {
        Task<ProjectTask> CreateAsync(ProjectTask task);
        Task<ProjectTask?> FindByIdOrNullAsync(int? taskId);
        Task<ProjectTask?> FindByIdOrNullIncludeUsersAndCategoriesAsNoTrackingAsync(int taskId);
        Task<ProjectTask?> FindByIdOrNullAsNoTrackingAsync(int? taskId);
        Task DeleteByIdAsync(int taskId);
        Task ChangeStatusByIdAsync(int taskId, ProjectTaskStatus taskStatus);
        Task<ProjectTask> UpdateAsync(ProjectTask task);
        Task<ProjectTaskDTO?> GetTaskDtoByIdAsync(int taskId);
        Task<List<ProjectTaskDTO>> GetTasksDtoWithParamsAsync(int projectId, int? categoryId, string? sortColumn, string? userId, UserRoles? userRole,
            string? filterStatus, string? filterExecutor, DateTime? filterDate);
    }
}
