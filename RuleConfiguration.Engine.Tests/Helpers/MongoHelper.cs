using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using RuleConfiguration.Storage;
using RuleConfiguration.Storage.Models;

namespace RuleConfiguration.Engine.Tests.Helpers;

public static class MongoHelper
{
    public static IRuleRepository GetDb(MongoDbRunner runner)
    {
        var dbsettings = new DbSettings
        {
            ConnectionString = runner.ConnectionString,
            DatabaseName = "Configurator",
            RulesCollectionName = "Rules"
        };


        var mongoClient = new MongoClient(dbsettings.ConnectionString);
        var options = Options.Create(dbsettings);
        var mongo = new RuleRepository(options, mongoClient);
        return mongo;
    }
    
}