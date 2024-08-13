using RuleConfiguration.Storage.DbModels;

namespace RuleConfiguration.Storage.Repositories.Base;

public class BaseModifierRepo<T>(IBaseRulesCache<T> baseRulesCache) : IBaseModifierRepo<T>
    where T : class
{
    public List<IBaseModifier<T>> AllModifiers { get; set; } = new();

    public async Task<T> ApplyModifiers(T data)
    {
        var modifiers = await GetModifiers(data);
        foreach (var mod in modifiers) mod.Modify(data);

        return data;
    }
    
    private async Task<List<IBaseModifier<T>>> GetModifiers(T data)
    {
        var modifiers = await FilterModifiers(data, AllModifiers);
        return modifiers.Distinct().ToList();
    }
    
    private async Task<List<IBaseModifier<T>>> FilterModifiers(T data, List<IBaseModifier<T>> modifiers)
    {
        var config = await baseRulesCache.GetTenantConfig(GetTenantId(data));
        if (config == null) return new List<IBaseModifier<T>>();
        var mods = new List<IBaseModifier<T>>();
        foreach (var rule in config.Values)
        {
            var conditions = rule.Expressions.Select(x => x.Invoke(data)).ToList();
            switch (rule.Operator.ToLowerInvariant())
            {
                case "and" when conditions.All(x => x):
                    mods.AddRange(ApplyFilter(rule.Modifiers, modifiers));
                    break;
                case "or" when conditions.Any(x => x):
                    mods.AddRange(ApplyFilter(rule.Modifiers, modifiers));
                    break;
            }
        }

        return mods;
    }

    private List<IBaseModifier<T>> ApplyFilter(List<Modifier> ruleModifiers, List<IBaseModifier<T>> allModifiers)
    {
        var activeModifiers = ruleModifiers.Where(x => x.Enabled).Select(x => x.Name.ToLowerInvariant()).ToList();
        var result = allModifiers.Where(x => activeModifiers.Contains(x.GetModifierName().ToLowerInvariant())).ToList();

        return result;
    }
    
    private Guid GetTenantId<T>(T data)
    {
        var type = data.GetType();

        var prop = type.GetProperty("TenantId");

        var tenantId = prop.GetValue(data);

        return Guid.Parse(tenantId.ToString());
    }
}