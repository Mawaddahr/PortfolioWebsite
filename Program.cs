using Dapper.FluentMap;
using DataAnnotatedModelValidations;
using FluentValidation;
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

builder.Services.AddCors(options =>
options.AddPolicy(name: AllowSpecificOrigins,
                        policy =>
                        {
                            policy.WithOrigins(["http://localhost:5173", "http://localhost:5174"])
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                        }));

FluentMapper.Initialize(config =>
config.AddMap(new ExperienceMap()));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
//app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseCors(AllowSpecificOrigins);
app.MapGraphQL();
app.UseStaticFiles();
app.MapUploadEndpoints();

{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MawaddaDbContext>();
    await context.Init();
}

app.Run("http://localhost:5142");
