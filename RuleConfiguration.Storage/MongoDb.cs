using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RuleConfiguration.Storage.DbModels;
using RuleConfiguration.Storage.Models;

namespace RuleConfiguration.Storage;

public interface IMongoDb
{
    IMongoCollection<Rule> GetCollection(IMongoDatabase db);
    Task<List<Rule>> GetRulesList(Guid tenantId);

    Task<Rule?> GetRule(Guid tenantId, string name);

    Task<Guid> CreateRule(Rule newRule);

    Task UpdateRule(Guid tenantId, Guid id, Rule updatedRule);

    Task RemoveRule(Guid tenantId, string name);
}

public class MongoDb : IMongoDb
{
    private readonly IMongoCollection<Rule> _ruleCollection;

    public MongoDb(IOptions<DbSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

        var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

        _ruleCollection = GetCollection(mongoDb);
    }

    public IMongoCollection<Rule> GetCollection(IMongoDatabase database)
    {
        var collection = database.GetCollection<Rule>("Rules");

        return collection;
    }


    public async Task<List<Rule>> GetRulesList(Guid tenantId)
    {
        var result = await _ruleCollection.Find(x => x.TenantId == tenantId).ToListAsync();
        return result;
    }

    public async Task<Rule?> GetRule(Guid tenantId, string name)
    {
        var result = await _ruleCollection.Find(x => x.Name == name && x.TenantId == tenantId)
            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<Guid> CreateRule(Rule newRule)
    {
        await _ruleCollection.InsertOneAsync(newRule);
        return newRule.Id;
    }

    public async Task UpdateRule(Guid tenantId, Guid id, Rule updatedRule)
    {
        await _ruleCollection.ReplaceOneAsync(x => x.Id == id && x.TenantId == tenantId, updatedRule);
    }

    public async Task RemoveRule(Guid tenantId, string name)
    {
        await _ruleCollection.DeleteOneAsync(x => x.Name == name);
    }
}