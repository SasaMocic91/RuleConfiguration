
using MongoDB.Bson;
using RuleConfiguration.Engine.Common;

namespace RuleConfiguration.Models;

public class FilterStatementProperties
{
    public Connector Connector { get; set; }

    public string PropertyId { get; set; }

    public string Operation { get; set; }
    public BsonValue Value { get; set; }
    public BsonValue Value2 { get; set; }
}