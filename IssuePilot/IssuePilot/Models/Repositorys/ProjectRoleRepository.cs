using IssuePilot.Data;
using IssuePilot.Models.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssuePilot.Models.Repositorys
{
    public class ProjectRoleRepository : IProjectRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public ProjectRoleRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task AddProjectMemberEntryAsync(string userId, int roleId, int projectId)
        {
            if (userId == null) { throw new ArgumentNullException(); }
            User dbUser = _context.Users.Find(userId);
            ProjectRole dbRole = _context.ProjectRoles.Find(roleId);
            Project dbProject = _context.Projects.Find(projectId);

            if (dbUser == null || dbRole == null || dbProject == null) { throw new NullReferenceException(); }
            ProjectMemberEntry entry = new ProjectMemberEntry
            {
                Project = dbProject,
                ProjectRole = dbRole,
                User = dbUser
            };
            _context.ProjectMemberEntries.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllNonMembersOfProjectAsync(int projectId)
        {
            var alreadyMember = await GetMembersOfProjectByIdAsync(projectId);
            return _context.Users.ToList().Except((IEnumerable<User>)alreadyMember).Where(c => c.IsDeleted == false).ToList();
        }

        public async Task<List<ProjectRole>> GetAllProjectRolesAsync()
        {
            return await _context.ProjectRoles.ToListAsync();
        }
        public async Task<List<Project>> GetProjectsOfUserByIdAsync(string userId)
        {
            if (userId == null) { throw new ArgumentNullException(); }
            var memberEntry = await _context.ProjectMemberEntries.Where(r => r.FK_UserId == userId).ToListAsync();
            List<Project> listOfProjects = new List<Project>();
            foreach (var member in memberEntry)
            {
                listOfProjects.Add(_context.Projects.Find(member.Project.Id));
            }
            return listOfProjects;
        }

        public async Task<List<User>> GetMembersOfProjectByIdAsync(int projectId)
        {
            if (await _context.Projects.FindAsync(projectId) == null) { throw new NullReferenceException(); }
            var memberEntry = await _context.ProjectMemberEntries.Where(r => r.FK_ProjectId == projectId).ToListAsync();
            var listOfUsers = new List<User>();
            foreach (var entry in memberEntry)
            {
                listOfUsers.Add(await _context.Users.FindAsync(entry.FK_UserId));
            }
            return listOfUsers;
        }

        public async Task<ProjectRole> GetProjectRoleOfUserAsync(string userId, int projectId)
        {
            if (userId == null) { throw new ArgumentNullException(); }
            var entry = await _context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.Project.Id == projectId && r.User.Id == userId);
            return await _context.ProjectRoles.FindAsync(entry.FK_ProjectRoleId);
        }

        public async Task<bool> IsUserInProjectAsync(string userId, int projectId)
        {
            if (userId == null) { throw new ArgumentNullException(); }
            var entry = await _context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.FK_ProjectId == projectId && r.FK_UserId == userId);
            return entry != null;
        }

        public async Task RemoveMemberFromProjectAsync(int projectId, string userId)
        {
            if (userId == null) { throw new ArgumentNullException(); }
            var entry = await _context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.FK_ProjectId == projectId && r.FK_UserId == userId);
            _context.ProjectMemberEntries.Remove(entry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectMemberRoleAsync(int projectId, string userId, int oldRoleId, int newRoleId)
        {
            if (userId == null) { throw new ArgumentNullException(); }
            var dbEntry = await _context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.FK_ProjectId == projectId && r.FK_ProjectRoleId == oldRoleId && r.FK_UserId == userId);
            _context.ProjectMemberEntries.Remove(dbEntry);
            await _context.SaveChangesAsync();

            await AddProjectMemberEntryAsync(userId, newRoleId, projectId);
            await _context.SaveChangesAsync();
        }
    }
}
