namespace PortfolioWebsite.Objects
{
    public class AboutMeText
    {
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public AboutMeText(string text, string imageUrl)
        {
            Text = text;
            ImageUrl = imageUrl;
        }
    }
}
