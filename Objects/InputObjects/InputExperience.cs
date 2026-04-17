namespace PortfolioWebsite.Objects.InputObjects
{
    public class InputExperience
    {
        public string Company { get; set; }
        public string Role { get; set; }

        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Location { get; set; }
        public string Duration { get; set; }
    }
}
