using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace PortfolioWebsite.Objects
{
    [Table("Education")]
    public class Education
    {
        public Education() { }
        public Education (
            string institution,
            string studyProgram,
            string studyProgramType,
            DateOnly startDate,
            bool onGoing,
            DateOnly endDate)
        {
            Id = Guid.NewGuid().ToString();
            Institution = institution;
            StudyProgram = studyProgram;
            StudyProgramType = studyProgramType;
            StartDate = startDate;
            OnGoing = onGoing;
            EndDate = endDate;
        }
        
        [System.ComponentModel.DataAnnotations.Key]
        [Required(ErrorMessage = "Id is missing.")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Institution must be specified.")]
        [MaxLength(100, ErrorMessage = "Total characters must be less than 100")]

        public string Institution { get; set; }
        [Required(ErrorMessage = "Study program must be specified.")]
        [MaxLength(100, ErrorMessage = "Total characters must be less than 100")]

        public string StudyProgram { get; set; }
        [MaxLength(100, ErrorMessage = "Total characters must be less than 100")]

        public string? StudyProgramType { get; set;}
        [Required(ErrorMessage = "Start date must be specified.")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "Specify whether the study program is still ongoing.")]
        public bool OnGoing { get; set; }
        [Required(ErrorMessage = "Specify (estimated) end date.")]
        public DateOnly EndDate { get; set; }

        }

    }
