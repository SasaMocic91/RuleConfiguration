using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using RuleConfiguration.Models;
using RuleConfiguration.Storage;

namespace RuleConfiguration.Engine.Tests.Helpers;


public static class MongoHelper
{
    public static IMongoCollection<Rule> GetCollection(MongoDbRunner runner)
    {
        var db = new MongoClient(runner.ConnectionString).GetDatabase("Configurator");

        db.CreateCollection("Rules");

        var collection = db.GetCollection<Rule>("Rules");

        return collection;
    }

    public static IMongoDb GetDb(MongoDbRunner runner)
    {
        var dbsettings = new DbSettings
        {
            ConnectionString = runner.ConnectionString,
            DatabaseName = "Configurator",
            RulesCollectionName = "Rules"
        };
        var options = Options.Create(dbsettings);
        var mongo = new MongoDb(options);
        return mongo;
    }
}