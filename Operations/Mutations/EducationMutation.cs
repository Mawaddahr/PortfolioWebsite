using PortfolioWebsite.Objects;
using PortfolioWebsite.Services;
using PortfolioWebsite.Objects.InputObjects;
using FluentValidation;


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
            await educationValidator.ValidateAndThrowAsync(inputEducation);
            try { await educationService.InsertEducationAsync(inputEducation); }
            catch (Exception e) { throw new Exception($"{e}"); }
            return "Education succesfully inserted!";
        }

        public async Task<string> UpdateEducationAsync(Education education,
            [Service("educationService")] EducationService educationService,
            [Service("educationValidator")] IValidator<Education> educationValidator)
        {
            await educationValidator.ValidateAndThrowAsync(education);
            try { await educationService.UpdateEducationAsync(education); }
            catch (Exception e) { throw new Exception($"{e}"); }
            return $"Education with id: {education.Id} succesfully updated!";
        }

        public async Task<string> DeleteEducationAsync(string Id,
            [Service("educationService")] EducationService educationService)
        {
            string table = "Education";
            var idValidator = new InlineValidator<string>();
            idValidator.RuleFor(x => x)
                .NotEmpty()
                .WithMessage("Id must not be empty.");

            await idValidator.ValidateAndThrowAsync(Id);

            try { await educationService.DeleteEducationAsync(Id); }
            catch (Exception e) { throw new Exception($"{e}"); }
            return $"Education with id: {Id} succesfully deleted!";
        }
    }
}
