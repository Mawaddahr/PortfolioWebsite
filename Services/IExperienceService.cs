using PortfolioWebsite.Database;
using PortfolioWebsite.Objects;

namespace PortfolioWebsite.Services
{
    public interface IExperienceService
    {
        Task<Experience> GetExperienceById(string Id);
    }
}
