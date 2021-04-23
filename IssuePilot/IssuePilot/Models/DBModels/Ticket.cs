using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        [Required]
        public string Title { set; get; }
        public DateTime? Deadline { set; get; }
        public string Description { get; set; }
        [Required]
        public virtual TicketStatus Status { get; set; }
        public virtual Project Project { get; set; }
        public DateTime CreateDate { set; get; }
        public DateTime? CloseDate { set; get; }
        public int Weight { get; set; }
        public virtual User ClosedFromUser { get; set; }
        public virtual User TicketCreator { get; set; }

        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<TicketProjectCategory> TicketProjectCategories { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<TicketHistoryEntry> TicketHistoryEntries { get; set; }
        public virtual ICollection<TicketWorker> TicketWorkers { get; set; }
    }
}
