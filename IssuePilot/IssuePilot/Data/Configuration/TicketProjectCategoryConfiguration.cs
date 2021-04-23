using IssuePilot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssuePilot.Data.Configuration
{
    public class TicketProjectCategoryConfiguration : IEntityTypeConfiguration<TicketProjectCategory>
    {
        public void Configure(EntityTypeBuilder<TicketProjectCategory> builder)
        {
            builder.ToTable("TicketProjectCategories");
            builder.HasData(
                new
                {
                    FK_TicketId = 1,
                    FK_TicketCategoryId = 2
                },
                new
                {
                    FK_TicketId = 2,
                    FK_TicketCategoryId = 2
                },
                new
                {
                    FK_TicketId = 2,
                    FK_TicketCategoryId = 1
                },
                new
                {
                    FK_TicketId = 3,
                    FK_TicketCategoryId = 2
                },
                new
                {
                    FK_TicketId = 4,
                    FK_TicketCategoryId = 3
                },
                new
                {
                    FK_TicketId = 5,
                    FK_TicketCategoryId = 4
                });
        }
    }
}