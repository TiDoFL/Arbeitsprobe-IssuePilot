using IssuePilot.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface IProjectRepository
    {
        // Project
        Task<(ProjectUpdateViewModel, Project)> CreateProjectAsync(ProjectUpdateViewModel model, string userId);
        Task<Project> GetProjectByIdAsync(int id);
        Task<ProjectUpdateViewModel> UpdateProjectAsync(ProjectUpdateViewModel model);
        Task<List<Project>> GetAllProjectsAsync();
        Task DeleteProjectAsync(int id);

        // TicketCategories
        Task<List<TicketCategory>> GetAllTicketCategoriesOfProjectAsync(int projectId);
        Task<TicketCategory> CreateTicketCategoryAsync(string categorieTitle, Project project);
        Task UpdateTicketCategoriesAsync(CategoryCreateInputModel model);
        Task<TicketCategory> GetTicketCategoryByIdAsync(int id);
        Task DeleteTicketCategoryAsync(int id);
    }
}
