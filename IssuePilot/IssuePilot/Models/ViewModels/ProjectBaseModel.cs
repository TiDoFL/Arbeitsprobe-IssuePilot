using System;
using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class ProjectBaseModel
    {
        public int Id { set; get; }
        [Required(ErrorMessage = "Ein Projekt muss einen Titel haben!")]
        public string Title { set; get; }
        public string Description { get; set; }
        public DateTime CreateDate { set; get; }
    }
}
