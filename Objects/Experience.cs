using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioWebsite.Objects
{
    [Table("Experiences")]
    public class Experience
    {
        public Experience(
            string company,
            string role,
            string description,
            DateOnly startDate,
            DateOnly endDate,
            string location
            )
        {
            Id = new Guid().ToString();
            Company = company;
            Role = role;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Location = location;
        }
        public string Id { get; set; }
        public string Company { get; set; }
        public string Role { get; set; }

        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Location { get; set; }
        public string Duration { get; set; } = string.Empty;
    }
}
