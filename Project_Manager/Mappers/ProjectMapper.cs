using Project_Manager.DTO.Project;
using Project_Manager.Models;
using Project_Manager.ViewModels;

namespace Project_Manager.Mappers
{
    public static class ProjectMapper
    {
        public static ProjectDTO ToProjectDTO(this Project project)
        {
            return new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name
            };
        }

        public static Project ToProject(this CreateAndEditProjectVM createProjectVM)
        {
            return new Project
            {
                Name = createProjectVM.Name
            };
        }

        public static CreateAndEditProjectVM ToCreateAndEditProjectVM(this Project project)
        {
            return new CreateAndEditProjectVM(project.Name);
        }
    }
}
