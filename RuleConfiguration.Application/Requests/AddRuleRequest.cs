using System.Text.Json;
using RuleConfiguration.Storage.DbModels;

namespace RuleConfiguration.Requests;

public class AddRuleRequest
{
    public Guid TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<RuleFilters> Conditions { get; set; } = new();

    public List<Modifier> Modifiers { get; set; } = new();

    public string Type { get; set; }

    public string Operator { get; set; }
}

public class RuleFilters
{
    /// <summary>
    ///     Property identifier conventionalized by for the Expression Builder.
    /// </summary>
    public string PropertyId { get; set; }

    /// <summary>
    ///     Express the interaction between the property and the constant value defined in this filter statement.
    /// </summary>
    public string Operation { get; set; }

    /// <summary>
    ///     Constant value that will interact with the property defined in this filter statement.
    /// </summary>
    public JsonElement Value { get; set; }

    /// <summary>
    ///     Constant value that will interact with the property defined in this filter statement when the operation demands a
    ///     second value to compare to.
    /// </summary>
    public JsonElement Value2 { get; set; }
}