using IssuePilot.Data;
using IssuePilot.Models.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IssuePilot.Models.Repositorys
{
    public class AspNetRoleRepository : IAspNetRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public AspNetRoleRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public AspNetRoleRepository(ApplicationDbContext context, UserManager<User> userManager, IUserRepository userRepository)
        {
            this._context = context;
            this._userManager = userManager;
            this._userRepository = userRepository;
        }

        public async Task<IdentityResult> AddUserToIdentityRoleAsync(User user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveUserToIdentityRoleAsync(User user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<string> GetRoleOfUserByIdAsync(string userId)
        {
            var dbUser = await _userRepository.GetUserByIdAsync(userId);
            if (dbUser == null) { throw new NullReferenceException(); }
            System.Collections.Generic.IList<string> roles = await _userManager.GetRolesAsync(dbUser);
            return roles.Count == 0 ? "" : roles.First();
        }
    }
}
