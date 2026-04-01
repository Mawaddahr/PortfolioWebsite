using HotChocolate.AspNetCore;
using PortfolioWebsite.Database;  

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGraphQLServer();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IConnectionFactory>(_ => new NpgsqlConnectionFactory(builder.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapGraphQL();

app.Run();

{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<NpgsqlConnectionFactory>();
    await context.Init();
}
