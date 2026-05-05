using System.ComponentModel.DataAnnotations;

namespace PortfolioWebsite.Objects.InputObjects
{
    public class InputExperience
    {
        [MaxLength(100, ErrorMessage = "Total characters must be less than 100.")]
        [Required(ErrorMessage = "Company must be specified.")]
        public string Company { get; set; }
        [Required(ErrorMessage = ("Role must be specified."))]
        public string Role { get; set; }
        [MaxLength(500, ErrorMessage = "Total characters must be less than 500.")]

        public string Description { get; set; }
        [Required(ErrorMessage = "Startdate must be specified.")]
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; } = default;
        [MaxLength(100)]
        public string Location { get; set; }
    }
}
