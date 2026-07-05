using FluentValidation;
using PortfolioWebsite.Objects;
using PortfolioWebsite.Objects.InputObjects;
using PortfolioWebsite.Services;


namespace PortfolioWebsite.Operations.Mutations
{
    [ExtendObjectType("Mutation")]
    public class EducationMutation
    {
        // insert, update, delete

        public async Task<string> InsertEducationAsync(InputEducation inputEducation,
            [Service("educationService")] EducationService educationService,
            [Service("inputEducationValidator")] IValidator<InputEducation> educationValidator)
        {
            try
            {
                await educationValidator.ValidateAndThrowAsync(inputEducation);
                await educationService.InsertEducationAsync(inputEducation);
                return "Education succesfully inserted!";
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
                    .SetMessage("Failed to insert education.")
                    .SetExtension("detail", e.Message)
                    .Build());
            }
        }

        public async Task<string> UpdateEducationAsync(Education education,
            [Service("educationService")] EducationService educationService,
            [Service("educationValidator")] IValidator<Education> educationValidator)
        {
            try
            {
                await educationValidator.ValidateAndThrowAsync(education);
                await educationService.UpdateEducationAsync(education);
                return $"Education with id: {education.Id} succesfully updated!";
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
                    .SetMessage("Failed to update education.")
                    .SetExtension("detail", e.Message)
                    .Build());
            }
        }

        public async Task<string> DeleteEducationAsync(string Id,
            [Service("educationService")] EducationService educationService,
            [Service("idValidator")] IValidator<(string id, string table)> idValidator)
        {
            string table = "Education";
            try
            {
                await idValidator.ValidateAndThrowAsync((Id, table));
                await educationService.DeleteEducationAsync(Id);
                return $"Education with id: {Id} succesfully deleted!";
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
                    .SetMessage("Failed to delete education.")
                    .SetExtension("detail", e.Message)
                    .Build());
            }
        }
    }
}
