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
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectRoleRepository _projectRoleRepository;

        public DashboardRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public DashboardRepository(ApplicationDbContext context, IProjectRoleRepository projectRoleRepository)
        {
            this._projectRoleRepository = projectRoleRepository;
            this._context = context;
        }

        public async Task CreateNewsdeedEntryForNewCommentAsync(Ticket ticket, string commentCreatorName)
        {
            // creator
            await CreateNewsfeedEntryAsync(ticket.TicketCreator.Id, "Das Ticket -" + ticket.Title + "- im Projekt -" + ticket.Project.Title + "- wurde von " + commentCreatorName + " kommientiert.");
            // ticket workers
            foreach (var worker in ticket.TicketWorkers)
            {
                if (worker.User.Id != ticket.TicketCreator.Id)
                {
                    await CreateNewsfeedEntryAsync(worker.User.Id, "Das Ticket -" + ticket.Title + "- im Projekt -" + ticket.Project.Title + "- wurde von " + commentCreatorName + " kommientiert.");
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task CreateNewsdeedEntryForTicketStatusChangeAsync(Ticket ticket)
        {
            // creator
            await CreateNewsfeedEntryAsync(ticket.TicketCreator.Id, "Der Status des Tickets -" + ticket.Title + "- im Projekt -" + ticket.Project.Title + "- wurde zu " + ticket.Status.Name + " geändert.");
            // ticket workers
            foreach (var worker in ticket.TicketWorkers)
            {
                if (worker.User.Id != ticket.TicketCreator.Id)
                {
                    await CreateNewsfeedEntryAsync(worker.User.Id, "Der Status des Tickets -" + ticket.Title + "- im Projekt -" + ticket.Project.Title + "- wurde zu " + ticket.Status.Name + " geändert.");
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task CreateNewsfeedEntryAsync(string userId, string text)
        {
            if (text == null || text == "")
            {
                throw new ArgumentException($"{nameof(CreateNewsfeedEntryAsync)} text can not be null or empty!");
            }
            NewsfeedEntry newsfeedEntry = new NewsfeedEntry
            {
                CreateDate = DateTime.Now,
                NewsText = text,
                Seen = false
            };

            var user = await _context.Users.FindAsync(userId);
            newsfeedEntry.User = user ?? throw new ArgumentNullException();

            await _context.NewsfeedEntries.AddAsync(newsfeedEntry);
            await _context.SaveChangesAsync();
        }

        // Creates an entry for each member of the project.
        public async Task CreateNewsfeedEntryForAllMembersAsync(int projectId, string text)
        {
            if (text == null || text == "")
            {
                throw new ArgumentNullException();
            }
            var members = await _projectRoleRepository.GetMembersOfProjectByIdAsync(projectId);
            foreach (var member in members)
            {
                NewsfeedEntry newsfeedEntry = new NewsfeedEntry
                {
                    CreateDate = DateTime.Now,
                    NewsText = text,
                    Seen = false
                };

                var user = await _context.Users.FindAsync(member.Id);
                newsfeedEntry.User = user ?? throw new Exception();
                await _context.NewsfeedEntries.AddAsync(newsfeedEntry);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllEntriesOfUserAsync(string userId)
        {
            if (userId == null) { throw new ArgumentNullException(); }
            var listOfEntries = await _context.NewsfeedEntries.Where(c => c.User.Id == userId).ToListAsync();
            if (listOfEntries != null)
            {
                _context.NewsfeedEntries.RemoveRange(listOfEntries);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<NewsfeedListViewModel>> GetNewsfeedByUserIdAsync(string userId)
        {
            if (userId == null) { throw new ArgumentNullException(); }
            var dbListOfNewsfeedEntries = await _context.NewsfeedEntries.Where(r => r.User.Id == userId).OrderByDescending(x => x.CreateDate).ToListAsync();
            List<NewsfeedListViewModel> listModel = new List<NewsfeedListViewModel>();
            foreach (var newsfeedEntry in dbListOfNewsfeedEntries)
            {
                NewsfeedListViewModel model = new NewsfeedListViewModel
                {
                    CreateDate = newsfeedEntry.CreateDate,
                    Id = newsfeedEntry.Id,
                    NewsText = newsfeedEntry.NewsText,
                    Seen = newsfeedEntry.Seen
                };
                listModel.Add(model);
            }
            return listModel;
        }

        public async Task UpdateSeenStatusAsync(int id)
        {
            var entry = await _context.NewsfeedEntries.FindAsync(id);
            entry.Seen = true;
            await _context.SaveChangesAsync();
        }
    }
}
