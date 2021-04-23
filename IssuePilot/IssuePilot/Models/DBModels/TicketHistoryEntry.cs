using IssuePilot.Models.DBModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models
{
    public class TicketHistoryEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual User EntryCreator { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public DateTime EntryDate { get; set; }

        public EntryCaseId EntryCaseId { get; set; }
        public virtual EntryCase EntryCase { get; set; }
    }
}
