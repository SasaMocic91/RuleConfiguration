using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;
using RuleConfiguration.Handlers;
using RuleConfiguration.Models;
using RuleConfiguration.Repos;
using RuleConfiguration.Requests;
using RuleConfiguration.Storage;
using RuleConfiguration.Storage.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("RuleConfiguration.Db"));



builder.Services.AddSingleton<IMongoClient>(_ =>
{
    var connectionString = builder.Configuration.GetValue<string>("RuleConfiguration.Db.ConnectionString");
    return new MongoClient(connectionString);
});

builder.Services.AddSingleton<IRuleRepository, RuleRepository>();
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddTransient<IRuleHandler, RuleHandler>();
builder.Services.AddTransient<ITicketHandler, TicketHandler>();
builder.Services.AddMemoryCache();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/rules",
        async (IRuleHandler ruleHandler, AddRuleRequest request) =>
        {
            var result = await ruleHandler.StoreRule(request);
            return Results.Ok();
        })
    .WithName("AddRule");

app.MapGet("/rules/{tenantId}/{name}", async (IRuleHandler ruleHandler, Guid tenantId, string name) =>
    {
        var result = await ruleHandler.GetRule(tenantId, name);
        return result is not null ? Results.Ok(result) : Results.BadRequest();
    })
    .WithName("GetRule");

app.MapGet("/rules/{tenantId}", async (IRuleHandler ruleHandler, Guid tenantId) =>
    {
        var result = await ruleHandler.GetRules(tenantId);
        return result is not null ? Results.Ok(result) : Results.BadRequest();
    })
    .WithName("GetRules");

app.MapPost("/tickets", async (ITicketHandler ticketHandler, Ticket ticket) =>
    {
        var result = await ticketHandler.CheckTicket(ticket);
        return result is not null ? Results.Ok(result) : Results.BadRequest();
    })
    .WithName("CheckTicket");


app.Run();


[ExcludeFromCodeCoverage]
public static partial class Program
{
}