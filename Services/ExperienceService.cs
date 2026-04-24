using Dapper;
using PortfolioWebsite.Database;
using PortfolioWebsite.Objects;
using PortfolioWebsite.Objects.InputObjects;

namespace PortfolioWebsite.Services
{
    public class ExperienceService
    {
        private MawaddaDbContext _context { get; set; }

        public ExperienceService(MawaddaDbContext context)
        {
            _context = context;
        }

        // add pagination later on
        public async Task<List<Experience>?> GetAllExperienceAsync()
        {
            var conn = await _context.ConnectionFactory.CreateConnection();
            var sql = "SELECT * FROM Experiences";
            var query = await conn.QueryAsync<Experience>(sql);
            var experiences = query.ToList();
            return experiences;
        }

        public async Task<Experience> GetExperienceByIdAsync(string Id)
        {
            var conn = await _context.ConnectionFactory.CreateConnection();
            var parameters = new { ID = Id };
            var sql = "SELECT * FROM Experiences WHERE id = @ID";
            Experience experience = await conn.QueryFirstAsync<Experience>(sql, parameters);
            return experience;
        }

        public async Task InsertExperienceAsync(InputExperience inputexperience)
        {
            var conn = await _context.ConnectionFactory.CreateConnection();
            var parms = new DynamicParameters();
            Experience experience = new Experience(
                inputexperience.Company,
                inputexperience.Role,
                inputexperience.Description,
                inputexperience.StartDate,
                inputexperience.EndDate,
                inputexperience.Location
                );
            parms.Add("@Company", experience.Company);
            parms.Add("@Description", experience.Description);
            parms.Add("@Role", experience.Role);
            parms.Add("@Location", experience.Location);
            parms.Add("@StartDate", experience.StartDate, System.Data.DbType.Date);
            parms.Add("@EndDate", experience.EndDate, System.Data.DbType.Date);
            parms.Add("@Id", experience.Id);

            var sql = @"INSERT INTO Experiences (id, company, location, role, description, startdate, enddate)
                        VALUES (@Id, @Company, @Location, @Role, @Description, @StartDate, @EndDate)";
            await conn.ExecuteAsync(sql, parms);
        }

        public async Task UpdateExperienceAsync(Experience experience)
        {
            var conn = await _context.ConnectionFactory.CreateConnection();
            var parms = new DynamicParameters();
            parms.Add("@Company", experience.Company);
            parms.Add("@Description", experience.Description);
            parms.Add("@Role", experience.Role);
            parms.Add("@Location", experience.Location);
            parms.Add("@StartDate", experience.StartDate, System.Data.DbType.Date);
            parms.Add("@EndDate", experience.EndDate, System.Data.DbType.Date);
            parms.Add("@Id", experience.Id);
            var sql = @"UPDATE Experiences
                        SET company = @Company,
                            description = @Description,
                            role = @Role,
                            location = @Location,
                            startdate = @StartDate,
                            enddate = @enddate
                        WHERE id = @Id";
            await conn.ExecuteAsync(sql, parms);
        }

        public async Task DeleteExperienceByIdAsync(string Id)
        {
            Experience experience = await GetExperienceByIdAsync(Id);
            var conn = await _context.ConnectionFactory.CreateConnection();
            var parms = new DynamicParameters();
            parms.Add("@Id", experience.Id);
            var sql = @"DELETE FROM Experiences WHERE id = @Id";
            await conn.ExecuteAsync(sql, parms);
        }
    }
}
