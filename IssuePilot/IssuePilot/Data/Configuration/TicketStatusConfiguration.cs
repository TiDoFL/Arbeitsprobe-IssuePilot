using IssuePilot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssuePilot.Data.Configuration
{
    public class TicketStatusConfiguration : IEntityTypeConfiguration<TicketStatus>
    {
        public void Configure(EntityTypeBuilder<TicketStatus> builder)
        {
            builder.ToTable("TicketStatuses");
            builder.HasData(
                new
                {
                    Id = 1,
                    Name = "Offen"
                },
                new
                {
                    Id = 2,
                    Name = "Abgeschlossen"
                },
                new
                {
                    Id = 3,
                    Name = "Abgebrochen"
                },
                new
                {
                    Id = 4,
                    Name = "Pausiert"
                },
                new
                {
                    Id = 5,
                    Name = "In Bearbeitung"
                });
        }
    }
}
