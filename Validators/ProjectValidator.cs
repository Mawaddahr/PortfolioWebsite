using Dapper;
using FluentValidation;
using PortfolioWebsite.Database;
using PortfolioWebsite.Objects;

namespace PortfolioWebsite.Validators
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        private static MawaddaDbContext _context;
        public ProjectValidator(MawaddaDbContext context)
        {
            _context = context;
            RuleFor(e => e.Id).MustAsync(ExistInDB).WithMessage("Id does not exist.");
        }

        private static async Task<bool> ExistInDB(string Id,
            CancellationToken cancellationToken)
        {
            using var conn = await _context.ConnectionFactory.CreateConnection();

            var parms = new DynamicParameters();
            parms.Add("@Id", Id);

            var sql = @"SELECT id FROM Projects
                        WHERE id = @Id";
            var result = await conn.QueryFirstOrDefaultAsync<string>(sql, parms);

            return result != null;
        }
    }
}
