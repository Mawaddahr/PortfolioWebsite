using PortfolioWebsite.Objects;
using PortfolioWebsite.Objects.InputObjects;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Operations.Mutations
{
    //insert, update, delete

    [ExtendObjectType("Mutation")]
    public class ProjectMutation
    {
        public async Task<string> InsertProjectAsync(InputProject inputProject, [Service("projectService")] ProjectService projectService)
        {
            try { await projectService.InsertProjectAsync(inputProject); }
            catch (Exception e) { throw new Exception($"{e}"); }
            return "Succesfully inserted project!";
        }

        public async Task<string> UpdateProjectAsync(Project project, [Service("projectService")] ProjectService projectService)
        {
            try { await projectService.UpdateProjectAsync(project); }
            catch (Exception e) { throw new Exception($"{e}"); }
            return "Succesfully updated project!";
        }

        public async Task<string> DeleteProjectAsync(string Id, [Service("projectService")] ProjectService projectService)
        {
            try { await projectService.DeleteProjectAsync(Id); }
            catch (Exception e) { throw new Exception($"{e}"); }
            return "Succesfully deleted project!";
        }
    }
}
