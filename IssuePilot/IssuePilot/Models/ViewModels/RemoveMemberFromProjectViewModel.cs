namespace IssuePilot.Models.ViewModels
{
    public class RemoveMemberFromProjectViewModel
    {
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
