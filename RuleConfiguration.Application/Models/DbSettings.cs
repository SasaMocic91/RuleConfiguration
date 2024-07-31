using System.Diagnostics.CodeAnalysis;

namespace RuleConfiguration.Models;

[ExcludeFromCodeCoverage]
public class DbSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string RulesCollectionName { get; set; } = null!;
}