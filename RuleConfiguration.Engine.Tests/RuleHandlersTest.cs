using System.Text.Json;
using Mongo2Go;
using NUnit.Framework;
using RuleConfiguration.Engine.Operations;
using RuleConfiguration.Engine.Tests.Helpers;
using RuleConfiguration.Handlers;
using RuleConfiguration.Modifiers;
using RuleConfiguration.Requests;
using RuleConfiguration.Storage.DbModels;

namespace RuleConfiguration.Engine.Tests;

[TestFixture]
public class RuleHandlersTest
{
    private readonly Guid _tenantId = Guid.NewGuid();

    [Test]
    public async Task RuleHandler_Create_Success()
    {
        using var mongoRunner = MongoDbRunner.Start();
        var mongoDb = MongoHelper.GetDb(mongoRunner);

        var handler = new RuleHandler(mongoDb);

        var request = new AddRuleRequest
        {
            TenantId = _tenantId,
            Name = "TaxRule",
            Conditions = new List<RuleFilters>
            {
                new()
                {
                    PropertyId = "WinAmount",
                    Operation = nameof(Operation.GreaterThanOrEqualTo),
                    Value = JsonSerializer.SerializeToElement(1000M),
                    Value2 = default
                }
            },
            Modifiers = new List<Modifier>
            {
                new()
                {
                    Enabled = true,
                    Name = nameof(TaxModifier)
                }
            },
            Type = "Tax",
            Operator = "and"
        };


        var result = await handler.StoreRule(request);
        Assert.That(Guid.Empty, Is.Not.EqualTo(result));
    }

    [Test]
    public async Task RuleHandler_GetRule_Success()
    {
        using var mongoRunner = MongoDbRunner.Start();
        var mongoDb = MongoHelper.GetDb(mongoRunner);

        var handler = new RuleHandler(mongoDb);

        var request = new AddRuleRequest
        {
            TenantId = _tenantId,
            Name = "TaxRule",
            Conditions = new List<RuleFilters>
            {
                new()
                {
                    PropertyId = "WinAmount",
                    Operation = nameof(Operation.GreaterThanOrEqualTo),
                    Value = JsonSerializer.SerializeToElement(1000M),
                    Value2 = default
                }
            },
            Modifiers = new List<Modifier>
            {
                new()
                {
                    Enabled = true,
                    Name = nameof(TaxModifier)
                }
            },
            Type = "Tax",
            Operator = "and"
        };

        var result = await handler.StoreRule(request);
        Assert.That(Guid.Empty, Is.Not.EqualTo(result));

        var getRule = await handler.GetRule(request.TenantId, request.Name);
        Assert.That(getRule, Is.Not.Null);
    }

    [Test]
    public async Task RuleHandler_GetRules_Success()
    {
        using var mongoRunner = MongoDbRunner.Start();
        var mongoDb = MongoHelper.GetDb(mongoRunner);

        var handler = new RuleHandler(mongoDb);

        var request = new AddRuleRequest
        {
            TenantId = _tenantId,
            Name = "TaxRule",
            Conditions = new List<RuleFilters>
            {
                new()
                {
                    PropertyId = "WinAmount",
                    Operation = nameof(Operation.GreaterThanOrEqualTo),
                    Value = JsonSerializer.SerializeToElement(1000M),
                    Value2 = default
                }
            },
            Modifiers = new List<Modifier>
            {
                new()
                {
                    Enabled = true,
                    Name = nameof(TaxModifier)
                }
            },
            Type = "Tax",
            Operator = "and"
        };


        var result = await handler.StoreRule(request);
        Assert.That(Guid.Empty, Is.Not.EqualTo(result));

        var getRules = await handler.GetRules(request.TenantId);
        Assert.That(getRules, Is.Not.Null);
    }

    [Test]
    public async Task RuleHandler_UpdateRule_Success()
    {
        using var mongoRunner = MongoDbRunner.Start();
        var mongoDb = MongoHelper.GetDb(mongoRunner);

        var handler = new RuleHandler(mongoDb);

        var addRuleRequest = new AddRuleRequest()
        {
            TenantId = _tenantId,
            Name = "TaxRule",
            Conditions = new List<RuleFilters>
            {
                new()
                {
                    PropertyId = "WinAmount",
                    Operation = nameof(Operation.GreaterThanOrEqualTo),
                    Value = JsonSerializer.SerializeToElement(1000M),
                    Value2 = default
                }
            },
            Modifiers = new List<Modifier>
            {
                new()
                {
                    Enabled = true,
                    Name = nameof(TaxModifier)
                }
            },
            Type = "Tax",
            Operator = "and"
        };

        var result = await handler.StoreRule(addRuleRequest);

        var request = new UpdateRuleRequest
        {
            Id = result,
            TenantId = _tenantId,
            Name = "TaxRuleUpdated",
            Conditions = new List<RuleFilters>
            {
                new()
                {
                    PropertyId = "WinAmount",
                    Operation = nameof(Operation.GreaterThanOrEqualTo),
                    Value = JsonSerializer.SerializeToElement(1000M),
                    Value2 = default
                }
            },
            Modifiers = new List<Modifier>
            {
                new()
                {
                    Enabled = true,
                    Name = nameof(TaxModifier)
                }
            },
            Type = "Tax",
            Operator = "and"
        };


        await handler.UpdateRule(request);

        var updatedRule = await handler.GetRule(request.TenantId, request.Name);


        Assert.That("TaxRuleUpdated", Is.EqualTo(updatedRule.Name));
    }
}