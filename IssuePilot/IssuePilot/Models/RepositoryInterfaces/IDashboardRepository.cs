using IssuePilot.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface IDashboardRepository
    {
        Task<List<NewsfeedListViewModel>> GetNewsfeedByUserIdAsync(string userId);
        Task CreateNewsfeedEntryAsync(string userId, string text);
        Task CreateNewsfeedEntryForAllMembersAsync(int projectId, string text);
        Task CreateNewsdeedEntryForTicketStatusChangeAsync(Ticket ticket);
        Task CreateNewsdeedEntryForNewCommentAsync(Ticket ticket, string commentCreatorName);
        Task UpdateSeenStatusAsync(int id);
        Task DeleteAllEntriesOfUserAsync(string userId);
    }
}
