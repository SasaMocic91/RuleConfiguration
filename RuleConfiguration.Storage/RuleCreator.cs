using MongoDB.Bson;
using RuleConfiguration.Engine;
using RuleConfiguration.Engine.Generics;
using RuleConfiguration.Engine.Operations;
using RuleConfiguration.Storage.DbModels;
using RuleConfiguration.Storage.Models;

namespace RuleConfiguration.Storage;

public static class RuleCreator
{
    public static Dictionary<string, RuleRecord<T>> CreateRuleRecords<T>(Dictionary<string, Rule> rules) where T : class
    {
        var dict = new Dictionary<string, RuleRecord<T>>();
        foreach (var rule in rules)
        {
            var ruleRecord = new RuleRecord<T>(rule.Key, rule.Value.Modifiers, rule.Value.Type, rule.Value.Operator);
            foreach (var condition in rule.Value.Conditions)
            {
                var filter = new Filter<T>();

                var value1 = Convert(condition.Value);
                var value2 = Convert(condition.Value2);

                filter.By(condition.PropertyId, Operation.ByName(condition.Operation),
                    value1, value2);

                var filterBuilder = new FilterBuilder();
                var exp = filterBuilder.GetExpression<T>(filter);
                ruleRecord.Expressions.Add(exp.Compile());
            }

            dict.Add(ruleRecord.Key, ruleRecord);
        }

        return dict;
    }


    private static object? Convert(BsonValue? val)
    {
        if (val is null) return null;

        var type = val.GetType();

        object result;

        switch (type.Name)
        {
            case "BsonString":
                result = val.ToString();
                break;
            case "BsonInt32":
                result = int.Parse(val.ToString());
                break;
            case "BsonInt64":
                result = long.Parse(val.ToString()!);
                break;
            case "BsonDecimal128":
                result = decimal.Parse(val.ToString()!);
                break;
            case "BsonBoolean":
                result = bool.Parse(val.ToString());
                break;
            case "BsonNull":
                result = null;
                break;
            default: throw new NotSupportedException();
        }

        return result;
    }
}