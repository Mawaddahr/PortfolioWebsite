using PortfolioWebsite.Database;
using PortfolioWebsite.Objects;
using Dapper;
using PortfolioWebsite.Services;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioWebsite.Operations
{
    public class Query
    {
        [UseResolverScope]
        public async Task<Experience> GetExperienceByIdAsync(string id, [Service("experienceService")] ExperienceService experienceService)
        {
            Experience experience = await experienceService.GetExperienceById(id);
            return experience;
        }

        public async Task<List<Experience>> GetExperiencesAsync([Service("experienceService")] ExperienceService experienceService)
        {
            List<Experience> experiences = await experienceService.GetExperiencesAsync();
            return experiences;
        }
    }
}
