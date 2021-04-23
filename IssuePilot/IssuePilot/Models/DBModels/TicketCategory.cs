﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models
{
    public class TicketCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public virtual Project Project { set; get; }
        public virtual ICollection<TicketProjectCategory> TicketProjectCategories { get; set; }
    }
}
