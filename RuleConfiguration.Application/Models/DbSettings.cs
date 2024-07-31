namespace RuleConfigurator.Application.Models;

public class DbSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string RulesCollectionName { get; set; } = null!;
}