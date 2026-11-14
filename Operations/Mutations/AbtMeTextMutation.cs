using PortfolioWebsite.Auth;
using PortfolioWebsite.Objects;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Operations.Mutations
{
    [ExtendObjectType("Mutation")]
    public class AbtMeTextMutation
    {
        [HotChocolate.Authorization.Authorize(AdminAuth.Policy)]
        public async Task<string> PutTextInJson(AboutMeText abtMeText, [Service("abtMeTextHandler")] AbtMeTextHandler abtMeTextHandler)
        {
            try
            {
                await abtMeTextHandler.WriteAsync(abtMeText);
            }
            catch (Exception)
            {
                return "Could not insert text!";
            }
            return "Succesfully inserted text!";
        }
    }
}
