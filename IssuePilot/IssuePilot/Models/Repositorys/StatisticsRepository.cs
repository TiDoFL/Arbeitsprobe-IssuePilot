using IssuePilot.Data;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace IssuePilot.Models.Repositorys
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectRoleRepository _projectRoleRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITicketRepository _ticketRepository;
        public StatisticsRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public StatisticsRepository(ApplicationDbContext context, IProjectRoleRepository projectRoleRepository, IProjectRepository projectRepository, ITicketRepository ticketRepository)
        {
            this._context = context;
            _projectRoleRepository = projectRoleRepository;
            _projectRepository = projectRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<StatisticsViewModel> GetProjectStatisticDataAsync(int projectId)
        {
            StatisticsViewModel statisticsModel = new StatisticsViewModel
            {
                ListNumbersOfTicketStatus = new List<NumbersOfTicketStatus>(),
                ListNumberOfCreatedTicketsByUsers = new List<NumberOfCreatedTicketsByUser>(),
                ListNumberOfTicketsAssignedToUser = new List<NumberOfTicketsAssignedToUser>(),
                ListNumberOfTimesCategoryWasUsed = new List<NumberOfTimesCategoryWasUsed>(),
                ListOfTicketCreatedDate = new List<CalendarEventModel>(),
                ListOfTicketClosedDate = new List<CalendarEventModel>()
            };
            var listOfTicketStatus = _context.TicketStatuses.ToList();
            List<User> members = await _projectRoleRepository.GetMembersOfProjectByIdAsync(projectId);
            Project project = await _projectRepository.GetProjectByIdAsync(projectId);
            List<TicketCategory> listOfTicketCategories = await _projectRepository.GetAllTicketCategoriesOfProjectAsync(projectId);

            statisticsModel.ProjectData = project;


            statisticsModel.NumberOfTicketsCreated = _context.Tickets.Where(c => c.Project.Id == projectId).Count();


            foreach (var status in listOfTicketStatus)
            {
                NumbersOfTicketStatus numbersOfTicketStatus = new NumbersOfTicketStatus
                {
                    StatusName = status.Name,
                    NumberOfTicketsWithStatus = _context.Tickets.Where(c => c.Project.Id == projectId && c.Status.Id == status.Id).Count()
                };
                statisticsModel.ListNumbersOfTicketStatus.Add(numbersOfTicketStatus);
            }


            foreach (var member in members)
            {
                NumberOfCreatedTicketsByUser numberOfCreatedTicketsByUser = new NumberOfCreatedTicketsByUser
                {
                    UserName = member.UserName,
                    NumberOfTickets = _context.Tickets.Where(c => c.Project.Id == projectId && c.TicketCreator.Id == member.Id).Count()
                };
                statisticsModel.ListNumberOfCreatedTicketsByUsers.Add(numberOfCreatedTicketsByUser);
            }
            statisticsModel.ListNumberOfCreatedTicketsByUsers = statisticsModel.ListNumberOfCreatedTicketsByUsers.OrderByDescending(o => o.NumberOfTickets).ToList();


            foreach (var user in members)
            {
                NumberOfTicketsAssignedToUser numberOfTicketsAssignedToUser = new NumberOfTicketsAssignedToUser
                {
                    UserName = user.UserName,
                    NumberOfTickets = _context.TicketWorkers.Where(c => c.Ticket.Project.Id == projectId && c.User.Id == user.Id).Count()
                };
                statisticsModel.ListNumberOfTicketsAssignedToUser.Add(numberOfTicketsAssignedToUser);
            }


            statisticsModel.NumberOfDeletedTickets = project.DeletedTicketsCount;


            var listCreateDates = _context.Tickets.Where(c => c.Project.Id == projectId).ToList().GroupBy(c => c.CreateDate.Date).Select(d => new CalendarEventModel() { Date = d.Key.Date, NumberOfTickets = d.Count() });
            foreach (var createDate in listCreateDates)
            {
                statisticsModel.ListOfTicketCreatedDate.Add(createDate);
            }
            var listClosedDates = _context.Tickets.Where(c => c.CloseDate != null && c.Project.Id == projectId).Select(c => new { Closedate = (DateTime)c.CloseDate }).ToList().GroupBy(c => c.Closedate.Date).Select(d => new CalendarEventModel() { Date = d.Key.Date, NumberOfTickets = d.Count() });

            foreach (var closedDate in listClosedDates)
            {
                statisticsModel.ListOfTicketClosedDate.Add(closedDate);
            }


            foreach (var category in listOfTicketCategories)
            {
                NumberOfTimesCategoryWasUsed numberOfTimesCategoryWasUsed = new NumberOfTimesCategoryWasUsed
                {
                    NameOfCategory = category.Name,
                    NumberOfCategoryUses = _context.TicketProjectCategories.Where(c => c.Ticket.Project.Id == projectId && c.FK_TicketCategoryId == category.Id).Count()
                };
                statisticsModel.ListNumberOfTimesCategoryWasUsed.Add(numberOfTimesCategoryWasUsed);
            }
            statisticsModel.ListNumberOfTimesCategoryWasUsed = statisticsModel.ListNumberOfTimesCategoryWasUsed.OrderByDescending(o => o.NumberOfCategoryUses).ToList();


            statisticsModel.NumberOfMembers = _context.ProjectMemberEntries.Where(c => c.FK_ProjectId == projectId).Count();


            // status: 1 = offen 2 = Abgeschlossen 3 = Abgebrochen 4 = Pausiert 5 = In Bearbeitung
            statisticsModel.NumberOfTicketsOverDeadline = _context.Tickets.Where(c => c.Project.Id == projectId && c.Status.Id != 2 && c.Deadline < DateTime.Now).Count();


            var listOfClosedTickets = _context.Tickets.Where(c => c.Project.Id == projectId && c.Status.Id == 2 && c.CreateDate != null && c.CloseDate != null).ToList();
            if (listOfClosedTickets != null && listOfClosedTickets.Count > 0)
            {
                var listOfTimeSpans = new List<double>();
                foreach (var ticket in listOfClosedTickets)
                {
                    listOfTimeSpans.Add((ticket.CloseDate - ticket.CreateDate).Value.TotalMilliseconds);
                }
                statisticsModel.AVGProcessingTimeOfTicketsInDays = TimeSpan.FromMilliseconds(listOfTimeSpans.Average());
            }

            return statisticsModel;
        }
    }
}
