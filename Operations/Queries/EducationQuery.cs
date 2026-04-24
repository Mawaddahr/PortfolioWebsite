using PortfolioWebsite.Objects;
using PortfolioWebsite.Services;


namespace PortfolioWebsite.Operations.Queries
{
    [ExtendObjectType("Query")]
    public class EducationQuery
    {

        //getall, get

        public async Task<List<Education>?> GetAllEducationAsync([Service("educationService")] EducationService educationService)
        {
            List<Education> AllEducation = await educationService.GetAllEducationAsync();
            //if AllEducation
            return AllEducation;
        }

        public async Task<Education> GetEducationById(string Id, [Service("educationService")] EducationService educationService)
        {
            Education education = await educationService.GetEducationByIdAsync(Id);
            return education;
        }
    }
}
