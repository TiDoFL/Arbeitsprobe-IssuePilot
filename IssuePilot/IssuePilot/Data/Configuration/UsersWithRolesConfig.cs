using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssuePilot.Data.Configuration
{
    public class UsersWithRolesConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        // user
        private const string adminUserId = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7";
        private const string managerUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1";
        private const string userUserId = "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335";
        private const string demoAdminId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5";

        // role
        private const string adminRoleId = "2301D884-221A-4E7D-B509-0113DCC043E1";
        private const string userRoleId = "78A7570F-3CE5-48BA-9461-80283ED1D94D";
        private const string managerRoleId = "7D9B7113-A8F8-4035-99A7-A20DD400F6A3";

        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            IdentityUserRole<string> admin = new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            };

            builder.HasData(admin);

            IdentityUserRole<string> manager = new IdentityUserRole<string>
            {
                RoleId = managerRoleId,
                UserId = managerUserId
            };

            builder.HasData(manager);

            IdentityUserRole<string> user = new IdentityUserRole<string>
            {
                RoleId = userRoleId,
                UserId = userUserId
            };

            builder.HasData(user);

            IdentityUserRole<string> demoAdmin = new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = demoAdminId
            };

            builder.HasData(demoAdmin);
        }
    }
}
