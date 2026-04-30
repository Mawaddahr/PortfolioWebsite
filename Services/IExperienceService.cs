using PortfolioWebsite.Objects;
using PortfolioWebsite.Database;

namespace PortfolioWebsite.Services
{
    public interface IExperienceService
    {
        Task<Experience> GetExperienceById(string Id);
    }
}
