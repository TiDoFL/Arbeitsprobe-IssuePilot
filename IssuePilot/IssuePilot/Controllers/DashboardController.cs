using IssuePilot.Helper;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IssuePilot.Controllers
{
    [Authorize(Roles = "Admin,Projektmanager,Benutzer")]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var listOfNewsfeed = await _dashboardRepository.GetNewsfeedByUserIdAsync(currentUserId);

            for (int i = 0; i < listOfNewsfeed.Count; i++)
            {
                await _dashboardRepository.UpdateSeenStatusAsync(listOfNewsfeed[i].Id);
            }
            int pageSize = 10;
            return View(await PaginatedList<NewsfeedListViewModel>.CreateFromListAsync(listOfNewsfeed, pageNumber ?? 1, pageSize));
        }
    }
}
