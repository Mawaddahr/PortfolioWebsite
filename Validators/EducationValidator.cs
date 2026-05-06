using Dapper;
using FluentValidation;
using PortfolioWebsite.Database;
using PortfolioWebsite.Objects;


namespace PortfolioWebsite.Validators
{
    public class EducationValidator : AbstractValidator<Education>
    {
        private static MawaddaDbContext _context;
        public EducationValidator(MawaddaDbContext context)
        {
            _context = context;
            RuleFor(e => e.Id).MustAsync(IdMustExist)
                .WithMessage("Id does not exist.");
            RuleFor(e => e.EndDate).Must(LaterThanStartDate)
                .WithMessage("EndDate must be later than StartDate.");
            RuleFor(e => e.OnGoing).Must(TrueIfLaterThanDT)
                .WithMessage("Education is still ongoing.");
        }

        private static async Task<bool> IdMustExist(string Id,
            CancellationToken cancellationToken)
        {
            using var conn = await _context.ConnectionFactory.CreateConnection();

            var parms = new DynamicParameters();
            parms.Add("@Id", Id);

            var sql = @"SELECT id FROM Education
                        WHERE id = @Id";
            var result = await conn.QueryFirstOrDefaultAsync<string>(sql, parms);

            return result != null;
        }
        private static bool LaterThanStartDate(Education education,
            DateOnly endDate)
            => endDate.CompareTo(education.StartDate) > 0;

        private static bool TrueIfLaterThanDT(Education education, bool onGoing)
        {
            if (education
                .EndDate
                .CompareTo(DateOnly.FromDateTime(DateTime.Now)) > 0)
            {
                return onGoing;
            }
            return !onGoing;
        }
    }
}