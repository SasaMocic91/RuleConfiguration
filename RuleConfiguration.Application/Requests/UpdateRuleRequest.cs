using RuleConfiguration.Storage.DbModels;

namespace RuleConfiguration.Requests;

public class UpdateRuleRequest
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<RuleFilters> Conditions { get; set; } = new();

    public List<Modifier> Modifiers { get; set; } = new();

    public string Type { get; set; }

    public string Operator { get; set; }
}