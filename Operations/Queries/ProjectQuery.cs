using Microsoft.AspNetCore.Mvc.Routing;
using PortfolioWebsite.Objects;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Operations.Queries
{
    [ExtendObjectType("Query")]
    public class ProjectQuery
    {
        //get all, get

        [UsePaging(MaxPageSize = 5)]
        public async Task<List<Project>> GetProjectsAsync([Service("projectService")] ProjectService projectService)
        {
            return await projectService.GetAllProjectsAsync();
        }

        public async Task<Project> GetProjectByIdAsync(string Id, [Service("projectService")] ProjectService projectService)
        {
            return await projectService.GetProjectByIdAsync(Id);
        }
    }
}
