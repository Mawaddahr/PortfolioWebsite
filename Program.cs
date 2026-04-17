using Dapper.FluentMap;
using HotChocolate.AspNetCore;
using PortfolioWebsite.Database;
using PortfolioWebsite.Operations;
using PortfolioWebsite.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddTransient<MawaddaDbContext>(_
    => new MawaddaDbContext(new NpgsqlConnectionFactory(builder.Configuration)));
builder.Services.AddKeyedScoped<ExperienceService>("experienceService");
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

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
