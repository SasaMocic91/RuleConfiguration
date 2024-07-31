using System.Text.Json;
using MongoDB.Bson;
using NUnit.Framework;
using RuleConfiguration.Storage;

namespace RuleConfiguration.Engine.Tests;

[TestFixture]
public class ExtensionTests
{
    [Test]
    public void BsonExtension_ReturnsGoodValue()
    {
        var decimalValue = JsonSerializer.SerializeToElement(1000.2M);
        var decimalConverted = decimalValue.ToBsonValue();
        Assert.That(BsonType.Decimal128, Is.EqualTo(decimalConverted.BsonType));

        var intValue = JsonSerializer.SerializeToElement(1000);
        var intConverted = intValue.ToBsonValue();
        Assert.That(BsonType.Int32,Is.EqualTo(intConverted.BsonType));

        var stringValue = JsonSerializer.SerializeToElement("test");
        var stringConverted = stringValue.ToBsonValue();
        Assert.That(BsonType.String, Is.EqualTo(stringConverted.BsonType));

        var trueValue = JsonSerializer.SerializeToElement(true);
        var trueConverted = trueValue.ToBsonValue();
        Assert.That(BsonType.Boolean, Is.EqualTo(trueConverted.BsonType));

        var falseValue = JsonSerializer.SerializeToElement(false);
        var falseConverted = falseValue.ToBsonValue();
        Assert.That(BsonType.Boolean, Is.EqualTo(falseConverted.BsonType));

        var arr = new int[2];
        var arrayValue = JsonSerializer.SerializeToElement(arr);
        var arrConverted = arrayValue.ToBsonValue();
        Assert.That(BsonType.Array, Is.EqualTo(arrConverted.BsonType));
    }
}