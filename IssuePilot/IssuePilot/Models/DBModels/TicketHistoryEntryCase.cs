using System.Collections.Generic;

namespace IssuePilot.Models.DBModels
{
    public class EntryCase
    {
        public EntryCaseId EntryCaseId { get; set; }
        public string EntryCaseName { get; set; }
        public virtual List<TicketHistoryEntry> TicketHistoryEntries { get; set; }
    }
}
