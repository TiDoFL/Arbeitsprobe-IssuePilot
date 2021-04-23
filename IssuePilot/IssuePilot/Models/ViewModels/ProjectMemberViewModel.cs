using IssuePilot.Helper;
using System.Collections.Generic;

namespace IssuePilot.Models.ViewModels
{
    public class ProjectMemberViewModel
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int ProjectRoleId { get; set; }
        public int NewProjectRoleId { get; set; }
        public List<ProjectRole> ProjectRoleList { get; set; }
        public ProjectRole ProjectRole { get; set; }
        public PaginatedList<User> NonMemberList { get; set; }
        public PaginatedList<ProjectMemberDataModel> Members { get; set; }
        public bool IsOwner { get; set; }
        public bool IsMember { get; set; }
    }
}
