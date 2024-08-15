using Microsoft.Extensions.Caching.Memory;
using RuleConfiguration.Storage.Models;

namespace RuleConfiguration.Storage.Repositories.Base;

public class BaseRulesCache<T>(IMemoryCache memoryCache, IMongoDb mongoDb) : IBaseRulesCache<T>
    where T : class
{
    public async Task StoreConfiguration(Guid tenantId)
    {
        var rulesDb = await mongoDb.GetRulesList(tenantId);
        var ordered = rulesDb.OrderByDescending(x => x.Conditions.Count).ToList();
        var rules = ordered.ToDictionary(x => x.Name);
        
        if (rules.Count == 0) return;

        var records = RuleCreator.CreateRuleRecords<T>(rules);
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(1));
        memoryCache.Set(tenantId, records, cacheEntryOptions);
    }

    public async Task<Dictionary<string, RuleRecord<T>>?> GetTenantConfig(Guid tenantId)
    {
        // Get From Cache
        memoryCache.TryGetValue(tenantId, out Dictionary<string, RuleRecord<T>>? cacheValue);
        if (cacheValue is not null) return cacheValue;
        var rulesDb = await mongoDb.GetRulesList(tenantId);
        var ordered = rulesDb.OrderByDescending(x => x.Conditions.Count).ToList();
        var rules = ordered.ToDictionary(x => x.Name);

        if (rules.Count == 0) return null;

        // Store if results came from mongoDB
        var records = RuleCreator.CreateRuleRecords<T>(rules);
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(1));
        memoryCache.Set(tenantId, records, cacheEntryOptions);
        return records;
    }


}