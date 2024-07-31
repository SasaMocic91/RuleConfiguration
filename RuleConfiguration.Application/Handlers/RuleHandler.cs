using RuleConfiguration.Models;
using RuleConfiguration.Requests;
using RuleConfiguration.Storage;

namespace RuleConfiguration.Handlers;

public interface IRuleHandler
{
    Task<Guid> StoreRule(AddRuleRequest request);

    Task<Rule?> GetRule(Guid tenantId, string name);

    Task<List<Rule>?> GetRules(Guid tenantId);

    Task UpdateRule(UpdateRuleRequest request);
}

public class RuleHandler : IRuleHandler
{
    private readonly IMongoDb _rulesDb;

    public RuleHandler(IMongoDb rulesDb)
    {
        _rulesDb = rulesDb;
    }

    public async Task<Guid> StoreRule(AddRuleRequest request)
    {
        var newRule = new Rule
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            Name = request.Name,
            Conditions = request.Conditions.ConvertAll(x => new FilterStatementProperties
            {
                Connector = x.Connector,
                PropertyId = x.PropertyId,
                Operation = x.Operation,
                Value = x.Value.ToBsonValue(),
                Value2 = x.Value2.ToBsonValue()
            }),
            Modifiers = request.Modifiers,
            Type = request.Type,
            Operator = request.Operator
        };
        var result = await _rulesDb.CreateRule(newRule);
        return result;
    }

    public async Task<Rule?> GetRule(Guid tenantId, string name)
    {
        var result = await _rulesDb.GetRule(tenantId, name);
        return result;
    }

    public async Task<List<Rule>?> GetRules(Guid tenantId)
    {
        var result = await _rulesDb.GetRulesList(tenantId);
        return result;
    }

    public async Task UpdateRule(UpdateRuleRequest request)
    {
        var updateRule = new Rule
        {
            Id = request.Id,
            TenantId = request.TenantId,
            Name = request.Name,
            Conditions = request.Conditions.ConvertAll(x => new FilterStatementProperties
            {
                Connector = x.Connector,
                PropertyId = x.PropertyId,
                Operation = x.Operation,
                Value = x.Value.ToBsonValue(),
                Value2 = x.Value2.ToBsonValue()
            }),
            Modifiers = request.Modifiers,
            Type = request.Type,
            Operator = request.Operator
        };
        await _rulesDb.UpdateRule(request.TenantId, updateRule.Id, updateRule);
    }
}