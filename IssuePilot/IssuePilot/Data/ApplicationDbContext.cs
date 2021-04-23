using IssuePilot.Data.Configuration;
using IssuePilot.Models;
using IssuePilot.Models.DBModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace IssuePilot.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<NewsfeedEntry> NewsfeedEntries { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TicketCategory> TicketCategories { get; set; }
        public DbSet<ProjectMemberEntry> ProjectMemberEntries { get; set; }
        public DbSet<ProjectRole> ProjectRoles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketHistoryEntry> TicketHistoryEntries { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public override DbSet<User> Users { get; set; }
        public DbSet<TicketWorker> TicketWorkers { get; set; }
        public DbSet<TicketProjectCategory> TicketProjectCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.UseIdentityColumns();
            builder.ApplyConfiguration(new AspNetRoleConfiguration());
            builder.ApplyConfiguration(new ProjectRoleConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UsersWithRolesConfig());
            builder.ApplyConfiguration(new ProjectConfiguration());
            builder.ApplyConfiguration(new ProjectMemberEntriesConfiguration());
            builder.ApplyConfiguration(new TicketStatusConfiguration());
            builder.ApplyConfiguration(new TicketConfiguration());
            builder.ApplyConfiguration(new TicketCategoriesConfiguration());
            builder.ApplyConfiguration(new TicketProjectCategoryConfiguration());

            // *** Association tables ***
            // ProjectMemberEntrace
            builder.Entity<Project>().HasMany(c => c.ProjectMemberEntries).WithOne(e => e.Project);
            builder.Entity<User>().HasMany(c => c.ProjectMemberEntries).WithOne(e => e.User);
            builder.Entity<ProjectRole>().HasMany(c => c.ProjectMemberEntries).WithOne(e => e.ProjectRole);
            builder.Entity<ProjectMemberEntry>().HasKey(c => new { c.FK_ProjectId, c.FK_ProjectRoleId, c.FK_UserId });

            // TicketProjectCategory
            builder.Entity<Ticket>().HasMany(c => c.TicketProjectCategories).WithOne(e => e.Ticket);
            builder.Entity<TicketCategory>().HasMany(c => c.TicketProjectCategories).WithOne(e => e.TicketCategory);
            builder.Entity<TicketProjectCategory>().HasKey(c => new { c.FK_TicketId, c.FK_TicketCategoryId });

            // TicketWorker
            builder.Entity<Ticket>().HasMany(c => c.TicketWorkers).WithOne(e => e.Ticket);
            builder.Entity<User>().HasMany(c => c.TicketWorkers).WithOne(e => e.User);
            builder.Entity<TicketWorker>().HasKey(c => new { c.FK_TicketId, c.FK_UserId });

            // *** Others ***
            builder.Entity<Project>().HasOne(c => c.Creator).WithMany(c => c.Projects);

            builder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
            });

            builder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.Title).IsUnique();
            });

            // TicketHistory
            builder.Entity<TicketHistoryEntry>().Property(e => e.EntryCaseId).HasConversion<int>();
            builder.Entity<EntryCase>().Property(e => e.EntryCaseId).HasConversion<int>();
            builder.Entity<EntryCase>().HasData(
                    Enum.GetValues(typeof(EntryCaseId))
                        .Cast<EntryCaseId>()
                        .Select(e => new EntryCase()
                        {
                            EntryCaseId = e,
                            EntryCaseName = e.ToString()
                        })
                );
        }
    }
}
