using IssuePilot.Helper;
using IssuePilot.Models;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IssuePilot.Controllers
{
    [Authorize(Roles = "Admin,Projektmanager,Benutzer")]

    public class ProjectController : ProjectAccessController
    {
        private readonly IProjectRoleRepository _projectRoleRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IDashboardRepository _dashboardRepository;


        public ProjectController(IProjectRoleRepository projectRoleRepository, IProjectRepository projectRepository,
            IUserRepository userRepository, ITicketRepository ticketRepository, IDashboardRepository dashboardRepository) : base(projectRoleRepository, projectRepository)
        {
            _projectRoleRepository = projectRoleRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _dashboardRepository = dashboardRepository;
        }

        // GET: Projects
        public async Task<IActionResult> Index(string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {
            return View(await GetProjectsAsync(sortOrder, pageNumber, currentFilter, searchString));
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin,Projektmanager")]
        public IActionResult Create()
        {
            return View(new ProjectUpdateViewModel());
        }

        // POST: Projects/Create
        [Authorize(Roles = "Admin,Projektmanager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProjectUpdateViewModel projectModel)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Project project = new Project { Title = projectModel.Title, Description = projectModel.Description };
            Project createdProject = null;
            ProjectUpdateViewModel createdModel = null;
            if (ModelState.IsValid)
            {
                (createdModel, createdProject) = await _projectRepository.CreateProjectAsync(projectModel, currentUserId);
                if (createdModel.TitleExists == true)
                {
                    return View(createdModel);
                }
                await _projectRoleRepository.AddProjectMemberEntryAsync(currentUserId, 1, createdProject.Id);
                var message = "Du hast das Projekt - " + project.Title + " - erstellt.";
                await _dashboardRepository.CreateNewsfeedEntryAsync(currentUserId, message);
                return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = createdProject.Id }));
            }
            return View(projectModel);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin,Projektmanager")]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            if (await ProjectAccessAsync((int)id) && await ProjectFunctionAccessAsync((int)id))
            {
                var project = await _projectRepository.GetProjectByIdAsync((int)id);
                if (project == null)
                {
                    return NotFound();
                }
                ProjectUpdateViewModel model = new ProjectUpdateViewModel() { Id = project.Id, Title = project.Title, Description = project.Description };
                return View(model);
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // POST: Projects/Edit/5
        [Authorize(Roles = "Admin,Projektmanager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProjectUpdateViewModel projectModel)
        {
            if (ModelState.IsValid)
            {
                var updatedModel = await _projectRepository.UpdateProjectAsync(projectModel);
                if (updatedModel.TitleExists == true)
                {
                    return View(updatedModel);
                }
                return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = projectModel.Id }));
            }
            return View(projectModel);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin,Projektmanager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync((int)id) && await ProjectFunctionAccessAsync((int)id))
            {
                var project = await _projectRepository.GetProjectByIdAsync((int)id);
                if (project == null)
                {
                    return NotFound();
                }
                return View(project);
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "Admin,Projektmanager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _projectRepository.DeleteProjectAsync((int)id);

            return RedirectToAction(nameof(Index));
        }

        // GET: Projects/Other/5
        public async Task<IActionResult> Other(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync((int)id))
            {
                var project = await _projectRepository.GetProjectByIdAsync((int)id);
                var categories = await _projectRepository.GetAllTicketCategoriesOfProjectAsync((int)id);
                if (project == null)
                {
                    return NotFound();
                }
                ProjectViewModel model = new ProjectViewModel() { Id = project.Id, CreateDate = project.CreateDate, Description = project.Description, Title = project.Title, Creator = project.Creator.UserName, TicketCategories = categories };

                if (await ProjectFunctionAccessAsync(project.Id))
                {
                    model.IsOwner = true;
                }
                return View(model);
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // GET: Projects/CreateCategory
        [Authorize(Roles = "Admin,Projektmanager")]
        public async Task<IActionResult> CreateCategoryAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync((int)id))
            {
                return View(new CategoryCreateInputModel() { ProjectId = (int)id });
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // POST: Projects/CreateCategory
        [Authorize(Roles = "Admin,Projektmanager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateInputModel model)
        {
            if (ModelState.IsValid)
            {
                var project = await _projectRepository.GetProjectByIdAsync(model.ProjectId);
                await _projectRepository.CreateTicketCategoryAsync(model.Name, project);
                return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = model.ProjectId }));
            }
            return View(model);
        }

        // GET: Projects/EditCategory/5
        [Authorize(Roles = "Admin,Projektmanager")]
        public async Task<IActionResult> EditCategory(int? id, int? categoryId)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync((int)id))
            {
                TicketCategory category = await _projectRepository.GetTicketCategoryByIdAsync((int)categoryId);
                CategoryCreateInputModel model = new CategoryCreateInputModel() { CategoryId = category.Id, Name = category.Name, ProjectId = category.Project.Id };
                return View(model);
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // POST: Projects/EditCategory/5
        [Authorize(Roles = "Admin,Projektmanager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory([FromForm] CategoryCreateInputModel model)
        {
            if (ModelState.IsValid)
            {
                await _projectRepository.UpdateTicketCategoriesAsync(model);
                return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = model.ProjectId }));
            }
            return View(model);
        }

        // GET: Projects/DeleteCategory/5
        [Authorize(Roles = "Admin,Projektmanager")]
        public async Task<IActionResult> DeleteCategory(int? id, int? categoryId)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync((int)id))
            {
                TicketCategory category = await _projectRepository.GetTicketCategoryByIdAsync((int)categoryId);
                var project = await _projectRepository.GetProjectByIdAsync((int)id);
                return View(new CategoryDeleteViewModel() { Project = project, ProjectId = project.Id, CategoryId = category.Id, TicketCategory = category });
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // POST: Projects/DeleteCategory/5
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed([FromForm] CategoryDeleteViewModel model)
        {
            await _projectRepository.DeleteTicketCategoryAsync(model.CategoryId);
            return RedirectToAction(nameof(Other), new RouteValueDictionary(new { controller = "Project", action = "Other", Id = model.ProjectId }));
        }

        // GET: Projects/Members/5
        public async Task<IActionResult> Members(int? id, int? pageNumber)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync((int)id))
            {
                var members = await _projectRoleRepository.GetMembersOfProjectByIdAsync((int)id);
                var project = await _projectRepository.GetProjectByIdAsync((int)id);
                List<ProjectMemberDataModel> projectMemberDataList = new List<ProjectMemberDataModel>();
                foreach (var member in members)
                {
                    ProjectMemberDataModel projectMemberData = new ProjectMemberDataModel
                    {
                        User = member,
                        ProjectRole = await _projectRoleRepository.GetProjectRoleOfUserAsync(member.Id, project.Id)
                    };
                    projectMemberDataList.Add(projectMemberData);
                }
                int pageSize = 10;
                ProjectMemberViewModel model = new ProjectMemberViewModel() { Project = project, Members = await PaginatedList<ProjectMemberDataModel>.CreateFromListAsync(projectMemberDataList, pageNumber ?? 1, pageSize) };
                if (await ProjectFunctionAccessAsync(project.Id))
                {
                    model.IsOwner = true;
                }
                // Is user(Admin) a member?
                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (await _projectRoleRepository.IsUserInProjectAsync(currentUserId, (int)id))
                {
                    model.IsMember = true;
                }
                return View(model);
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // GET: Projects/AddMember
        public async Task<IActionResult> AddMemberAsync(int? id, string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {

            if (id == null)
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync((int)id))
            {
                ViewData["FirstnameSortParm"] = sortOrder == "Firstname" ? "firstname_desc" : "Firstname";
                ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
                ViewData["SurnameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "surname_desc" : "";

                var users = await _projectRoleRepository.GetAllNonMembersOfProjectAsync((int)id);
                var roleList = await _projectRoleRepository.GetAllProjectRolesAsync();
                ViewData["Users"] = new SelectList(users, "Id", "UserName");
                ViewData["Roles"] = new SelectList(roleList, "Id", "Title");

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
                    users = users.Where(s => s.UserName.Contains(searchString)).ToList();
                }
                ViewData["CurrentSort"] = sortOrder;
                switch (sortOrder)
                {
                    case "firstname_desc":
                        users = users.OrderByDescending(s => s.Firstname).ToList();
                        break;
                    case "Firstname":
                        users = users.OrderBy(s => s.Firstname).ToList();
                        break;
                    case "Date":
                        users = users.OrderBy(s => s.CreateDate).ToList();
                        break;
                    case "date_desc":
                        users = users.OrderByDescending(s => s.CreateDate).ToList();
                        break;
                    case "surname_desc":
                        users = users.OrderByDescending(s => s.Surname).ToList();
                        break;
                    default:
                        users = users.OrderBy(s => s.Surname).ToList();
                        break;
                }


                int pageSize = 20;
                return View(new ProjectMemberViewModel() { ProjectId = (int)id, NonMemberList = await PaginatedList<User>.CreateFromListAsync(users, pageNumber ?? 1, pageSize) });
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // POST: Projects/AddMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember([FromForm] ProjectMemberViewModel model)
        {
            await _projectRoleRepository.AddProjectMemberEntryAsync(model.UserId, model.ProjectRoleId, model.ProjectId);
            // Newsfeed message
            var project = await _projectRepository.GetProjectByIdAsync(model.ProjectId);
            var message = "Du wurdest dem Projekt - " + project.Title + " - hinzugefügt.";
            await _dashboardRepository.CreateNewsfeedEntryAsync(model.UserId, message);
            return RedirectToAction(nameof(Members), new RouteValueDictionary(new { controller = "Project", action = "Members", Id = model.ProjectId }));

        }

        // GET: Projects/EditMember/5
        public async Task<IActionResult> EditMember(int? projectId, string userId)
        {
            if (projectId == null || userId == "")
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync((int)projectId))
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                var role = await _projectRoleRepository.GetProjectRoleOfUserAsync(userId, (int)projectId);
                var roleList = await _projectRoleRepository.GetAllProjectRolesAsync();
                ViewData["Roles"] = new SelectList(roleList, "Id", "Title");
                ProjectMemberViewModel model = new ProjectMemberViewModel() { ProjectId = (int)projectId, User = user, UserId = userId, ProjectRole = role, ProjectRoleId = role.Id, ProjectRoleList = roleList };
                return View(model);
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // POST: Projects/EditMember/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMember([FromForm] ProjectMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _projectRoleRepository.UpdateProjectMemberRoleAsync(model.ProjectId, model.UserId, model.ProjectRoleId, model.NewProjectRoleId);
                return RedirectToAction(nameof(Members), new RouteValueDictionary(new { controller = "Project", action = "Member", Id = model.ProjectId }));
            }
            return View(model);
        }

        // GET: Projects/RemoveMember/5
        public async Task<IActionResult> RemoveMember(int? projectId, string userId)
        {
            if (projectId == null || userId == "")
            {
                return NotFound();
            }
            if (await ProjectFunctionAccessAsync((int)projectId))
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                return View(new RemoveMemberFromProjectViewModel() { ProjectId = (int)projectId, UserId = userId, User = user });
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // POST: Projects/RemoveMember/5
        [HttpPost, ActionName("RemoveMember")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMemberConfirmed([FromForm] RemoveMemberFromProjectViewModel model)
        {
            await _projectRoleRepository.RemoveMemberFromProjectAsync(model.ProjectId, model.UserId);
            // Newsfeed message
            var project = await _projectRepository.GetProjectByIdAsync(model.ProjectId);
            var message = "Du wurdest aus dem Projekt - " + project.Title + " - entfernt!";
            await _dashboardRepository.CreateNewsfeedEntryAsync(model.UserId, message);
            return RedirectToAction(nameof(Members), new RouteValueDictionary(new { controller = "Project", action = "Members", Id = model.ProjectId }));
        }

        // GET: Projects/ExitProject/5
        public IActionResult ExitProject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View(new ExitProjectViewModel { ProjectId = (int)id });
        }

        // POST: Projects/ExitProject/5
        [HttpPost, ActionName("ExitProject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExitProject([FromForm] ExitProjectViewModel model)
        {
            await _projectRoleRepository.RemoveMemberFromProjectAsync(model.ProjectId, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return RedirectToAction(nameof(Index), new RouteValueDictionary(new { controller = "Project", action = "Index" }));
        }
    }
}
