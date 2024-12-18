using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Helpers;
using Project_Manager.Models;
using Project_Manager.Models.Enums;
using Project_Manager.Services.Interfaces;
using Project_Manager.ViewModels;

namespace Project_Manager.Services
{
    public class ProjectTasksService: IProjectTasksService
    {
        private readonly IProjectTaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserAccessService _userAccessService;
        private readonly ILogger<ProjectTasksService> _logger;

        public ProjectTasksService(IProjectTaskRepository taskRepository, IProjectRepository projectRepository, 
            IProjectUserRepository projectUserRepository, ICommentRepository commentRepository, 
            ICategoryRepository categoryRepository, IUserAccessService userAccessService, ILogger<ProjectTasksService> logger)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _projectUserRepository = projectUserRepository;
            _commentRepository = commentRepository;
            _categoryRepository = categoryRepository;
            _userAccessService = userAccessService;
            _logger = logger;
        }

        public async Task<CreateReadTaskVM> GetCreateReadTaskVMAsync(int projectId, int? taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: projectId = {ProjectId}, taskId = {TaskId}", nameof(GetCreateReadTaskVMAsync), projectId, taskId);
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                _logger.LogError("Проект с ID {ProjectId} не найден.", projectId);
                throw new KeyNotFoundException($"Проект с ID {projectId} не найден.");
            }
            var model = await ToCreateReadTaskVM(projectId, taskId);
            _logger.LogInformation("Создана модель CreateReadTaskVM для проекта с ID {projectId}.", projectId);
            return model;
        }

        public async Task<ProjectTask> CreateTaskAsync(ProjectTask task)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: {@Task}", nameof(CreateTaskAsync), task);
            return await _taskRepository.CreateAsync(task);
        }

        public async Task<ProjectTask> UpdateTaskAsync(ProjectTask task)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: {@Task}", nameof(UpdateTaskAsync), task);
            return await _taskRepository.UpdateAsync(task);
        }

        public async Task<ProjectTask> FindTaskByIdOrNullAsync(int taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}", nameof(FindTaskByIdOrNullAsync), taskId);
            return await _taskRepository.FindByIdOrNullAsync(taskId);
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}", nameof(DeleteTaskAsync), taskId);
            await _taskRepository.DeleteByIdAsync(taskId);
        }

        public async Task ChangeTaskStatusAsync(int taskId, ProjectTaskStatus taskStatus)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}, taskStatus = {TaskStatus}", nameof(ChangeTaskStatusAsync), taskId, taskStatus);
            await _taskRepository.ChangeStatusByIdAsync(taskId, taskStatus);
        }

        public async Task<Comment> AddCommentAsync(int taskId, string content)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}, content = {Content}", nameof(AddCommentAsync), taskId, content);
            var comment = new Comment
            {
                Content = content,
                ProjectTaskId = taskId,
                CreatedAt = DateTime.Now
            };
            await _commentRepository.CreateAsync(comment);
            _logger.LogInformation("Добавлен комментарий к задаче с ID {TaskId}.", taskId);
            return comment;
        }

        public async Task<CreateReadTaskVM> ToCreateReadTaskVM(int projectId, int? taskId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: projectId = {ProjectId}, taskId = {TaskId}", nameof(ToCreateReadTaskVM), projectId, taskId);
            ProjectTask task = null;
            if (taskId != null)
            {
                task = await _taskRepository.FindByIdOrNullAsNoTrackingAsync(taskId);
                if (task == null) 
                {
                    _logger.LogWarning("Задача с ID {TaskId} не найдена.", taskId);
                    throw new KeyNotFoundException($"Задача с ID {taskId} не найдена.");
                }
            }
            var projectUsers = await _projectUserRepository.GetUsersDtoByProjectAsync(projectId);
            var categories = await _categoryRepository.GetCategoriesByProjectIdAsync(projectId);
            var model = new CreateReadTaskVM
            {
                Task = task,
                ProjectId = projectId,
                Categories = categories,
                Users = projectUsers
            };

            _logger.LogInformation("Создана модель CreateReadTaskVM для проекта с ID {ProjectId}.", projectId);
            return model;
        }

        public async Task<ViewTaskVM?> GetViewTaskVMAsync(int taskId, int projectId)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: taskId = {TaskId}, projectId = {ProjectId}", nameof(GetViewTaskVMAsync), taskId, projectId);
            var taskDTO = await _taskRepository.GetTaskDtoByIdAsync(taskId);
            if (taskDTO == null)
            {
                _logger.LogWarning("Задача с ID {TaskId} не найдена.", taskId);
                throw new KeyNotFoundException($"Задача с ID {taskId} не найдена.");
            }
            var model = new ViewTaskVM
            {
                Task = taskDTO,
                ProjectId = projectId,
                Role = (UserRoles)await _userAccessService.CurrentUserRoleInProjectOrNullAsync(projectId),
            };
            _logger.LogInformation("Создана модель ViewTaskVM для задачи с ID {TaskId}.", taskId);
            return model;
        }

        public async Task<TaskCategoryVM> GetTaskCategoryVMAsync(int projectId, int? categoryId, string? sortColumn, 
            string? filterStatus, string? filterExecutor, DateTime? filterDate)
        {
            _logger.LogInformation("Вызван метод {MethodName} с параметрами: projectId = {ProjectId}, categoryId = {CategoryId}, " +
                "sortColumn = {SortColumn}, filterStatus = {FilterStatus}, filterExecutor = {FilterExecutor}, filterDate = {FilterDate}",
                nameof(GetTaskCategoryVMAsync), projectId, categoryId, sortColumn, filterStatus, filterExecutor, filterDate);

            var categories = await _categoryRepository.GetCategoriesByProjectIdAsync(projectId);
            var currUserId = await _userAccessService.CurrentUserIdAsync();
            var role = await _userAccessService.CurrentUserRoleInProjectOrNullAsync(projectId);
            string orderBy = null;
            if (sortColumn != null)
            {
                var isAscending = !SortState.isColumnInProjectTaskViewSorted.GetValueOrDefault(sortColumn, false);
                orderBy = isAscending ? sortColumn : sortColumn + " desc";

                SortState.isColumnInProjectTaskViewSorted[sortColumn] = isAscending;
            }
            var tasks = await _taskRepository.GetTasksDtoWithParamsAsync(projectId, categoryId, orderBy, currUserId, 
                role, filterStatus, filterExecutor, filterDate);

            var model = new TaskCategoryVM
            {
                Categories = categories,
                SelectedCategory = categoryId,
                Tasks = tasks,
                SortedColumn = sortColumn,
                IsAsc = sortColumn != null ? SortState.isColumnInProjectTaskViewSorted.GetValueOrDefault(sortColumn, false) : null,
                ProjectId = projectId,
                Role = (UserRoles)role
            };

            _logger.LogInformation("Создана модель TaskCategoryVM для проекта с ID {ProjectId}.", projectId);
            return model;
        }
    }
}
