using IssuePilot.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels
{
    public class TicketEditViewModel : TicketBaseModel
    {
        public List<int> SelectedTicketCategories { get; set; }
        public List<int> OldTicketCategories { get; set; }
        public List<SelectableImage> ImageList { get; set; }
        public List<SelectListItem> CategoriesOfProject { get; set; }
    }
}
