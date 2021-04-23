using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels
{
    public class ProjectViewModel : ProjectBaseModel
    {
        public string Creator { get; set; }
        public List<TicketCategory> TicketCategories { get; set; }
        public bool IsOwner { get; set; }
    }
}
