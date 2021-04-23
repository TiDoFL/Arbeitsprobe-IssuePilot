using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models
{
    public class ProjectMemberEntry
    {
        [Key]
        [Column(Order = 0)]
        public int FK_ProjectId { get; set; }
        [Key]
        [Column(Order = 1)]
        public string FK_UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int FK_ProjectRoleId { get; set; }

        [ForeignKey("FK_ProjectId")]
        public virtual Project Project { get; set; }
        [ForeignKey("FK_UserId")]
        public virtual User User { get; set; }
        [ForeignKey("FK_ProjectRoleId")]
        public virtual ProjectRole ProjectRole { get; set; }
    }
}
