namespace IssuePilot.Models.ViewModels
{
    public class CategoryDeleteViewModel
    {
        public int CategoryId { set; get; }
        public TicketCategory TicketCategory { set; get; }
        public int ProjectId { set; get; }
        public Project Project { set; get; }
    }
}
