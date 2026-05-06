using Dapper;
using FluentValidation;
using PortfolioWebsite.Database;
using PortfolioWebsite.Objects;
using PortfolioWebsite.Objects.InputObjects;

namespace PortfolioWebsite.Validators
{
    public class InputProjectValidator : AbstractValidator<InputProject>
    {
        private static MawaddaDbContext _context;

        public InputProjectValidator(MawaddaDbContext context)
        {
            _context = context;

            RuleFor(p => p).MustAsync(NotExistInDB).WithMessage("project already exists.");
        }

        private static async Task<bool> NotExistInDB(InputProject project, CancellationToken token)
        {
            var conn = await _context.ConnectionFactory.CreateConnection();

            var parms = new DynamicParameters();
            parms.Add("@Name", project.Name);
            parms.Add("@Description", project.Description);
            parms.Add("@Link", project.Link);
            parms.Add("@ImageUrl", project.ImageUrl);

            var sql = @"SELECT id FROM Projects
                        WHERE name = @Name
                        and description = @Description
                        and link = @Link
                        and imageurl = @ImageUrl";

            var result = await conn.QueryFirstOrDefaultAsync<Project>(sql, parms);

            return result == default;
        }
    }
}
