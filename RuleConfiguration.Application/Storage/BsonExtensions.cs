using System.Text.Json;
using MongoDB.Bson;

namespace RuleConfiguration.Storage;

public static class BsonExtensions
{
    public static BsonValue ToBsonValue(this JsonElement e, bool tryParseDateTimes = false)
    {
        return e.ValueKind switch
        {
            JsonValueKind.String => BsonValue.Create(e.GetString()),
            JsonValueKind.Number when e.TryGetInt32(out var v) => BsonValue.Create(v),
            JsonValueKind.Number when e.TryGetDecimal(out var v) => BsonValue.Create(v),
            JsonValueKind.Null => BsonValue.Create(null),
            JsonValueKind.True => BsonValue.Create(true),
            JsonValueKind.False => BsonValue.Create(false),
            JsonValueKind.Array => new BsonArray(e.EnumerateArray().Select(v => v.ToBsonValue(tryParseDateTimes))),
            JsonValueKind.Undefined => BsonNull.Value,
            _ => throw new NotSupportedException($"ToBsonValue: {e}")
        };
    }
}