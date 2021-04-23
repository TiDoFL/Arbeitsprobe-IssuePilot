using System;
using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels.Statistics
{
    public class CalendarEventModel
    {
        public int NumberOfTickets { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
