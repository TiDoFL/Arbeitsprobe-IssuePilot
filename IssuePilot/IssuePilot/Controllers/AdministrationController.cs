using IssuePilot.Helper;
using IssuePilot.Models;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IssuePilot.Controllers
{
    [Authorize(Roles = "Admin,Projektmanager,Benutzer")]
    public class AdministrationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IAspNetRoleRepository _aspNetRoleRepository;
        public AdministrationController(IUserRepository userRepository, IAspNetRoleRepository aspNetRoleRepository)
        {
            this._userRepository = userRepository;
            this._aspNetRoleRepository = aspNetRoleRepository;
        }

        // GET: Administration
        public async Task<IActionResult> Index(string sortOrder, int? pageNumber, string currentFilter, string searchString)
        {
            ViewData["FirstnameSortParm"] = sortOrder == "Firstname" ? "firstname_desc" : "Firstname";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["SurnameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "surname_desc" : "";

            var users = await _userRepository.GetAllUsersAsync();
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.UserName.Contains(searchString)).ToList();
            }
            ViewData["CurrentSort"] = sortOrder;
            switch (sortOrder)
            {
                case "firstname_desc":
                    users = users.OrderByDescending(s => s.Firstname).ToList();
                    break;
                case "Firstname":
                    users = users.OrderBy(s => s.Firstname).ToList();
                    break;
                case "Date":
                    users = users.OrderBy(s => s.CreateDate).ToList();
                    break;
                case "date_desc":
                    users = users.OrderByDescending(s => s.CreateDate).ToList();
                    break;
                case "surname_desc":
                    users = users.OrderByDescending(s => s.Surname).ToList();
                    break;
                default:
                    users = users.OrderBy(s => s.Surname).ToList();
                    break;
            }
            int pageSize = 20;
            return View(await PaginatedList<User>.CreateFromListAsync(users, pageNumber ?? 1, pageSize));

        }

        // GET: Administration/Create
        [Authorize(Roles = "Admin")]
        public IActionResult CreateAsync()
        {
            return View();
        }

        // POST: Administration/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserRegisterInputModel inputModel)
        {
            User user = new User { Email = inputModel.Email, Firstname = inputModel.Firstname, Surname = inputModel.Surname };
            if (ModelState.IsValid)
            {
                (User, IdentityResult) userResult = await _userRepository.AddUserAsync(user, inputModel.Password);
                if (userResult.Item2.Succeeded)
                {
                    IdentityResult roleResult = await _aspNetRoleRepository.AddUserToIdentityRoleAsync(userResult.Item1, inputModel.Role);
                    return RedirectToAction(nameof(Index));
                }
                // Password Validation RequireDigit, RequiredLength=6, RequireUppercase, RequireLowercase
                foreach (IdentityError error in userResult.Item2.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(inputModel);
        }


        // GET: Administration/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);
            string role = await _aspNetRoleRepository.GetRoleOfUserByIdAsync(id);
            UserEditViewModel viewData = new UserEditViewModel { UserName = user.UserName, CurrentRole = role, Id = user.Id, Firstname = user.Firstname, Surname = user.Surname, Email = user.Email };
            return View(viewData);
        }

        // POST: Administration/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [FromForm] UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                // update user data
                User updatedUser = await _userRepository.UpdateUserAsync(model);

                // update role
                if (model.Role != model.CurrentRole && model.Role != "keine Änderung")
                {
                    Microsoft.AspNetCore.Identity.IdentityResult result = await _aspNetRoleRepository.RemoveUserToIdentityRoleAsync(updatedUser, model.CurrentRole);
                    await _aspNetRoleRepository.AddUserToIdentityRoleAsync(updatedUser, model.Role);
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Administration/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);
            string role = await _aspNetRoleRepository.GetRoleOfUserByIdAsync(id);
            UserDeleteViewModel viewData = new UserDeleteViewModel { Role = role, Id = user.Id, Firstname = user.Firstname, Surname = user.Surname, Email = user.Email, UserName = user.UserName };
            return View(viewData);
        }

        // POST: Administration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, string role)
        {
            User user = await _userRepository.GetUserByIdAsync(id);
            Microsoft.AspNetCore.Identity.IdentityResult result = await _aspNetRoleRepository.RemoveUserToIdentityRoleAsync(user, role);
            await _userRepository.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
