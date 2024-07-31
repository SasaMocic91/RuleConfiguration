using Microsoft.Extensions.Caching.Memory;
using MongoDB.Bson;
using RuleConfiguration.Engine;
using RuleConfiguration.Engine.Common;
using RuleConfiguration.Engine.Generics;
using RuleConfiguration.Engine.Operations;
using RuleConfiguration.Models;
using RuleConfigurator.Application.Models;

namespace RuleConfiguration.Storage;

public interface IRulesCache
{
    public Task StoreConfiguration(Guid tenantId);

    public Task<Dictionary<string, RuleRecord>?> GetTenantConfig(Guid tenantId);
}

public class RulesCache : IRulesCache
{
    private readonly IMemoryCache _memoryCache;
    private readonly IMongoDb _mongoDb;

    public RulesCache(IMemoryCache memoryCache, IMongoDb mongoDb)
    {
        _memoryCache = memoryCache;
        _mongoDb = mongoDb;
    }


    public async Task StoreConfiguration(Guid tenantId)
    {
        var rulesDb = await _mongoDb.GetRulesList(tenantId);
        var rules = rulesDb.ToDictionary(x => x.Name);

        if (rules.Count == 0) return;

        var records = CreateRuleRecords(rules);
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(1));
        _memoryCache.Set(tenantId, records, cacheEntryOptions);
    }

    public async Task<Dictionary<string, RuleRecord>?> GetTenantConfig(Guid tenantId)
    {
        // Get From Cache
        _memoryCache.TryGetValue(tenantId, out Dictionary<string, RuleRecord>? cacheValue);
        if (cacheValue is not null) return cacheValue;
        var rulesDb = await _mongoDb.GetRulesList(tenantId);
        var rules = rulesDb.ToDictionary(x => x.Name);

        if (rules.Count == 0) return null;

        // Store if results came from mongoDB
        var records = CreateRuleRecords(rules);
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(1));
        _memoryCache.Set(tenantId, records, cacheEntryOptions);
        return records;
    }

    public Dictionary<string, RuleRecord> CreateRuleRecords(Dictionary<string, Rule> rules)
    {
        var dict = new Dictionary<string, RuleRecord>();
        foreach (var rule in rules)
        {
            var ruleRecord = new RuleRecord(rule.Key, rule.Value.Modifiers, rule.Value.Type, rule.Value.Operator);
            foreach (var condition in rule.Value.Conditions)
            {
                var filter = new Filter<Ticket>();

                var value1 = Convert(condition.Value);
                var value2 = Convert(condition.Value2);
                    
                filter.StartGroup();
                filter.By(condition.PropertyId, Operation.ByName(condition.Operation),
                    value1, value2,
                    condition.Connector);

                var filterBuilder = new FilterBuilder();
                var exp = filterBuilder.GetExpression<Ticket>(filter);
                ruleRecord.Expressions.Add(exp.Compile());
            }

            dict.Add(ruleRecord.Key, ruleRecord);
        }

        return dict;
    }


    private object? Convert(BsonValue? val)
    {
        if (val is null) return null;

        var type = val.GetType();

        object result;


        switch (type.Name)
        {
            case "BsonString":
                result = val.ToString();
                break;
            case "BsonInt32":
                result = int.Parse(val.ToString());
                break;
            case "BsonInt64":
                result = long.Parse(val.ToString()!);
                break;
            case "BsonDecimal128":
                result = decimal.Parse(val.ToString()!);
                break;
            case "BsonBoolean":
                result = bool.Parse(val.ToString());
                break;
            default: throw new NotSupportedException();
        }

        return result;
    }
}