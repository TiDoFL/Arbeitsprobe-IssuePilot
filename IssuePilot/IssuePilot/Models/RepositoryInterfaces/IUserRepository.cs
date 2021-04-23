using IssuePilot.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string Id);
        Task<(User, IdentityResult)> AddUserAsync(User newUser, string password);
        Task<List<User>> GetAllUsersAsync();
        Task<User> UpdateUserAsync(UserEditViewModel editUserModel);
        Task DeleteUserAsync(string userId);
    }
}
