using System.ComponentModel.DataAnnotations;
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
            DateOnly? endDate,
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
        [System.ComponentModel.DataAnnotations.Key]
        [Required(ErrorMessage = "Id must be specified.")]
        public string Id { get; set; }
        [MaxLength(100, ErrorMessage = "Total characters must be less than 100.")]
        [Required(ErrorMessage = "Company must be specified.")]
        public string Company { get; set; }
        [Required(ErrorMessage =("Role must be specified."))]
        public string Role { get; set; }
        [MaxLength(500, ErrorMessage = "Total characters must be less than 500.")]

        public string Description { get; set; }
        [Required(ErrorMessage = "Startdate must be specified.")]
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; } = default;
        [MaxLength(100)]
        public string Location { get; set; }
        public string Duration => EndDate == null ? $"{new DateOnly().DayNumber - StartDate.DayNumber}" : $"{EndDate?.DayNumber - StartDate.DayNumber}";
    }
}
