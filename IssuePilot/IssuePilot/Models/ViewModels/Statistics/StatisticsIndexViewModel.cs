using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels.Statistics
{
    public class StatisticsIndexViewModel
    {
        public List<Project> Projects { get; set; }
        public int ComparisonProjectIdFirst { get; set; }
        public int ComparisonPojectIdSecond { get; set; }
    }
}
