using Dapper.FluentMap;
using DataAnnotatedModelValidations;
using HotChocolate.AspNetCore;
using PortfolioWebsite.Database;
using PortfolioWebsite.Operations.Mutations;
using PortfolioWebsite.Operations.Queries;
using PortfolioWebsite.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddTransient<MawaddaDbContext>(_
    => new MawaddaDbContext(new NpgsqlConnectionFactory(builder.Configuration)));
builder.Services.AddKeyedScoped<ExperienceService>("experienceService");
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

app.MapGraphQL();

{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MawaddaDbContext>();
    await context.Init();
}

app.Run();
