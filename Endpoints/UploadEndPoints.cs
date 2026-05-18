using Path = System.IO.Path;

namespace PortfolioWebsite.Endpoints
{
    public static class UploadEndPoints
    {
        public static void MapUploadEndpoints(this WebApplication app)
        {
            app.MapPost("/api/img-upload", async (IFormFile file, HttpRequest request) =>
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "imguploads"
                );

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName =
                    $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                var filePath = Path.Combine(
                    uploadsFolder,
                    fileName
                );

                using var stream = new FileStream(filePath, FileMode.Create);

                await file.CopyToAsync(stream);

                var imageUrl =
                    $"{request.Scheme}://{request.Host}/uploads/{fileName}";

                return Results.Ok(new
                {
                    imageUrl
                });
            });
        }
    }
}
