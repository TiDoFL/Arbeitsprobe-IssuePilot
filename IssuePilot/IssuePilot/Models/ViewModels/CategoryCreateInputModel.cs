using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class CategoryCreateInputModel
    {
        public int CategoryId { set; get; }
        [Required]
        public string Name { set; get; }
        public int ProjectId { set; get; }
    }
}
