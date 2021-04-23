using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models
{
    public class NewsfeedEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        public DateTime CreateDate { set; get; }
        [Required]
        public string NewsText { set; get; }
        public bool Seen { set; get; }
        public virtual User User { get; set; }
    }
}
