using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PortfolioWebsite.Auth;
using Path = System.IO.Path;

namespace PortfolioWebsite.Endpoints
{
    public static class UploadEndPoints
    {
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        private static readonly Dictionary<string, string> AllowedImageTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg",
            [".png"] = "image/png",
            [".gif"] = "image/gif",
            [".webp"] = "image/webp",
            [".avif"] = "image/avif",
        };

        public static void MapUploadEndpoints(this WebApplication app)
        {
            app.MapPost("/api/img-upload", async (IFormFile file, HttpRequest request, IConfiguration config, CancellationToken cancellationToken) =>
            {
                if (file.Length == 0 || file.Length > MaxFileSizeBytes)
                {
                    return Results.BadRequest(new { error = "Afbeelding moet tussen 0 en 5 MB zijn." });
                }

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedImageTypes.TryGetValue(extension, out var contentType))
                {
                    return Results.BadRequest(new { error = "Alleen jpg, png, gif, webp en avif zijn toegestaan." });
                }

                var fileName = $"{Guid.NewGuid()}{extension}";

                // In Azure: opslaan in Blob Storage, zodat afbeeldingen een herstart overleven.
                var blobConnectionString = config["Storage:ConnectionString"];
                if (!string.IsNullOrWhiteSpace(blobConnectionString))
                {
                    var containerName = config["Storage:Container"] ?? "images";
                    var container = new BlobContainerClient(blobConnectionString, containerName);
                    var blob = container.GetBlobClient(fileName);

                    await using var uploadStream = file.OpenReadStream();
                    await blob.UploadAsync(uploadStream, new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders { ContentType = contentType },
                    }, cancellationToken);

                    return Results.Ok(new { imageUrl = blob.Uri.ToString() });
                }

                // Lokaal: opslaan in wwwroot/imguploads, zoals voorheen.
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "imguploads"
                );
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                }

                var imageUrl = $"{request.Scheme}://{request.Host}/imguploads/{fileName}";
                return Results.Ok(new { imageUrl });
            })
            .DisableAntiforgery()
            .RequireAuthorization(AdminAuth.Policy);
        }
    }
}
