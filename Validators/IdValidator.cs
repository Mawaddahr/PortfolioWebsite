using Dapper;
using FluentValidation;
using PortfolioWebsite.Database;

namespace PortfolioWebsite.Validators
{
    public class IdValidator : AbstractValidator<(string id, string table)>
    {
        private static MawaddaDbContext _context;
        public IdValidator(MawaddaDbContext context)
        {
            _context = context;
            RuleFor(t => t.id)
                .MustAsync((t, id, token) => ExistInDB(id, t.table, token))
                .WithMessage($"Id does not exist.");
        }

        private static async Task<bool> ExistInDB(string Id,
            string table, CancellationToken token)
        {
            using var conn = await _context.ConnectionFactory.CreateConnection();

            var parms = new DynamicParameters();
            parms.Add("@Id", Id);

            var sql = $"SELECT id FROM {table} WHERE id = @Id";
            var result = await conn.QueryFirstOrDefaultAsync<string>(sql, parms);

            return result != null;
        }
    }
}
