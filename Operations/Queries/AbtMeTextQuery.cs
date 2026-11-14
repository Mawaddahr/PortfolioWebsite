using PortfolioWebsite.Objects;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Operations.Queries
{
    [ExtendObjectType("Query")]
    public class AbtMeTextQuery
    {
        public async Task<AboutMeText> ReadAbtMeText([Service("abtMeTextHandler")] AbtMeTextHandler abtMeTextHandler)
        {
            return await abtMeTextHandler.ReadAsync();
        }
    }
}
