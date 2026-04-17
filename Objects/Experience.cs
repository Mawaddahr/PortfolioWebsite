using Dapper.Contrib;
using Dapper.Contrib.Extensions;

namespace PortfolioWebsite.Objects
{
    [Table("Experiences")]
    public class Experience
    {
        public Experience() { Id = new Guid().ToString(); }
        public Experience(
            string company,
            string role,
            string description,
            DateOnly startDate,
            DateOnly endDate,
            string location
            )
        {
            Id = System.Guid.NewGuid().ToString();
            Company = company;
            Role = role;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Location = location;
        }
        [Key]
        public string Id { get; set; }
        public string Company { get; set; }
        public string Role { get; set; }

        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Location { get; set; }
        public string Duration { get; set; }
    }
}
