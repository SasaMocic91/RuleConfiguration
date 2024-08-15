using RuleConfiguration.Storage.Models;

namespace RuleConfiguration.Storage.Repositories.Base;

public interface IBaseRulesCache<T> where T : class
{
    public Task StoreConfiguration(Guid tenantId);

    public Task<Dictionary<string, RuleRecord<T>>?> GetTenantConfig(Guid tenantId);
}