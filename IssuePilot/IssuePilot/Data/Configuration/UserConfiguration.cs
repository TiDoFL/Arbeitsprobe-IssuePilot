using IssuePilot.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace IssuePilot.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private const string adminId = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7";
        private const string demoAdminId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5";
        private const string managerId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1";
        private const string userId = "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335";

        public void Configure(EntityTypeBuilder<User> builder)
        {
            User admin = new User
            {
                Id = adminId,
                Firstname = "admin",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "Admin@Admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = new Guid().ToString("D"),
            };

            admin.PasswordHash = PasswordGenerate(admin);
            builder.HasData(admin);

            User manager = new User
            {
                Id = managerId,
                Firstname = "manager",
                UserName = "Manager",
                NormalizedUserName = "MANAGER",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = new Guid().ToString("D"),
            };
            manager.PasswordHash = PasswordGenerate(manager);
            builder.HasData(manager);

            User user = new User
            {
                Id = userId,
                Firstname = "benutzer",
                UserName = "Benutzer",
                NormalizedUserName = "BENUTZER",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = new Guid().ToString("D"),
            };
            user.PasswordHash = PasswordGenerate(user);
            builder.HasData(user);

            User demoAdmin = new User
            {
                Id = demoAdminId,
                Firstname = "demo",
                Surname = "admin",
                UserName = "DemoAdmin",
                NormalizedUserName = "DEMOADMIN",
                Email = "demoadmin@Admin.com",
                NormalizedEmail = "DEMOADMIN@ADMIN.COM",
                SecurityStamp = new Guid().ToString("D"),
            };

            demoAdmin.PasswordHash = PasswordGenerate(demoAdmin);
            builder.HasData(demoAdmin);
        }

        public string PasswordGenerate(User user)
        {
            PasswordHasher<User> passHash = new PasswordHasher<User>();
            return passHash.HashPassword(user, "password");
        }
    }
}
