using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace PortfolioWebsite.Auth
{
    /// <summary>
    /// Controleert de Firebase ID-token die de admin-site meestuurt, en laat
    /// alleen de e-mailadressen uit Auth:AdminEmails wijzigingen doen.
    /// </summary>
    public static class AdminAuth
    {
        public const string Policy = "Admin";

        public static IServiceCollection AddFirebaseAdminAuth(
            this IServiceCollection services,
            IConfiguration config,
            IHostEnvironment environment)
        {
            var projectId = config["Auth:FirebaseProjectId"];
            if (string.IsNullOrWhiteSpace(projectId))
            {
                projectId = "portfoliowebsite-admin-7f818";
            }

            var adminEmails = (config["Auth:AdminEmails"] ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(email => email.ToLowerInvariant())
                .ToHashSet();

            // Lokaal zonder ingestelde adressen mag elke ingelogde gebruiker beheren;
            // in productie wordt dan alles geweigerd.
            var allowAnyUser = adminEmails.Count == 0 && environment.IsDevelopment();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://securetoken.google.com/{projectId}";
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = $"https://securetoken.google.com/{projectId}",
                        ValidateAudience = true,
                        ValidAudience = projectId,
                        ValidateLifetime = true,
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy, policy => policy
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context =>
                    {
                        if (allowAnyUser)
                        {
                            return true;
                        }

                        var email = context.User.FindFirst("email")?.Value?.ToLowerInvariant();
                        var emailVerified = context.User.FindFirst("email_verified")?.Value;

                        return email is not null
                            && string.Equals(emailVerified, "true", StringComparison.OrdinalIgnoreCase)
                            && adminEmails.Contains(email);
                    }));
            });

            return services;
        }
    }
}
