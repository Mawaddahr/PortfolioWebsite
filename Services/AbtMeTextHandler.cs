using PortfolioWebsite.Objects;
using Newtonsoft.Json;
namespace PortfolioWebsite.Services
{
    public class AbtMeTextHandler
    {
        public AboutMeText ReadFromJson(string path)
        {
            using StreamReader r = new StreamReader(path);
            string json = r.ReadToEnd();
            AboutMeText AbtMeText = JsonConvert.DeserializeObject<AboutMeText>(json);
            return AbtMeText;
        }

        public bool WriteToJson(AboutMeText abtMeText, string path)
        {
            try
            {
                File.WriteAllTextAsync(path, string.Empty);
                var SerializedText =  JsonConvert.SerializeObject(abtMeText);
                File.WriteAllTextAsync(path, SerializedText);

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
