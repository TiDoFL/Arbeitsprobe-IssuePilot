namespace IssuePilot.Models.ViewModels
{
    public class ProjectUpdateViewModel : ProjectBaseModel
    {
        public int DeletedTicketsCount { set; get; }
        public User Creator { get; set; }
        public bool TitleExists { get; set; }
    }
}
