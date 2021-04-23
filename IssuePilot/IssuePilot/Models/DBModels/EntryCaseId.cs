using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssuePilot.Models.DBModels
{
    public enum EntryCaseId : int
    {
        UserAdded,
        UserRemoved,
        TicketClosed,
        TicketOpened,
        TicketCanceled,
        TicketPaused,
        TicketInProgress
    }
}
