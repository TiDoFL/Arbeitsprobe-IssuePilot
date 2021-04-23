using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models
{
    public class User : IdentityUser
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public DateTime CreateDate { set; get; }
        public bool IsDeleted { set; get; }
        public virtual ICollection<ProjectMemberEntry> ProjectMemberEntries { get; set; }
        public virtual ICollection<NewsfeedEntry> NewsfeedEntries { get; set; }
        [InverseProperty("TicketCreator")]
        public virtual ICollection<Ticket> CreatedTickets { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<TicketWorker> TicketWorkers { get; set; }
    }
}
