using PortfolioWebsite.Objects;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Operations.Queries
{
    [ExtendObjectType("Query")]
    public class AbtMeTextQuery
    {
        public AboutMeText ReadAbtMeText([Service("abtMeTextHandler")] AbtMeTextHandler abtMeTextHandler)
        {
            try
            {
                string path = @"/app/Database/AbtMeText.json";
                AboutMeText abtMeText = abtMeTextHandler.ReadFromJson(path);
                return abtMeText;
            }
            catch(Exception e)
            {
                throw new Exception($"{e}");
            }
        }
    }
}
