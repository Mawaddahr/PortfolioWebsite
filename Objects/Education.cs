using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;

namespace PortfolioWebsite.Objects
{
    [Table("Education")]
    public class Education
    {
        public Education (
            string institution,
            string studyProgram,
            string studyProgramType,
            DateOnly startDate,
            bool onGoing,
            DateOnly estimatedEndDate,
            DateOnly endDate)
        {
            Id = new Guid().ToString();
            Institution = institution;
            StudyProgram = studyProgram;
            StudyProgramType = studyProgramType;
            StartDate = startDate;
            OnGoing = onGoing;
            EstimatedEndDate = estimatedEndDate;
            EndDate = endDate;
        }
        private DateOnly _endDate { get; set; }
        [Key]
        public string Id { get; set; }
        public string Institution { get; set; }
        public string StudyProgram { get; set; }
        public string StudyProgramType { get; set;}
        public DateOnly StartDate { get; set; }
        public bool OnGoing { get; set; }
        public DateOnly EstimatedEndDate { get; set; }
        public DateOnly EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = OnGoing == true ? EstimatedEndDate : value;
            }
        }

    }
}
