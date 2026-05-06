using PortfolioWebsite.Objects.InputObjects;
using FluentValidation;
using Dapper;
using PortfolioWebsite.Database;
using PortfolioWebsite.Objects;

namespace PortfolioWebsite.Validators
{
    public class InputEducationValidator : AbstractValidator<InputEducation>
    {
        private static MawaddaDbContext _context;

        public InputEducationValidator(MawaddaDbContext context)
        {
            _context = context;

            RuleFor(e => e.EndDate)
                .Must(LaterThanStartDate).WithMessage("Enddate must be later than startDate");
            RuleFor(e => e).MustAsync(DoesntExistInDb).WithMessage("Education already exists in the database");
            RuleFor(e => e.OnGoing).Must(TrueIfLaterThanDT).WithMessage("Education is still on going.");
        }
        private static bool LaterThanStartDate(InputEducation education, DateOnly endDate){
            return endDate.CompareTo(education.StartDate) > 0;
        }

        private static async Task<bool> DoesntExistInDb(InputEducation education, CancellationToken cancellationToken)
        {
            var parms = new DynamicParameters();

            parms.Add("@Institution", education.Institution);
            parms.Add("@StudyProgram", education.StudyProgram);
            parms.Add("@StudyProgramType", education.StudyProgramType);
            parms.Add("@StartDate", education.StartDate, System.Data.DbType.Date);
            parms.Add("@OnGoing", education.OnGoing, System.Data.DbType.Boolean);
            parms.Add("@EndDate", education.EndDate, System.Data.DbType.Date);

            var sql = @"SELECT id FROM Education
                        WHERE institution = @Institution
                        and studyprogram = @StudyProgram
                        and studyprogramtype = @StudyProgramType
                        and startdate = @StartDate
                        and ongoing = @OnGoing
                        and enddate = @EndDate";

            var conn = await _context.ConnectionFactory.CreateConnection();
            var query = await conn.QueryAsync<Education?>(sql, parms);
            var result = query.FirstOrDefault();
            
            return result == default;
        }

        private static bool TrueIfLaterThanDT(InputEducation education, bool onGoing)
        {
            if (education.EndDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) > 0)
            {
                return onGoing;
            }
            return !onGoing;
        }
    }
}
