using IssuePilot.Models.ViewModels.Statistics;
using System.Threading.Tasks;

namespace IssuePilot.Models.RepositoryInterfaces
{
    public interface IStatisticsRepository
    {
        Task<StatisticsViewModel> GetProjectStatisticDataAsync(int projectId);
    }
}
