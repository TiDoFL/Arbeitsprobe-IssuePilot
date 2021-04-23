using IssuePilot.Helper;
using IssuePilot.Models.RepositoryInterfaces;
using IssuePilot.Models.ViewModels.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IssuePilot.Controllers
{
    [Authorize(Roles = "Admin,Projektmanager,Benutzer")]
    public class StatisticsController : ProjectAccessController
    {
        private readonly IProjectRoleRepository _projectRoleRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsController(IProjectRepository projectRepository, IStatisticsRepository statisticsRepository, IProjectRoleRepository projectRoleRepository) : base(projectRoleRepository, projectRepository)
        {
            _projectRepository = projectRepository;
            _statisticsRepository = statisticsRepository;
            _projectRoleRepository = projectRoleRepository;
        }

        // GET: StatisticsController
        public async Task<ActionResult> Index()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (User.IsInRole("Admin"))
            {
                return View(new StatisticsIndexViewModel() { Projects = await _projectRepository.GetAllProjectsAsync() });
            }
            else
            {
                return View(new StatisticsIndexViewModel() { Projects = await _projectRoleRepository.GetProjectsOfUserByIdAsync(currentUserId) });
            }

        }

        // GET: StatisticsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await _statisticsRepository.GetProjectStatisticDataAsync(id));
        }

        // POST: StatisticsController/Comparison
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comparison([FromForm] StatisticsIndexViewModel model)
        {
            ComparisonViewModel viewModel = new ComparisonViewModel
            {
                StatisticsModelFirst = await _statisticsRepository.GetProjectStatisticDataAsync(model.ComparisonProjectIdFirst),
                StatisticsModelSecond = await _statisticsRepository.GetProjectStatisticDataAsync(model.ComparisonPojectIdSecond)
            };
            return View(viewModel);
        }
    }
}
