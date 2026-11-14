using Dapper.FluentMap;
using DataAnnotatedModelValidations;
using FluentValidation;
using HotChocolate.AspNetCore;
using PortfolioWebsite.Auth;
using PortfolioWebsite.Database;
using PortfolioWebsite.Endpoints;
using PortfolioWebsite.Objects;
using PortfolioWebsite.Objects.InputObjects;
using PortfolioWebsite.Operations.Mutations;
using PortfolioWebsite.Operations.Queries;
using PortfolioWebsite.Services;
using PortfolioWebsite.Validators;

var AllowSpecificOrigins = "_allowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Toegestane frontends. In Azure via Cors__AllowedOrigins__0, __1, ... ingesteld.
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
if (allowedOrigins is null || allowedOrigins.Length == 0)
{
    allowedOrigins = ["http://localhost:5000", "http://localhost:3000"];
}

builder.Services.AddOpenApi();
builder.Services.AddTransient<MawaddaDbContext>(_
    => new MawaddaDbContext(new NpgsqlConnectionFactory(builder.Configuration)));

builder.Services.AddKeyedScoped<ExperienceService>("experienceService");
builder.Services.AddKeyedScoped<IValidator<InputExperience>,
    InputExperienceValidator>("inputExperienceValidator");
builder.Services.AddKeyedScoped<IValidator<Experience>,
    ExperienceValidator>("experienceValidator");

builder.Services.AddKeyedScoped<EducationService>("educationService");
builder.Services.AddKeyedScoped<IValidator<InputEducation>,
    InputEducationValidator>("inputEducationValidator");
builder.Services.AddKeyedScoped<IValidator<Education>,
    EducationValidator>("educationValidator");

builder.Services.AddKeyedScoped<ProjectService>("projectService");
builder.Services.AddKeyedScoped<IValidator<InputProject>,
    InputProjectValidator>("inputProjectValidator");
builder.Services.AddKeyedScoped<IValidator<Project>,
    ProjectValidator>("projectValidator");

builder.Services.AddKeyedScoped<IValidator<(string id, string table)>,
    IdValidator>("idValidator");

builder.Services.AddKeyedScoped<AbtMeTextHandler>("abtMeTextHandler");

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .ModifyRequestOptions(o =>
    {
        o.IncludeExceptionDetails = builder.Environment.IsDevelopment();
    })
    .AddDataAnnotationsValidator()
    .AddQueryType(q => q.Name("Query"))
    .AddType<ExperienceQuery>()
    .AddType<EducationQuery>()
    .AddType<ProjectQuery>()
    .AddType<AbtMeTextQuery>()
    .AddMutationType(m => m.Name("Mutation"))
    .AddType<ExperienceMutation>()
    .AddType<EducationMutation>()
    .AddType<ProjectMutation>()
    .AddType<AbtMeTextMutation>();

Microsoft.Extensions.DependencyInjection.ValidationServiceCollectionExtensions.AddValidation(
    builder.Services,
    options => { }
);

// Alleen ingelogde admins (Firebase-token + e-mailadres uit Auth:AdminEmails) mogen wijzigen.
builder.Services.AddFirebaseAdminAuth(builder.Configuration, builder.Environment);

builder.Services.AddCors(options =>
options.AddPolicy(name: AllowSpecificOrigins,
                        policy =>
                        {
                            policy.WithOrigins(allowedOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                        }));
builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 |
                                   System.Security.Authentication.SslProtocols.Tls13;
    });
});

FluentMapper.Initialize(config =>
config.AddMap(new ExperienceMap()));
var app = builder.Build();

// Configure the HTTP request pipeline.
// Achter Azure Container Apps wordt HTTPS aan de voorkant afgehandeld. Met
// ASPNETCORE_FORWARDEDHEADERS_ENABLED=true leest de app de X-Forwarded-*-headers,
// zodat Request.Scheme gewoon "https" is. Een eigen HTTPS-check is daardoor niet nodig.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHsts();
}

app.UseCors(AllowSpecificOrigins);
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL().WithOptions(new GraphQLServerOptions
{
    // De GraphQL-IDE (Nitro) alleen lokaal tonen
    Tool = { Enable = app.Environment.IsDevelopment() },
});
app.MapUploadEndpoints();

{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MawaddaDbContext>();
    await context.Init(
        System.IO.Path.Combine(AppContext.BaseDirectory, "Database", "AbtMeText.json"),
        builder.Configuration["PublicApiUrl"]);
}

app.Run();
