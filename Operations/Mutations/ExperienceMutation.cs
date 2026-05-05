using PortfolioWebsite.Objects;
using PortfolioWebsite.Services;
using PortfolioWebsite.Objects.InputObjects;
using FluentValidation;

namespace PortfolioWebsite.Operations.Mutations
{
    [ExtendObjectType("Mutation")]
    public class ExperienceMutation
    {
        [UseResolverScope]
        public async Task<string> InsertExperienceAsync(InputExperience experience,
            [Service("experienceService")] ExperienceService experienceService,
            [Service("experienceValidator")] IValidator<InputExperience> experienceValidator)
        {
            var validating = await experienceValidator.ValidateAsync(experience);

            if (!validating.IsValid)
            {
                throw new ValidationException($"Failed to insert experience :{validating.Errors.Select(e => e.ErrorMessage)}");
            }
            try
            {
                await experienceService.InsertExperienceAsync(experience);
            }
            catch (Exception e)
                {
                throw new Exception($"This is your error: {e}");
            }
            return "great job!";
        }

        public async Task<string> UpdateExperienceAsync(Experience experience, [Service("experienceService")] ExperienceService experienceService)
        {
            try
            {
                await experienceService.UpdateExperienceAsync(experience);
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
                await experienceService.DeleteExperienceByIdAsync(id);
            }
            catch (Exception e)
            {
                throw new Exception($"{e}");
            }
            return "succesfully deleted experience!";
        }
    }
}
