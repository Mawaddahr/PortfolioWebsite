using Dapper;
using Dapper.FluentMap.Mapping;
using PortfolioWebsite.Objects;

namespace PortfolioWebsite.Database
{
    public class MawaddaDbContext
    {
        public IConnectionFactory ConnectionFactory { get; set; }

        public MawaddaDbContext(IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }
        public async Task Init(string? aboutMeSeedPath = null, string? publicApiUrl = null)
        {
            await InitTables();
            await SeedAboutMe(aboutMeSeedPath, publicApiUrl);
        }

        /// <summary>
        /// Vult de AboutMe-tabel eenmalig met de tekst uit Database/AbtMeText.json,
        /// zodat de site op een lege database niet leeg begint.
        /// </summary>
        private async Task SeedAboutMe(string? seedPath, string? publicApiUrl)
        {
            using var connection = await ConnectionFactory.CreateConnection();
            var count = await connection.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM AboutMe");
            if (count > 0 || seedPath is null)
            {
                return;
            }

            var seed = PortfolioWebsite.Services.AbtMeTextHandler.ReadSeedFromJson(seedPath);
            if (seed is null)
            {
                return;
            }

            var imageUrl = seed.ImageUrl ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(publicApiUrl))
            {
                imageUrl = imageUrl.Replace("http://localhost:5142", publicApiUrl.TrimEnd('/'));
            }

            await connection.ExecuteAsync(
                "INSERT INTO AboutMe (id, text, imageurl) VALUES (1, @Text, @ImageUrl) ON CONFLICT (id) DO NOTHING",
                new { seed.Text, ImageUrl = imageUrl });
        }

        private async Task InitTables()
        {
            string createTables = @"
            CREATE TABLE IF NOT EXISTS Experiences(
                id TEXT PRIMARY KEY,
                company VARCHAR(100) NOT NULL,
                location VARCHAR(100) NOT NULL,
                role VARCHAR(100) NOT NULL,
                description VARCHAR(500),
                startdate DATE NOT NULL,
                enddate DATE
                );
            
            CREATE TABLE IF NOT EXISTS Education(
                id TEXT PRIMARY KEY,
                institution VARCHAR(100) NOT NULL,
                studyprogram VARCHAR(100) NOT NULL,
                studyprogramtype VARCHAR(100) NOT NULL,
                startdate DATE NOT NULL,
                ongoing BOOLEAN NOT NULL,
                enddate DATE
            );

            CREATE TABLE IF NOT EXISTS Projects(
                id TEXT PRIMARY KEY,
                name VARCHAR(100) NOT NULL,
                description VARCHAR(500),
                link TEXT,
                imageurl TEXT
            );

            CREATE TABLE IF NOT EXISTS AboutMe(
                id INT PRIMARY KEY,
                text TEXT NOT NULL,
                imageurl TEXT
            );
            ";

            using var connection = await ConnectionFactory.CreateConnection();
            await connection.ExecuteAsync(createTables);
        }
    }

    public class ExperienceMap : EntityMap<Experience>
    {
        public ExperienceMap()
        {
            Map(e => e.Id).ToColumn("id");
            Map(e => e.Company).ToColumn("company");
            Map(e => e.Role).ToColumn("role");
            Map(e => e.Description).ToColumn("description");
            Map(e => e.StartDate).ToColumn("startdate");
            Map(e => e.EndDate).ToColumn("enddate");
            Map(e => e.Location).ToColumn("location");
        }

    }

    public class EducationMap : EntityMap<Education>
    {
        public EducationMap()
        {
            Map(e => e.Id).ToColumn("id");
            Map(e => e.Institution).ToColumn("institution");
            Map(e => e.StudyProgram).ToColumn("studyprogram");
            Map(e => e.StudyProgramType).ToColumn("studyprogramtype");
            Map(e => e.StartDate).ToColumn("startdate");
            Map(e => e.OnGoing).ToColumn("ongoing");
            Map(e => e.EndDate).ToColumn("enddate");
        }
    }

    public class ProjectMap : EntityMap<Project>
    {
        public ProjectMap()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.Name).ToColumn("name");
            Map(p => p.Description).ToColumn("description");
            Map(p => p.Link).ToColumn("link");
            Map(p => p.ImageUrl).ToColumn("ImageUrl");
        }
    }
}
