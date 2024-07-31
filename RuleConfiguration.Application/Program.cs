using System.Diagnostics.CodeAnalysis;
using RuleConfiguration.Models;
using RuleConfiguration.Modifiers;
using RuleConfiguration.Storage;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("RuleConfiguration.Db"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoDb, MongoDb>();
builder.Services.AddTransient<IRulesCache,RulesCache>();
builder.Services.AddTransient<IModifierRepo, ModifierRepo>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


[ExcludeFromCodeCoverage]
public static partial class Program
{
}