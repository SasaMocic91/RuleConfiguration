using RuleConfiguration.Storage;
using RuleConfigurator.Application.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("RuleConfiguration.Db"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoDb, MongoDb>();
builder.Services.AddSingleton<IRulesCache, RulesCache>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();