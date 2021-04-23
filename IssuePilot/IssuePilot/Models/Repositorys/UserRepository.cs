using IssuePilot.Data;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssuePilot.Models.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IDashboardRepository _dashboardRepository;
        public UserRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public UserRepository(ApplicationDbContext context, IDashboardRepository dashboardRepository)
        {
            this._context = context;
            this._dashboardRepository = dashboardRepository;
        }

        public UserRepository(ApplicationDbContext context, UserManager<User> userManager, IDashboardRepository dashboardRepository)
        {
            this._context = context;
            this._userManager = userManager;
            this._dashboardRepository = dashboardRepository;
        }

        public string GenerateUserName(string firstname, string surname)
        {
            string generatedUserName = "";
            int abortCounter = 0;
            do
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 3000);
                generatedUserName = firstname + surname + randomNumber;

                // while-termination
                abortCounter++;
                if (abortCounter > 5000)
                {
                    throw new Exception("Couldn't generate UserName after 5000 runs! Please try another first or surname.");
                }
            } while (_context.Users.Any(t => t.UserName == generatedUserName));
            return generatedUserName;
        }

        public async Task<(User, IdentityResult)> AddUserAsync(User newUser, string password)
        {
            newUser.CreateDate = DateTime.Now;
            newUser.UserName = GenerateUserName(newUser.Firstname, newUser.Surname);
            IdentityResult result = await _userManager.CreateAsync(newUser, password);
            User returnUser = await _context.Users.FirstOrDefaultAsync(c => c.UserName == newUser.UserName);
            return (returnUser, result);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            if (userId == null) { throw new ArgumentNullException(); }
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> UpdateUserAsync(UserEditViewModel editUserModel)
        {
            if (editUserModel == null) { throw new ArgumentNullException(); }
            User databaseUser = await _context.Users.FindAsync(editUserModel.Id);
            if (databaseUser == null) { throw new NullReferenceException(); }
            if (editUserModel.Email != databaseUser.Email && editUserModel.Email != null)
            {
                databaseUser.NormalizedEmail = editUserModel.Email.ToUpper();
                databaseUser.Email = editUserModel.Email;
            }

            databaseUser.Firstname = editUserModel.Firstname;
            databaseUser.Surname = editUserModel.Surname;

            if (editUserModel.Password != "" && editUserModel.Password != null)
            {
                PasswordHasher<User> passHash = new PasswordHasher<User>();
                databaseUser.PasswordHash = passHash.HashPassword(databaseUser, editUserModel.Password);
            }

            _context.Users.Update(databaseUser);
            await _context.SaveChangesAsync();
            return databaseUser;
        }

        public async Task DeleteUserAsync(string userId)
        {
            if (userId == null) { throw new ArgumentNullException(); }
            User databaseUser = await _context.Users.FindAsync(userId);
            if (databaseUser == null) { throw new NullReferenceException(); }

            databaseUser.Email = null;
            databaseUser.CreateDate = DateTime.MinValue;
            databaseUser.NormalizedEmail = null;
            databaseUser.Firstname = null;
            databaseUser.Surname = null;
            databaseUser.PasswordHash = null;
            databaseUser.PhoneNumber = null;
            databaseUser.UserName = GenerateUserName("gelöschter", "Nutzer");
            databaseUser.NormalizedUserName = databaseUser.UserName.ToUpper();
            databaseUser.IsDeleted = true;

            _context.Users.Update(databaseUser);
            await _context.SaveChangesAsync();
            await _dashboardRepository.DeleteAllEntriesOfUserAsync(userId);
        }
    }
}
