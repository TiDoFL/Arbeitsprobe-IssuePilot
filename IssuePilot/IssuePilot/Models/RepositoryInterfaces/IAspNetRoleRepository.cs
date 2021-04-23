using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface IAspNetRoleRepository
    {
        Task<IdentityResult> AddUserToIdentityRoleAsync(User user, string role);
        Task<IdentityResult> RemoveUserToIdentityRoleAsync(User user, string role);
        Task<string> GetRoleOfUserByIdAsync(string databaseUserId);
    }
}
