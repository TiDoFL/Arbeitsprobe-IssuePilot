using IssuePilot.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetTicketsOfProjectAsync(int projectId);
        Task<List<TicketProjectCategory>> GetTicketProjectCategoriesOfProjectAsync(List<TicketCategory> categoriesOfProject);
        Task<Ticket> CreateTicketAsync(TicketCreateInputModel model, Project project, string userId, List<Image> imgList);
        Task CreateTicketProjectCategoriesAsync(int id, List<string> selectedTicketCategories);
        Task CreateTicketWorkersAsync(int id, List<string> selectedAssignees);
        Task<Ticket> GetTicketByIdAsync(int id);
        Task<(List<string>, List<int>)> GetCategoriesOfTicketAsync(int ticketId, List<TicketCategory> categoriesOfProject);
        Task<List<Comment>> GetCommentsOfTicket(int id);
        Task<(List<string>, List<string>)> GetAssignees(int id);
        Task<List<TicketStatus>> GetTicketStatuses();
        Task UpdateTicketWorkersAsync(List<string> selectedAssigneesIds, int ticketId, string currentUserId);
        Task CloseTicket(string currentUserId, int ticketId);
        Task UpdateTicketStatus(string selectedTicketStatus, string currentUserId, int ticketId);
        Task DeleteTicketAsync(int id);
        Task UpdateTicketAsync(TicketEditViewModel model, List<Image> imgList);
        Task AddCommentAsync(string commentInputText, string userId, int ticketId);
        Task<string> GetClosedByUserOfTicketAsync(int id);
        Task DeleteCommentAsync(int selectedCommentId);
    }
}
