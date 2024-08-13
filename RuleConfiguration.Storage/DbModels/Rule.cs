using MongoDB.Bson.Serialization.Attributes;
using RuleConfiguration.Storage.Models;

namespace RuleConfiguration.Storage.DbModels;

[BsonIgnoreExtraElements]
public class Rule
{
    [BsonId]
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<FilterStatementProperties> Conditions { get; set; } = new();

    public List<Modifier> Modifiers { get; set; } = new();

    public string Type { get; set; }

    public string Operator { get; set; }
}

public class Modifier
{
    public bool Enabled { get; set; }

    public string Name { get; set; }
}