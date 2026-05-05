using Dapper.FluentMap;
using DataAnnotatedModelValidations;
using FluentValidation;
using FluentValidation.AspNetCore;
using PortfolioWebsite.Database;
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
builder.Services.AddKeyedScoped<IValidator<InputExperience>, ExperienceValidator>("experienceValidator");
builder.Services.AddKeyedScoped<EducationService>("educationService");
builder.Services.AddKeyedScoped<ProjectService>("projectService");
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
                            policy.WithOrigins("http://localhost:5173")
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

{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MawaddaDbContext>();
    await context.Init();
}

app.Run();
