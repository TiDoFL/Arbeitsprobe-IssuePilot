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
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IssuePilot.Controllers
{
    [Authorize(Roles = "Admin,Projektmanager,Benutzer")]
    public class TicketController : ProjectAccessController
    {
        private readonly IProjectRoleRepository _projectRoleRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IDashboardRepository _dashboardRepository;

        public TicketController(IProjectRoleRepository projectRoleRepository, IProjectRepository projectRepository, ITicketRepository ticketRepository, IDashboardRepository dashboardRepository) : base(projectRoleRepository, projectRepository)
        {
            _projectRoleRepository = projectRoleRepository;
            _projectRepository = projectRepository;
            _ticketRepository = ticketRepository;
            _dashboardRepository = dashboardRepository;
        }

        // GET: Ticket/Tickets/5
        public async Task<IActionResult> Tickets(int? id, string sortOrder, int? pageNumber, string searchString, string currentFilter, bool open = true, bool inProgress = true, bool canceled = true, bool paused = true, bool closed = true)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync((int)id))
            {
                var project = await _projectRepository.GetProjectByIdAsync((int)id);
                if (project == null)
                {
                    return NotFound();
                }

                List<Ticket> tickets = await _ticketRepository.GetTicketsOfProjectAsync((int)id);

                ViewData["DateAscSortParm"] = "date_asc";
                ViewData["DateDescSortParm"] = "";
                ViewData["TitleAscSortParm"] = "title";
                ViewData["TitleDescSortParm"] = "title_desc";
                ViewData["WeightAscSortParm"] = "weight";
                ViewData["WeightDescSortParm"] = "weight_desc";
                ViewData["DeadlineAscSortParm"] = "deadline";
                ViewData["DeadlineDescSortParm"] = "deadline_desc";

                ViewData["Open"] = open != true;
                ViewData["InProgress"] = inProgress != true;
                ViewData["Canceled"] = canceled != true;
                ViewData["Paused"] = paused != true;
                ViewData["Closed"] = closed != true;
                ViewData["OpenIsChecked"] = open != false;
                ViewData["InProgressIsChecked"] = inProgress != false;
                ViewData["CanceledIsChecked"] = canceled != false;
                ViewData["PausedIsChecked"] = paused != false;
                ViewData["ClosedIsChecked"] = closed != false;

                if (!open)
                {
                    tickets = tickets.Except(tickets.Where(r => r.Status.Id == 1)).ToList();
                }
                if (!closed)
                {
                    tickets = tickets.Except(tickets.Where(r => r.Status.Id == 2)).ToList();
                }
                if (!canceled)
                {
                    tickets = tickets.Except(tickets.Where(r => r.Status.Id == 3)).ToList();
                }
                if (!paused)
                {
                    tickets = tickets.Except(tickets.Where(r => r.Status.Id == 4)).ToList();
                }
                if (!inProgress)
                {
                    tickets = tickets.Except(tickets.Where(r => r.Status.Id == 5)).ToList();
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
                    List<Ticket> copy = new List<Ticket>();
                    copy.AddRange(tickets);
                    foreach (var ticket in copy)
                    {
                        if (ticket.Description == null)
                        {
                            if (!ticket.Title.Contains(searchString) && !ticket.TicketCreator.UserName.Contains(searchString))
                            {
                                tickets.Remove(ticket);
                            }
                        }
                        else
                        {
                            if (!ticket.Title.Contains(searchString) && !ticket.TicketCreator.UserName.Contains(searchString) && !ticket.Description.Contains(searchString))
                            {
                                tickets.Remove(ticket);
                            }
                        }
                    }
                }

                ViewData["CurrentSort"] = sortOrder;

                switch (sortOrder)
                {
                    case "deadline":
                        tickets = tickets.OrderBy(s => s.Deadline).ToList();
                        break;
                    case "deadline_desc":
                        tickets = tickets.OrderByDescending(s => s.Deadline).ToList();
                        break;
                    case "weight":
                        tickets = tickets.OrderBy(s => s.Weight).ToList();
                        break;
                    case "weight_desc":
                        tickets = tickets.OrderByDescending(s => s.Weight).ToList();
                        break;
                    case "title_desc":
                        tickets = tickets.OrderByDescending(s => s.Title).ToList();
                        break;
                    case "title":
                        tickets = tickets.OrderBy(s => s.Title).ToList();
                        break;
                    case "date_asc":
                        tickets = tickets.OrderBy(s => s.CreateDate).ToList();
                        break;
                    default:
                        tickets = tickets.OrderByDescending(s => s.CreateDate).ToList();
                        sortOrder = "";
                        break;
                }

                var categoriesOfProject = await _projectRepository.GetAllTicketCategoriesOfProjectAsync((int)id);
                var ticketProjectCategoriesOfProject = await _ticketRepository.GetTicketProjectCategoriesOfProjectAsync(categoriesOfProject);
                int pageSize = 10;
                TicketListViewModel model = new TicketListViewModel()
                {
                    Tickets = await PaginatedList<Ticket>.CreateFromListAsync(tickets, pageNumber ?? 1, pageSize),
                    Project = project,
                    TicketCategoriesOfProject = categoriesOfProject,
                    TicketProjectCategoriesOfProject = ticketProjectCategoriesOfProject,
                    CurrentSortOrder = sortOrder
                };
                return View(model);
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // GET: Ticket/Details/5
        public async Task<IActionResult> Details(int? id, int? pageNumber)
        {
            if (id == null)
            {
                return NotFound();
            }
            Ticket ticket = await _ticketRepository.GetTicketByIdAsync((int)id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(ticket.Project.Id))
            {
                var model = await GetTicketDetailsById(ticket, pageNumber);
                return View(model);
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // POST: Ticket/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([FromForm] TicketDetailsViewModel model, string submit)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                switch (submit)
                {
                    case "Status ändern":
                        if (model.SelectedTicketStatus == "2")
                        {
                            await _ticketRepository.CloseTicket(currentUserId, model.Id);
                        }
                        await _ticketRepository.UpdateTicketStatus(model.SelectedTicketStatus, currentUserId, model.Id);
                        break;
                    case "Sich selbst zuweisen":
                        List<string> assigneeId = new List<string>() { currentUserId };
                        await _ticketRepository.UpdateTicketWorkersAsync(assigneeId, model.Id, currentUserId);
                        break;
                    case "Zugewiesene Mitglieder aktualisieren":
                        await _ticketRepository.UpdateTicketWorkersAsync(model.SelectedAssigneesIds, model.Id, currentUserId);
                        break;
                    case "Kommentar hinzufügen":
                        if (model.CommentInputText != null)
                        {
                            await _ticketRepository.AddCommentAsync(model.CommentInputText, currentUserId, model.Id);
                        }
                        break;
                    case "Löschen":
                        if (model.SelectedCommendUserId == currentUserId || model.IsProjectOwner)
                        {
                            await _ticketRepository.DeleteCommentAsync(model.SelectedCommentId);
                        }
                        break;
                }
                Ticket ticket = await _ticketRepository.GetTicketByIdAsync(model.Id);
                var updatedModel = await (GetTicketDetailsById(ticket, 1));
                return View(updatedModel);
            }
            return View(model);
        }

        private async Task<TicketDetailsViewModel> GetTicketDetailsById(Ticket ticket, int? pageNumber)
        {
            var categoriesOfProject = await _projectRepository.GetAllTicketCategoriesOfProjectAsync(ticket.Project.Id);
            List<string> ticketCategories;
            List<int> ticketCategoriesIds;
            (ticketCategories, ticketCategoriesIds) = await _ticketRepository.GetCategoriesOfTicketAsync(ticket.Id, categoriesOfProject);

            List<Comment> comments = await _ticketRepository.GetCommentsOfTicket(ticket.Id);
            int pageSize = 10;

            List<string> assignees;
            List<string> assigneesIds;
            (assignees, assigneesIds) = await _ticketRepository.GetAssignees(ticket.Id);
            var members = await _projectRoleRepository.GetMembersOfProjectByIdAsync(ticket.Project.Id);
            List<SelectListItem> memberList = new List<SelectListItem>();
            foreach (var member in members)
            {
                memberList.Add(new SelectListItem() { Text = member.UserName, Value = member.Id });
            }

            List<TicketStatus> statuses = await _ticketRepository.GetTicketStatuses();
            SelectList statusList = new SelectList(statuses, "Id", "Name", ticket.Status.Id);
            string closedByUser = await _ticketRepository.GetClosedByUserOfTicketAsync(ticket.Id);
            TicketDetailsViewModel model = new TicketDetailsViewModel()
            {
                CurrentTicketStatus = ticket.Status.Name,
                Description = ticket.Description,
                Title = ticket.Title,
                Weight = ticket.Weight,
                Id = ticket.Id,
                ProjectId = ticket.Project.Id,
                Deadline = ticket.Deadline,
                CreateDate = ticket.CreateDate,
                CloseDate = ticket.CloseDate,
                CategoriesOfTicket = ticketCategories,
                CommentsOfTicket = await PaginatedList<Comment>.CreateFromListAsync(comments, pageNumber ?? 1, pageSize),
                SelectedAssigneesIds = assigneesIds,
                SelectedAssignees = assignees,
                StatusList = statusList,
                MemberList = memberList,
                ClosedByUser = closedByUser,
                CreatedByUser = ticket.TicketCreator.UserName
            };

            if (await ProjectFunctionAccessAsync(ticket.Project.Id))
            {
                model.IsProjectOwner = true;
            }
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserId == ticket.TicketCreator.Id)
            {
                model.IsCreator = true;
            }

            var images = ticket.Images;
            if (images != null)
            {
                List<string> imageDataURLList = new List<string>();
                foreach (var image in images)
                {
                    string imageBase64Data = Convert.ToBase64String(image.ImageData);
                    string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                    imageDataURLList.Add(imageDataURL);
                }
                model.ImageDataURLList = imageDataURLList;
            }
            return model;
        }

        // GET: Ticket/TicketHistory/5
        public async Task<IActionResult> TicketHistory(int? id, int? pageNumber)
        {
            if (id == null)
            {
                return NotFound();
            }
            Ticket ticket = await _ticketRepository.GetTicketByIdAsync((int)id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(ticket.Project.Id))
            {
                List<TicketHistoryEntry> ticketHistoryEntriesOfTicket = ticket.TicketHistoryEntries.ToList();
                int pageSize = 30;
                TicketHistoryViewModel model = new TicketHistoryViewModel()
                {
                    TicketHistoryEntriesOfTicket = await PaginatedList<TicketHistoryEntry>.CreateFromListAsync(ticketHistoryEntriesOfTicket, pageNumber ?? 1, pageSize),
                    Id = (int)id,
                    Title = ticket.Title,
                    CreateDate = ticket.CreateDate,
                    CreatorName = ticket.TicketCreator.UserName
                };
                return View(model);
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // GET: Ticket/SelectProject
        public async Task<IActionResult> SelectProject(string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {
            return View(await GetProjectsAsync(sortOrder, pageNumber, currentFilter, searchString));
        }

        // GET: Ticket/Create/5
        // id is id of project
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = await _projectRepository.GetProjectByIdAsync((int)id);
            if (project == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync((int)id))
            {
                List<TicketCategory> categoriesOfProject = await _projectRepository.GetAllTicketCategoriesOfProjectAsync((int)id);
                List<SelectListItem> categoryList = new List<SelectListItem>();
                foreach (var categoryOfProject in categoriesOfProject)
                {
                    categoryList.Add(new SelectListItem() { Text = categoryOfProject.Name, Value = categoryOfProject.Id.ToString() });
                }

                var members = await _projectRoleRepository.GetMembersOfProjectByIdAsync((int)id);
                List<SelectListItem> memberList = new List<SelectListItem>();
                foreach (var member in members)
                {
                    memberList.Add(new SelectListItem() { Text = member.UserName, Value = member.Id });
                }

                return View(new TicketCreateInputModel() { ProjectId = (int)id, CategoriesOfProject = categoryList, Members = memberList });

            }
            return RedirectToAction(nameof(NoAccess));
        }

        // POST: Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] TicketCreateInputModel model)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                List<Image> imgList = CreateImageList();
                var project = await _projectRepository.GetProjectByIdAsync(model.ProjectId);
                var ticket = await _ticketRepository.CreateTicketAsync(model, project, currentUserId, imgList);

                // Newsfeed
                var message = "Es wurde das Ticket - " + ticket.Title + " - im Projekt - " + project.Title + " - erstellt.";
                await _dashboardRepository.CreateNewsfeedEntryForAllMembersAsync(model.ProjectId, message);

                return RedirectToAction(nameof(Tickets), new RouteValueDictionary(new { controller = "Ticket", action = "Tickets", Id = model.ProjectId }));
            }

            return View(model);
        }

        // GET: Ticket/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket ticket = await _ticketRepository.GetTicketByIdAsync((int)id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(ticket.Project.Id))
            {
                if (ticket.TicketCreator.UserName == User.Identity.Name || await ProjectFunctionAccessAsync(ticket.Project.Id))
                {
                    List<TicketCategory> categoriesOfProject = await _projectRepository.GetAllTicketCategoriesOfProjectAsync(ticket.Project.Id);
                    List<SelectListItem> categoriesOfProjectList = new List<SelectListItem>();
                    List<string> categoriesOfTicket;
                    List<int> categoriesOfTicketIds;
                    (categoriesOfTicket, categoriesOfTicketIds) = await _ticketRepository.GetCategoriesOfTicketAsync((int)id, categoriesOfProject);
                    foreach (var categoryOfProject in categoriesOfProject)
                    {
                        categoriesOfProjectList.Add(new SelectListItem() { Text = categoryOfProject.Name, Value = categoryOfProject.Id.ToString() });
                    }

                    var images = ticket.Images;
                    List<SelectableImage> imageList = new List<SelectableImage>();

                    foreach (var image in images)
                    {
                        string imageBase64Data = Convert.ToBase64String(image.ImageData);
                        string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                        imageList.Add(new SelectableImage() { ImageData = imageDataURL, ImageId = image.Id.ToString(), IsSelected = false });
                    }

                    return View(new TicketEditViewModel()
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        Description = ticket.Description,
                        Deadline = ticket.Deadline,
                        Weight = ticket.Weight,
                        SelectedTicketCategories = categoriesOfTicketIds,
                        OldTicketCategories = categoriesOfTicketIds,
                        ImageList = imageList,
                        CategoriesOfProject = categoriesOfProjectList
                    });
                }

                return RedirectToAction(nameof(NoAccess));
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // POST: Ticket/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] TicketEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<Image> imgList = CreateImageList();
                await _ticketRepository.UpdateTicketAsync(model, imgList);

                return RedirectToAction(nameof(Details), new RouteValueDictionary(new { controller = "Ticket", action = "Details", Id = model.Id }));
            }
            return View(model);
        }

        private List<Image> CreateImageList()
        {
            List<Image> imgList = new List<Image>();
            foreach (var file in Request.Form.Files)
            {
                Image img = new Image();
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                img.ImageData = ms.ToArray();

                ms.Close();
                ms.Dispose();

                imgList.Add(img);
            }
            return imgList;
        }

        // GET: Ticket/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ticket = await _ticketRepository.GetTicketByIdAsync((int)id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (await ProjectAccessAsync(ticket.Project.Id))
            {
                if (ticket.TicketCreator.UserName == User.Identity.Name || await ProjectFunctionAccessAsync(ticket.Project.Id))
                {
                    return View(ticket);
                }
                return RedirectToAction(nameof(NoAccess));
            }
            return RedirectToAction(nameof(NoAccess));
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(id);
            var projectId = ticket.Project.Id;
            await _ticketRepository.DeleteTicketAsync((int)id);
            return RedirectToAction(nameof(Tickets), new RouteValueDictionary(new { controller = "Ticket", action = "Tickets", Id = projectId }));
        }
    }
}
