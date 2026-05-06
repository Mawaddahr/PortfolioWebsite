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
            [Service("inputExperienceValidator")] IValidator<InputExperience> experienceValidator)
        {
            await experienceValidator.ValidateAndThrowAsync(experience);
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

        public async Task<string> UpdateExperienceAsync(Experience experience,
            [Service("experienceService")] ExperienceService experienceService,
            [Service("experienceValidator")] IValidator<Experience> experienceValidator)
        {
            await experienceValidator.ValidateAndThrowAsync(experience);
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

        public async Task<string> DeleteExperienceAsync(string id,
            [Service("experienceService")] ExperienceService experienceService)
        {
            var idValidator = new InlineValidator<string>();
            idValidator.RuleFor(x => x)
                .NotEmpty().WithMessage("Id must not be empty.");

            await idValidator.ValidateAndThrowAsync(id);
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
