using IssuePilot.Models;
using IssuePilot.Models.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IssuePilot.Helper
{
    public abstract class ProjectAccessController : Controller
    {
        private readonly IProjectRoleRepository _projectRoleRepository;
        private readonly IProjectRepository _projectRepository;

        protected ProjectAccessController(IProjectRoleRepository projectRoleRepository, IProjectRepository projectRepository)
        {
            _projectRoleRepository = projectRoleRepository;
            _projectRepository = projectRepository;
        }

        // NoAccess
        public IActionResult NoAccess()
        {
            return View();
        }

        // Access
        public async Task<bool> ProjectFunctionAccessAsync(int projectId)
        {
            if (User.IsInRole("Admin"))
            {
                return true;
            }
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var activeUserRole = await _projectRoleRepository.GetProjectRoleOfUserAsync(currentUserId, projectId);
            // role = "Eigentümer"
            if (activeUserRole == null || activeUserRole.Id != 1)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ProjectAccessAsync(int projectId)
        {
            if (User.IsInRole("Admin") || await _projectRoleRepository.IsUserInProjectAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value, projectId))
            {
                return true;
            }
            return false;
        }

        public async Task<PaginatedList<Project>> GetProjectsAsync(string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {
            ViewData["TitleSortParm"] = sortOrder == "Title" ? "title_desc" : "Title";
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Date" : "";
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Project> projects = new List<Project>();
            if (User.IsInRole("Admin"))
            {
                projects = await _projectRepository.GetAllProjectsAsync();
            }
            else
            {
                projects = await _projectRoleRepository.GetProjectsOfUserByIdAsync(currentUserId);

            }
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                List<Project> copy =  new List<Project>();
                copy.AddRange(projects);
                foreach (var project in copy)
                {
                    if (project.Description == null)
                    {
                        if (!project.Title.Contains(searchString))
                        {
                            projects.Remove(project);
                        }
                    }
                    else
                    {
                        if (!project.Title.Contains(searchString) && !project.Description.Contains(searchString))
                        {
                            projects.Remove(project);
                        }
                    }
                }
            }
            ViewData["CurrentSort"] = sortOrder;
            switch (sortOrder)
            {
                case "title_desc":
                    projects = projects.OrderByDescending(s => s.Title).ToList();
                    break;
                case "Date":
                    projects = projects.OrderBy(s => s.CreateDate).ToList();
                    break;
                case "Title":
                    projects = projects.OrderBy(s => s.Title).ToList();
                    break;
                default:
                    projects = projects.OrderByDescending(s => s.CreateDate).ToList();
                    break;
            }

            int pageSize = 20;
            return await PaginatedList<Project>.CreateFromListAsync(projects, pageNumber ?? 1, pageSize);
        }
    }
}
