using Dapper;
using Npgsql.Replication.PgOutput.Messages;
using PortfolioWebsite.Database;
using PortfolioWebsite.Objects;
using PortfolioWebsite.Objects.InputObjects;

namespace PortfolioWebsite.Services
{
    public class ProjectService
    {
        private MawaddaDbContext _context { get; set; }

        public ProjectService(MawaddaDbContext context)
        {
            _context = context;
        }
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            var conn = await _context.ConnectionFactory.CreateConnection();

            var sql = @"SELECT * FROM Projects";

            var projects = await conn.QueryAsync<Project>(sql);

            return [.. projects];
        }

        public async Task<Project>? GetProjectByIdAsync(string Id)
        {
            var conn = await _context.ConnectionFactory.CreateConnection();
            var parms = new DynamicParameters();
            parms.Add("@Id", Id);
            var sql = @"SELECT * FROM Projects WHERE id = @Id";

            var project = await conn.QueryAsync<Project>(sql, parms);
            return project.FirstOrDefault();
        }

        public async Task InsertProjectAsync(InputProject inputProject)
        {
            Project project = new Project(
                inputProject.Name,
                inputProject.Description,
                inputProject.Link,
                inputProject.ImageUrl);

            var parms = new DynamicParameters();
            parms.Add("@Id", project.Id);
            parms.Add("@Name", project.Name);
            parms.Add("@Description", project.Description);
            parms.Add("@Link", project.Link);
            parms.Add("@ImageUrl", project.ImageUrl);

            var sql = @"INSERT INTO Projects (id, name, description, link, imageurl)
                        VALUES (@Id, @Name, @Description, @Link, @ImageUrl)";

            var conn = await _context.ConnectionFactory.CreateConnection();
            await conn.ExecuteAsync(sql, parms);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            var parms = new DynamicParameters();
            parms.Add("@Id", project.Id);
            parms.Add("@Name", project.Name);
            parms.Add("@Description", project.Description);
            parms.Add("@Link", project.Link);
            parms.Add("@ImageUrl", project.ImageUrl);

            var sql = @"UPDATE Projects
                        SET name = @Name,
                            description = @Description,
                            link = @Link,
                            imageurl = @ImageUrl
                        WHERE id = @Id";
            var conn = await _context.ConnectionFactory.CreateConnection();
            await conn.ExecuteAsync(sql, parms);
        }

        public async Task DeleteProjectAsync(string Id)
        {
            var parms = new DynamicParameters();
            parms.Add("@Id", Id);

            var sql = @"DELETE FROM Projects WHERE id = @Id";

            var conn = await _context.ConnectionFactory.CreateConnection();
            await conn.ExecuteAsync(sql, parms);
        }
    }
}
