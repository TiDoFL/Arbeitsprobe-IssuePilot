using IssuePilot.Data;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssuePilot.Models.Repositorys
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDashboardRepository _dashboardRepository;
        public TicketRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public TicketRepository(ApplicationDbContext context, IDashboardRepository dashboardRepository)
        {
            this._context = context;
            this._dashboardRepository = dashboardRepository;
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets.FindAsync(ticketId);
        }

        public async Task<List<TicketProjectCategory>> GetTicketProjectCategoriesOfProjectAsync(List<TicketCategory> categoriesOfProject)
        {
            List<TicketProjectCategory> ticketProjectCategoriesOfProject = new List<TicketProjectCategory>();
            await foreach (var ticketProjectCategory in _context.TicketProjectCategories)
            {
                foreach (var category in categoriesOfProject)
                {
                    if (category.Id == ticketProjectCategory.FK_TicketCategoryId)
                    {
                        ticketProjectCategoriesOfProject.Add(ticketProjectCategory);
                        break;
                    }
                }
            }
            return ticketProjectCategoriesOfProject;
        }


        public async Task<List<Ticket>> GetTicketsOfProjectAsync(int projectId)
        {
            return await _context.Tickets.Where(r => r.Project.Id == projectId).ToListAsync();
        }

        public async Task<Ticket> CreateTicketAsync(TicketCreateInputModel model, Project project, String userId, List<Image> imgList)
        {
            if (userId == null || model == null || model.Title == null || project == null) { throw new ArgumentNullException(); }
            Ticket ticket = new Ticket
            {
                Title = model.Title,
                Description = model.Description,
                CreateDate = DateTime.Now,
                Project = project,
                Deadline = model.Deadline,
                Status = await _context.TicketStatuses.FirstOrDefaultAsync(r => r.Id == 1),
                TicketCreator = await _context.Users.FindAsync(userId),
                Weight = model.Weight,
                Images = imgList
            };
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            if (model.SelectedTicketCategories != null)
            {
                await CreateTicketProjectCategoriesAsync(ticket.Id, model.SelectedTicketCategories);
            }
            if (model.SelectedAssignees != null)
            {
                await CreateTicketWorkersAsync(ticket.Id, model.SelectedAssignees);
            }

            return ticket;
        }
        public async Task CreateTicketProjectCategoriesAsync(int id, List<string> selectedTicketCategories)
        {
            if (selectedTicketCategories == null) { throw new ArgumentNullException(); }
            foreach (var selectedTicketCategory in selectedTicketCategories)
            {
                TicketProjectCategory ticketProjectCategory = new TicketProjectCategory
                {
                    FK_TicketCategoryId = int.Parse(selectedTicketCategory),
                    FK_TicketId = id
                };
                await _context.TicketProjectCategories.AddAsync(ticketProjectCategory);
            }
            await _context.SaveChangesAsync();
        }

        public async Task CreateTicketWorkersAsync(int ticketId, List<string> selectedAssignees)
        {
            foreach (var selectedAssignee in selectedAssignees)
            {
                TicketWorker ticketWorker = new TicketWorker
                {
                    FK_UserId = selectedAssignee,
                    FK_TicketId = ticketId
                };
                await _context.TicketWorkers.AddAsync(ticketWorker);
                await _dashboardRepository.CreateNewsfeedEntryAsync(selectedAssignee, "Dir wurde das Ticket -" + ticketWorker.Ticket.Title + "- des Projekts -" + ticketWorker.Ticket.Project.Title + "- zugewiesen.");
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicketWorkersAsync(List<string> selectedAssigneesIds, int ticketId, string currentUserId)
        {
            if (selectedAssigneesIds == null)
            {
                selectedAssigneesIds = new List<string>();
            }
            var listOfAssignees = await _context.TicketWorkers.Where(c => c.Ticket.Id == ticketId).ToListAsync();
            var entryCreator = await _context.Users.FirstOrDefaultAsync(r => r.Id == currentUserId);
            var ticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == ticketId);

            // add new workers
            foreach (var selected in selectedAssigneesIds)
            {
                if (listOfAssignees.FirstOrDefault(c => c.User.Id == selected) == null)
                {
                    TicketWorker ticketWorker = new TicketWorker
                    {
                        FK_UserId = selected,
                        FK_TicketId = ticketId
                    };
                    await _context.TicketWorkers.AddAsync(ticketWorker);
                    await CreateTicketHistoryEntryAsync(ticket, entryCreator, "Add", selected);
                    await _dashboardRepository.CreateNewsfeedEntryAsync(selected, "Dir wurde das Ticket - " + ticketWorker.Ticket.Title + "- des Projekts -" + ticketWorker.Ticket.Project.Title + "- zugewiesen.");
                }
            }

            // remove old workers
            foreach (var oldAssignee in listOfAssignees)
            {
                if (!selectedAssigneesIds.Contains(oldAssignee.User.Id))
                {
                    _context.TicketWorkers.Remove(_context.TicketWorkers.Find(ticketId, oldAssignee.User.Id));
                    await CreateTicketHistoryEntryAsync(ticket, entryCreator, "Remove", oldAssignee.User.Id);
                    await _dashboardRepository.CreateNewsfeedEntryAsync(oldAssignee.User.Id, "Du wurdest vom Ticket - " + oldAssignee.Ticket.Title + "- des Projekts -" + oldAssignee.Ticket.Project.Title + "- entfernt.");
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task CreateTicketHistoryEntryAsync(Ticket ticket, User entryCreator, string entryCase, string assigneeId = "optional")
        {
            TicketHistoryEntry newEntry = new TicketHistoryEntry();
            switch (entryCase)
            {
                case "1":
                    newEntry.EntryCaseId = DBModels.EntryCaseId.TicketOpened;
                    break;
                case "2":
                    newEntry.EntryCaseId = DBModels.EntryCaseId.TicketClosed;
                    break;
                case "3":
                    newEntry.EntryCaseId = DBModels.EntryCaseId.TicketCanceled;
                    break;
                case "4":
                    newEntry.EntryCaseId = DBModels.EntryCaseId.TicketPaused;
                    break;
                case "5":
                    newEntry.EntryCaseId = DBModels.EntryCaseId.TicketInProgress;
                    break;
                case "Remove":
                    newEntry.EntryCaseId = DBModels.EntryCaseId.UserRemoved;
                    var removedUser = await _context.Users.FirstOrDefaultAsync(r => r.Id == assigneeId);
                    newEntry.User = removedUser;
                    break;
                case "Add":
                    newEntry.EntryCaseId = DBModels.EntryCaseId.UserAdded;
                    var addedUser = await _context.Users.FirstOrDefaultAsync(r => r.Id == assigneeId);
                    newEntry.User = addedUser;
                    break;
            }
            newEntry.EntryDate = DateTime.Now;
            newEntry.EntryCreator = entryCreator;
            newEntry.Ticket = ticket;
            await _context.TicketHistoryEntries.AddAsync(newEntry);
        }
        public async Task<(List<string>, List<int>)> GetCategoriesOfTicketAsync(int ticketId, List<TicketCategory> categoriesOfProject)
        {
            var ticketProjectCategories = await GetTicketProjectCategoriesOfProjectAsync(categoriesOfProject);
            List<string> ticketCategoriesOfTicket = new List<string>();
            List<int> ticketCategoriesOfTicketIds = new List<int>();
            foreach (var ticketProjectCategory in ticketProjectCategories)
            {
                if (ticketProjectCategory.FK_TicketId == ticketId)
                {
                    foreach (var ticketCategory in categoriesOfProject)
                    {
                        if (ticketCategory.Id == ticketProjectCategory.FK_TicketCategoryId)
                        {
                            ticketCategoriesOfTicket.Add(ticketCategory.Name);
                            ticketCategoriesOfTicketIds.Add(ticketCategory.Id);
                            break;
                        }
                    }
                }
            }

            return (ticketCategoriesOfTicket, ticketCategoriesOfTicketIds);
        }
        public async Task<List<Comment>> GetCommentsOfTicket(int id)
        {
            return await _context.Comments.Where(r => r.TicketId == id).ToListAsync();
        }
        public async Task<(List<string>, List<string>)> GetAssignees(int id)
        {
            var ticketWorkersOfTicket = await _context.TicketWorkers.Where(r => r.FK_TicketId == id).ToListAsync();
            List<string> assigneesIds = new List<string>();
            List<string> assignees = new List<string>();
            foreach (var ticketWorker in ticketWorkersOfTicket)
            {
                foreach (var user in _context.Users)
                {
                    if (user.Id == ticketWorker.FK_UserId)
                    {
                        assigneesIds.Add(user.Id);
                        assignees.Add(user.UserName);
                        break;
                    }
                }

            }
            return (assignees, assigneesIds);
        }
        public async Task<List<TicketStatus>> GetTicketStatuses()
        {
            return await _context.TicketStatuses.ToListAsync();
        }
        public async Task CloseTicket(string currentUserId, int ticketId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(r => r.Id == currentUserId);
            var ticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == ticketId);
            ticket.CloseDate = DateTime.Now;
            ticket.ClosedFromUser = user;
            await _context.SaveChangesAsync();
            await _dashboardRepository.CreateNewsdeedEntryForTicketStatusChangeAsync(ticket);
        }
        public async Task UpdateTicketStatus(string selectedTicketStatus, string currentUserId, int ticketId)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == ticketId);
            var status = await _context.TicketStatuses.FirstOrDefaultAsync(r => r.Id == int.Parse(selectedTicketStatus));
            ticket.Status = status;
            var entryCreator = await _context.Users.FirstOrDefaultAsync(r => r.Id == currentUserId);
            await CreateTicketHistoryEntryAsync(ticket, entryCreator, selectedTicketStatus);
            await _context.SaveChangesAsync();
            await _dashboardRepository.CreateNewsdeedEntryForTicketStatusChangeAsync(ticket);
        }
        public async Task DeleteTicketAsync(int id)
        {
            _context.Tickets.Remove(await _context.Tickets.FindAsync(id));
            await _context.SaveChangesAsync();
        }
        public async Task UpdateTicketAsync(TicketEditViewModel model, List<Image> imagesToAddList)
        {
            var ticket = await _context.Tickets.FindAsync(model.Id);
            ticket.Title = model.Title;
            ticket.Description = model.Description;
            ticket.Deadline = model.Deadline;
            ticket.Weight = model.Weight;
            foreach (var image in imagesToAddList)
            {
                ticket.Images.Add(image);
            }
            if (model.ImageList != null)
            {
                foreach (var selectableImage in model.ImageList)
                {
                    if (selectableImage.IsSelected)
                    {
                        _context.Images.Remove(await _context.Images.FirstOrDefaultAsync(r => r.Id == int.Parse(selectableImage.ImageId)));
                    }
                }
            }
            if (model.OldTicketCategories == null)
            {
                model.OldTicketCategories = new List<int>();
            }
            if (model.SelectedTicketCategories == null)
            {
                model.SelectedTicketCategories = new List<int>();
            }
            var tagsToRemove = model.OldTicketCategories.Except(model.SelectedTicketCategories).ToList();
            var tagsToAdd = model.SelectedTicketCategories.Except(model.OldTicketCategories).ToList();

            foreach (var tagToRemove in tagsToRemove)
            {
                _context.TicketProjectCategories.Remove(await _context.TicketProjectCategories.FirstOrDefaultAsync(r => (r.FK_TicketCategoryId == tagToRemove) && (r.FK_TicketId == model.Id)));
            }
            foreach (var tagToAdd in tagsToAdd)
            {
                TicketProjectCategory ticketProjectCategory = new TicketProjectCategory
                {
                    FK_TicketCategoryId = tagToAdd,
                    FK_TicketId = model.Id
                };
                await _context.TicketProjectCategories.AddAsync(ticketProjectCategory);
            }
            await _context.SaveChangesAsync();
        }
        public async Task AddCommentAsync(string commentInputText, string userId, int ticketId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(r => r.Id == userId);
            Comment comment = new Comment
            {
                Text = commentInputText,
                CreateDate = DateTime.Now,
                Creator = user
            };
            await _context.Comments.AddAsync(comment);
            var ticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == ticketId);
            ticket.Comments.Add(comment);
            await _context.SaveChangesAsync();
            await _dashboardRepository.CreateNewsdeedEntryForNewCommentAsync(ticket, user.UserName);
        }
        public async Task<string> GetClosedByUserOfTicketAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(r => r.Id == ticketId);
            if (ticket.ClosedFromUser == null)
            {
                return null;
            }
            else
            {
                return ticket.ClosedFromUser.UserName;
            }

        }
        public async Task DeleteCommentAsync(int selectedCommentId)
        {
            if (_context.Comments.Any(r => r.Id == selectedCommentId))
            {
                _context.Comments.Remove(await _context.Comments.FirstOrDefaultAsync(r => r.Id == selectedCommentId));
                await _context.SaveChangesAsync();
            }
        }
    }
}
