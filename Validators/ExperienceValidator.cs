using Dapper;
using FluentValidation;
using PortfolioWebsite.Database;
using PortfolioWebsite.Objects;

namespace PortfolioWebsite.Validators
{
    public class ExperienceValidator : AbstractValidator<Experience>
    {
        private static MawaddaDbContext _context;
        public ExperienceValidator(MawaddaDbContext context)
        {
            _context = context;
            RuleFor(e => e.Id).MustAsync(ExistInDB)
                .WithMessage("Id does not exist.");
            RuleFor(e => e.EndDate)
                .Must(LaterThanStartDate)
                .WithMessage("Enddate must be later than startDate");
        }

        private static async Task<bool> ExistInDB(string Id,
            CancellationToken cancellationToken)
        {
            using var conn = await _context.ConnectionFactory.CreateConnection();

            var parms = new DynamicParameters();
            parms.Add("@Id", Id);

            var sql = @"SELECT id FROM Experiences
                        WHERE id = @Id";
            var result = await conn.QueryFirstOrDefaultAsync<string>(sql, parms);

            return result != null;
        }
        private static bool LaterThanStartDate(Experience experience,
            DateOnly? endDate)
        {
            if (endDate.HasValue)
                return endDate.Value.CompareTo(experience.StartDate) > 0;
            return true;
        }
    }
}
