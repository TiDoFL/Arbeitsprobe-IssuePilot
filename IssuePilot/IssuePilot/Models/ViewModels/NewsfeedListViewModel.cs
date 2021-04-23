using System;

namespace IssuePilot.Models.ViewModels
{
    public class NewsfeedListViewModel
    {
        public int Id { set; get; }
        public DateTime CreateDate { set; get; }
        public string NewsText { set; get; }
        public bool Seen { set; get; }
    }
}
