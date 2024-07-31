
using RuleConfiguration.Engine.Common;

namespace RuleConfiguration.Models;

public class FilterStatementProperties
{
    public Connector Connector { get; set; }

    public string PropertyId { get; set; }

    public string Operation { get; set; }
    public object Value { get; set; }
    public object Value2 { get; set; }
}