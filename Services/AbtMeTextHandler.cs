using Dapper;
using Newtonsoft.Json;
using PortfolioWebsite.Database;
using PortfolioWebsite.Objects;

namespace PortfolioWebsite.Services
{
    /// <summary>
    /// Leest en schrijft de "About me"-tekst. Die stond eerst in een JSON-bestand,
    /// maar in een container gaat dat verloren bij een herstart; daarom staat hij
    /// nu in de database (tabel AboutMe, altijd één rij met id 1).
    /// </summary>
    public class AbtMeTextHandler
    {
        private readonly MawaddaDbContext _context;

        public AbtMeTextHandler(MawaddaDbContext context)
        {
            _context = context;
        }

        public async Task<AboutMeText> ReadAsync()
        {
            using var conn = await _context.ConnectionFactory.CreateConnection();
            var row = await conn.QuerySingleOrDefaultAsync<AboutMeRow>(
                "SELECT text AS Text, imageurl AS ImageUrl FROM AboutMe WHERE id = 1");

            return row is null
                ? new AboutMeText(string.Empty, string.Empty)
                : new AboutMeText(row.Text ?? string.Empty, row.ImageUrl ?? string.Empty);
        }

        public sealed class AboutMeRow
        {
            public string? Text { get; set; }
            public string? ImageUrl { get; set; }
        }

        public async Task WriteAsync(AboutMeText abtMeText)
        {
            using var conn = await _context.ConnectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                @"INSERT INTO AboutMe (id, text, imageurl) VALUES (1, @Text, @ImageUrl)
                  ON CONFLICT (id) DO UPDATE SET text = EXCLUDED.text, imageurl = EXCLUDED.imageurl",
                new { abtMeText.Text, abtMeText.ImageUrl });
        }

        /// <summary>Leest de oorspronkelijke tekst uit Database/AbtMeText.json (alleen voor de eerste vulling).</summary>
        public static AboutMeText? ReadSeedFromJson(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<AboutMeText>(json);
        }
    }
}
