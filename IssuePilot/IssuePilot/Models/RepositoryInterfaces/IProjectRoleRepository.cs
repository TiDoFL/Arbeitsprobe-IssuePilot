using System.Collections.Generic;
using System.Threading.Tasks;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface IProjectRoleRepository
    {
        Task<List<ProjectRole>> GetAllProjectRolesAsync();
        Task AddProjectMemberEntryAsync(string userId, int roleId, int projectId);
        Task<List<Project>> GetProjectsOfUserByIdAsync(string userId);
        Task<List<User>> GetMembersOfProjectByIdAsync(int projectId);
        Task<bool> IsUserInProjectAsync(string userId, int projectId);
        Task<ProjectRole> GetProjectRoleOfUserAsync(string userId, int projectId);
        Task<List<User>> GetAllNonMembersOfProjectAsync(int projectId);
        Task UpdateProjectMemberRoleAsync(int projectId, string userId, int oldRoleId, int newRoleId);
        Task RemoveMemberFromProjectAsync(int projectId, string userId);
    }
}
