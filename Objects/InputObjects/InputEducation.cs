using System.ComponentModel.DataAnnotations;

namespace PortfolioWebsite.Objects.InputObjects
{
    public class InputEducation
    {
        [Required(ErrorMessage = "Institution must be specified.")]
        [MaxLength(100, ErrorMessage = "Total characters must be less than 100")]
        public string Institution { get; set; }
        [Required(ErrorMessage = "Study program must be specified.")]
        [MaxLength(100, ErrorMessage = "Total characters must be less than 100")]
        public string StudyProgram { get; set; }
        [MaxLength(100, ErrorMessage = "Total characters must be less than 100")]
        public string? StudyProgramType { get; set; }
        [Required(ErrorMessage = "Start date must be specified.")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "Specify whether the study program is still ongoing.")]
        public bool OnGoing { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
