using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RuleConfiguration.Engine.Common;

namespace RuleConfiguration.Storage.Models;

[BsonIgnoreExtraElements]
public class FilterStatementProperties
{
    public Connector Connector { get; set; }

    public string PropertyId { get; set; }

    public string Operation { get; set; }
    public BsonValue Value { get; set; }
    public BsonValue Value2 { get; set; }
}