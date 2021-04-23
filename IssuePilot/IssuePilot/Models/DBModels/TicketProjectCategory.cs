using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models
{
    public class TicketProjectCategory
    {
        [Key]
        [Column(Order = 1)]
        public int FK_TicketId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int FK_TicketCategoryId { get; set; }

        [ForeignKey("FK_TicketId")]
        public virtual Ticket Ticket { get; set; }
        [ForeignKey("FK_TicketCategoryId")]
        public virtual TicketCategory TicketCategory { get; set; }
    }
}