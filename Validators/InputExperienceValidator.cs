using FluentValidation;
using Dapper;
using PortfolioWebsite.Database;
using PortfolioWebsite.Objects;
using PortfolioWebsite.Objects.InputObjects;

namespace PortfolioWebsite.Validators
{
    public class InputExperienceValidator : AbstractValidator<InputExperience>
    {
        private static MawaddaDbContext _context;
        public InputExperienceValidator(MawaddaDbContext context)
        {
            _context = context;

            RuleFor(e => e.EndDate)
                .Must(LaterThanStartDate)
                .WithMessage("Enddate must be later than startDate");
            RuleFor(e => e).MustAsync(DoesntExistInDB)
                .WithMessage("Experience already exists.");
        }
        private static bool LaterThanStartDate(
            InputExperience experience,
            DateOnly endDate)
            => endDate.CompareTo(experience.StartDate) > 0;

        private static async Task<bool> DoesntExistInDB(InputExperience experience, CancellationToken token)
        {
            var conn = await _context.ConnectionFactory.CreateConnection();
            var parms = new DynamicParameters();

            parms.Add("@Company", experience.Company);
            parms.Add("@Description", experience.Description);
            parms.Add("@Role", experience.Role);
            parms.Add("@Location", experience.Location);
            parms.Add("@StartDate", experience.StartDate, System.Data.DbType.Date);
            parms.Add("@EndDate", experience.EndDate, System.Data.DbType.Date);

            var sql = @"SELECT id FROM Experiences
                        WHERE company = @Company
                        and description = @Description
                        and role = @Role
                        and location = @Location
                        and startdate = @StartDate
                        and enddate = @EndDate";

            var result = await conn.QueryAsync<Experience>(sql, parms);

            return result.FirstOrDefault() == default;
        }

    }
}
