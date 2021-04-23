using System;
using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels.Statistics
{
    public class StatisticsViewModel
    {
        public Project ProjectData { get; set; }
        public int NumberOfTicketsCreated { get; set; }
        public List<NumbersOfTicketStatus> ListNumbersOfTicketStatus { get; set; }
        public List<NumberOfCreatedTicketsByUser> ListNumberOfCreatedTicketsByUsers { get; set; }
        public List<NumberOfTicketsAssignedToUser> ListNumberOfTicketsAssignedToUser { get; set; }
        public int NumberOfDeletedTickets { get; set; }
        public List<CalendarEventModel> ListOfTicketCreatedDate { get; set; }
        public List<CalendarEventModel> ListOfTicketClosedDate { get; set; }
        public List<NumberOfTimesCategoryWasUsed> ListNumberOfTimesCategoryWasUsed { get; set; }
        public int NumberOfMembers { get; set; }
        public int NumberOfTicketsOverDeadline { get; set; }
        public TimeSpan AVGProcessingTimeOfTicketsInDays { get; set; }
    }
}
