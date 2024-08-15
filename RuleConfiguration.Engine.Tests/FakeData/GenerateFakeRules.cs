using RuleConfiguration.Engine.Common;
using RuleConfiguration.Engine.Operations;
using RuleConfiguration.Modifiers;
using RuleConfiguration.Storage.DbModels;
using RuleConfiguration.Storage.Models;

namespace RuleConfiguration.Engine.Tests.FakeData;

public static class GenerateFakeRules
{
    public static Rule GenerateBonusRule(Guid tenantId)
    {
        return new Rule
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = "BonusRule",
            Conditions = new List<FilterStatementProperties>
            {
                new()
                {
                    PropertyId = "Events[Match].Tournament.Name",
                    Operation = nameof(Operation.EndsWith),
                    Value = "Test",
                    Value2 = null
                },
                new()
                {
                    PropertyId = "Events.Count",
                    Operation = nameof(Operation.Between),
                    Value = 3,
                    Value2 = 8
                }
            },
            Modifiers = new List<Modifier>
            {
                new()
                {
                    Enabled = true,
                    Name = nameof(BonusModifier)
                }
            },
            Type = "Bonus",
            Operator = "and"
        };
    }


    public static Rule GenerateJackpotRule(Guid tenantId)
    {
        return new Rule
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = "JackpotRule",
            Conditions = new List<FilterStatementProperties>
            {
                new()
                {
                    PropertyId = "Events[Match].Tournament.Name",
                    Operation = nameof(Operation.EndsWith),
                    Value = "Test",
                    Value2 = null
                }
            },
            Modifiers = new List<Modifier>
            {
                new()
                {
                    Enabled = true,
                    Name = nameof(JackpotModifier)
                }
            },
            Type = "Jackpot",
            Operator = "and"
        };
    }

    public static Rule GenerateTaxRule(Guid tenantId)
    {
        return new Rule
        {
            TenantId = tenantId,
            Name = "TaxRule",
            Conditions = new List<FilterStatementProperties>
            {
                new()
                {
                    PropertyId = "WinAmount",
                    Operation = nameof(Operation.GreaterThanOrEqualTo),
                    Value = 1000.0,
                    Value2 = null
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
    }
}