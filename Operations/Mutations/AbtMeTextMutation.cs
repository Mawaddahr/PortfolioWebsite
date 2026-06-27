using PortfolioWebsite.Objects;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Operations.Mutations
{
    [ExtendObjectType("Mutation")]
    public class AbtMeTextMutation
    {
        public string PutTextInJson(AboutMeText abtMeText, [Service("abtMeTextHandler")] AbtMeTextHandler abtMeTextHandler)
        {
            try
            {
                string path = @"/app/Database/AbtMeText.json";
                abtMeTextHandler.WriteToJson(abtMeText, path);
            }
            catch (Exception)
            {
                return "Could not insert text!";
            }
            return "Succesfully inserted text!";
        }
    }
}
