using System;
using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class TicketBaseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ein Ticket muss einen Titel haben!")]
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public DateTime? Deadline { get; set; }
        public int Weight { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsProjectOwner { get; set; }
        public bool IsCreator { get; set; }
    }
}
