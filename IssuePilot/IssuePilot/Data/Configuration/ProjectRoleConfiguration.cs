using IssuePilot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssuePilot.Data.Configuration
{
    public class ProjectRoleConfiguration : IEntityTypeConfiguration<ProjectRole>
    {

        public void Configure(EntityTypeBuilder<ProjectRole> builder)
        {
            builder.ToTable("ProjectRoles");
            builder.HasData(
                new ProjectRole
                {
                    Id = 1,
                    Title = "Eigentümer/in"
                },
                new ProjectRole
                {
                    Id = 2,
                    Title = "Teilnehmer/in"

                });
        }
    }
}
