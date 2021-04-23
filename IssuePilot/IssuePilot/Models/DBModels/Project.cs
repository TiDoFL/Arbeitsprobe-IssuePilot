using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        [Required]
        public string Title { set; get; }
        public string Description { get; set; }
        public DateTime CreateDate { set; get; }
        public int DeletedTicketsCount { set; get; }
        public virtual User Creator { get; set; }
        public virtual ICollection<ProjectMemberEntry> ProjectMemberEntries { get; set; }
        public virtual ICollection<TicketCategory> TicketCategories { get; set; }
    }
}