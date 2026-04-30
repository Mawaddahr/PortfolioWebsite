using PortfolioWebsite.Objects;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Operations.Queries
{
    [ExtendObjectType("Query")]
    public class ExperienceQuery
    {
        [UseResolverScope]
        public async Task<Experience> GetExperienceByIdAsync(string id, [Service("experienceService")] ExperienceService experienceService)
        {
            Experience experience = await experienceService.GetExperienceByIdAsync(id);
            return experience;
        }

        [UsePaging(MaxPageSize = 5)]
        public async Task<List<Experience>> GetExperiencesAsync([Service("experienceService")] ExperienceService experienceService)
        {
            List<Experience> experiences = await experienceService.GetAllExperienceAsync();
            return experiences;
        }
    }
}
