using FluentValidation;
using PortfolioWebsite.Validators;
using PortfolioWebsite.Objects;
using PortfolioWebsite.Objects.InputObjects;
using PortfolioWebsite.Services;

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
            try
            {
                await experienceValidator.ValidateAndThrowAsync(experience);
                await experienceService.InsertExperienceAsync(experience);
                return "Succesfully inserted experience.";
            }
            catch (ValidationException e)
            {
                throw new GraphQLException(
                    ErrorBuilder.New()
                    .SetMessage(e.Message)
                    .SetCode("VALIDATION ERROR")
                    .Build());
            }
            catch (Exception e)
            {
                throw new GraphQLException(
                    ErrorBuilder
                    .New()
                    .SetMessage("Failed to insert experience.")
                    .SetExtension("detail", e.Message)
                    .Build());
            }
        }

        public async Task<string> UpdateExperienceAsync(Experience experience,
            [Service("experienceService")] ExperienceService experienceService,
            [Service("experienceValidator")] IValidator<Experience> experienceValidator)
        {
            try
            {
                await experienceValidator.ValidateAndThrowAsync(experience);
                await experienceService.UpdateExperienceAsync(experience);
                return $"experience with id: {experience.Id} updated successfully!";
        }
            catch (ValidationException e)
            {
                throw new GraphQLException(
                    ErrorBuilder.New()
                    .SetMessage(e.Message)
                    .SetCode("VALIDATION ERROR")
                    .Build());
            }
            catch (Exception e)
            {
                throw new GraphQLException(
                    ErrorBuilder
                    .New()
                    .SetMessage("Failed to update experience.")
                    .SetExtension("detail", e.Message)
                    .Build());
            }
        }

        public async Task<string> DeleteExperienceAsync(string id,
            [Service("experienceService")] ExperienceService experienceService,
            [Service("idValidator")] IValidator<(string id, string table)> idValidator)
        {
            try
            {
                string table = "Experiences";
                await idValidator.ValidateAndThrowAsync((id, table));
                await experienceService.DeleteExperienceByIdAsync(id);
                return "succesfully deleted experience!";
            }
            catch (ValidationException e)
            {
                throw new GraphQLException(
                    ErrorBuilder.New()
                    .SetMessage(e.Message)
                    .SetCode("VALIDATION ERROR")
                    .Build());
            }
            catch (Exception e)
            {
                throw new GraphQLException(
                    ErrorBuilder
                    .New()
                    .SetMessage("Failed to delete experience.")
                    .SetExtension("detail", e.Message)
                    .Build());
            }
        }
    }
}
