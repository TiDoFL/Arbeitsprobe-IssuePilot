using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        [Required]
        public string Text { set; get; }
        public DateTime CreateDate { set; get; }
        public virtual User Creator { set; get; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}