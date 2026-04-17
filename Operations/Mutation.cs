using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Objects;
using PortfolioWebsite.Services;
using PortfolioWebsite.Objects.InputObjects;

namespace PortfolioWebsite.Operations
{
    public class Mutation
    {
        [UseResolverScope]
        public async Task<string> InsertExperienceAsync(InputExperience experience, [Service("experienceService")] ExperienceService experienceService)
        {
            try
            {
                await experienceService.AddExperience(experience);
            }
            catch (Exception e)
                {
                throw new Exception($"You dumb fuck this is your error: {e}");
            }
            return "great job!";
        }

        public async Task<string> UpdateExperienceAsync(Experience experience, [Service("experienceService")] ExperienceService experienceService)
        {
            try
            {
                await experienceService.UpdateExperience(experience);
            }
            catch (Exception e)
            {
                throw new Exception($"{e}");
            }
            return "succesfully updated experience!";
        }

        public async Task<string> DeleteExperienceAsync(string id, [Service("experienceService")] ExperienceService experienceService)
        {
            try
            {
                await experienceService.DeleteExperienceById(id);
            }
            catch (Exception e)
            {
                throw new Exception($"{e}");
            }
            return "succesfully deleted experience!";
        }
    }
}
