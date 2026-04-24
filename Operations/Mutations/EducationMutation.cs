using PortfolioWebsite.Objects;
using PortfolioWebsite.Services;
using PortfolioWebsite.Objects.InputObjects;


namespace PortfolioWebsite.Operations.Mutations
{
    [ExtendObjectType("Mutation")]
    public class EducationMutation
    {
        // insert, update, delete

        public async Task<string> InsertEducationAsync(InputEducation inputEducation, [Service("educationService")] EducationService educationService)
        {
            try { await educationService.InsertEducationAsync(inputEducation); }
            catch (Exception e) { throw new Exception($"{e}"); }
            return "Education succesfully inserted!";
        }

        public async Task<string> UpdateEducationAsync(Education education, [Service("educationService")] EducationService educationService)
        {
            try { await educationService.UpdateEducationAsync(education); }
            catch (Exception e) { throw new Exception($"{e}"); }
            return $"Education with id: {education.Id} succesfully updated!";
        }

        public async Task<string> DeleteEducationAsync(string Id, [Service("educationService")] EducationService educationService)
        {
            try { await educationService.DeleteEducationAsync(Id); }
            catch (Exception e) { throw new Exception($"{e}"); }
            return $"Education with id: {Id} succesfully deleted!";
        }
    }
}
