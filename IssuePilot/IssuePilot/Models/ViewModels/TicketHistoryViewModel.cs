using IssuePilot.Helper;
using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels
{
    public class TicketHistoryViewModel : TicketBaseModel
    {
        public PaginatedList<TicketHistoryEntry> TicketHistoryEntriesOfTicket { get; set; }
        public string CreatorName { get; set; }
    }
}
