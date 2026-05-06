using FluentValidation;
using PortfolioWebsite.Objects;
using PortfolioWebsite.Objects.InputObjects;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Operations.Mutations
{
    //insert, update, delete

    [ExtendObjectType("Mutation")]
    public class ProjectMutation
    {
        public async Task<string> InsertProjectAsync(InputProject inputProject,
            [Service("projectService")] ProjectService projectService,
            [Service("inputProjectValidator")] IValidator<InputProject> inputProjectValidator)
        {
            await inputProjectValidator.ValidateAndThrowAsync(inputProject);
            try { await projectService.InsertProjectAsync(inputProject); }
            catch (Exception e) { throw new Exception($"{e}"); }
            return "Succesfully inserted project!";
        }

        public async Task<string> UpdateProjectAsync(Project project,
            [Service("projectService")] ProjectService projectService,
            [Service("projectValidator")] IValidator<Project> projectValidator)
        {
            await projectValidator.ValidateAndThrowAsync(project);
            try { await projectService.UpdateProjectAsync(project); }
            catch (Exception e) { throw new Exception($"{e}"); }
            return "Succesfully updated project!";
        }

        public async Task<string> DeleteProjectAsync(string Id,
            [Service("projectService")] ProjectService projectService)
        {
            var idValidator = new InlineValidator<string>();
            idValidator.RuleFor(x => x)
                .NotEmpty().WithMessage("Id must not be empty.");

            await idValidator.ValidateAndThrowAsync(Id);
            try { await projectService.DeleteProjectAsync(Id); }
            catch (Exception e) { throw new Exception($"{e}"); }
            return "Succesfully deleted project!";
        }
    }
}
