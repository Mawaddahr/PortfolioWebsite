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
            try
            {
                await inputProjectValidator.ValidateAndThrowAsync(inputProject);
                await projectService.InsertProjectAsync(inputProject);
                return "Succesfully inserted project!";
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
                    .SetMessage("Failed to insert project.")
                    .SetExtension("detail", e.Message)
                    .Build());
            }
        }

        public async Task<string> UpdateProjectAsync(Project project,
            [Service("projectService")] ProjectService projectService,
            [Service("projectValidator")] IValidator<Project> projectValidator)
        {
            try
            {
                await projectValidator.ValidateAndThrowAsync(project);
                await projectService.UpdateProjectAsync(project);
                return "Succesfully updated project!";
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
                    .SetMessage("Failed to update project.")
                    .SetExtension("detail", e.Message)
                    .Build());
            }
        }

        public async Task<string> DeleteProjectAsync(string Id,
            [Service("projectService")] ProjectService projectService,
            [Service("idValidator")] IValidator<(string id, string table)> idValidator)
        {
            try
            {
                string table = "Projects";
                await idValidator.ValidateAndThrowAsync((Id, table));
                await projectService.DeleteProjectAsync(Id);
                return "Succesfully deleted project!";
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
                    .SetMessage("Failed to delete project.")
                    .SetExtension("detail", e.Message)
                    .Build());
            }
        }
    }
}
