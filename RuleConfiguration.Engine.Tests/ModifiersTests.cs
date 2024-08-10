using Microsoft.Extensions.Caching.Memory;
using Mongo2Go;
using NUnit.Framework;
using RuleConfiguration.Engine.Tests.FakeData;
using RuleConfiguration.Engine.Tests.Helpers;
using RuleConfiguration.Models;
using RuleConfiguration.Modifiers.ModelModifiers;
using RuleConfiguration.RuleCaches;

namespace RuleConfiguration.Engine.Tests;

[TestFixture]
public class ModifierTests
{
    private readonly Guid _tenantId = Guid.NewGuid();

    [Test]
    public async Task MongoDb_AddNewRule_Tax()
    {
        var memCache = new MemoryCache(new MemoryCacheOptions());
        using var mongoRunner = MongoDbRunner.Start();
        var mongoDb = MongoHelper.GetDb(mongoRunner);
        var rulesCache = new TicketRulesCache(memCache, mongoDb);

        var rule = GenerateFakeRules.GenerateTaxRule(_tenantId);
        var modifierRepo = new TicketModifiers(rulesCache);

        await mongoDb.CreateRule(rule);
        await rulesCache.StoreConfiguration(rule.TenantId);

        var ticket = GenerateFakeBettingData.FakeHighTickets(1).Single();
        ticket.TenantId = _tenantId;

        Assert.That(0, Is.EqualTo(ticket.Bonus));

        await modifierRepo.ApplyModifiers(ticket);
        Assert.That(0, Is.Not.EqualTo(ticket.Tax));
    }

    [Test]
    public async Task MongoDb_AddNewRule_Jackpot()
    {
        var memCache = new MemoryCache(new MemoryCacheOptions());

        using var mongoRunner = MongoDbRunner.Start();
        var mongoDb = MongoHelper.GetDb(mongoRunner);
        var rulesCache = new TicketRulesCache(memCache, mongoDb);

        var modifierRepo = new TicketModifiers(rulesCache);

        var rule = GenerateFakeRules.GenerateJackpotRule(_tenantId);

        await mongoDb.CreateRule(rule);
        await rulesCache.StoreConfiguration(rule.TenantId);

        var ticket = GenerateFakeBettingData.FakeHighTickets(1).Single();
        ticket.TenantId = _tenantId;

        Assert.That(0, Is.EqualTo(ticket.Bonus));

        await modifierRepo.ApplyModifiers(ticket);
        Assert.That(0, Is.Not.EqualTo(Jackpot.TotalAmount));
    }

    [Test]
    public async Task MongoDb_AddNewRule_Bonus()
    {
        var memCache = new MemoryCache(new MemoryCacheOptions());

        using var mongoRunner = MongoDbRunner.Start();
        var mongoDb = MongoHelper.GetDb(mongoRunner);
        var rulesCache = new TicketRulesCache(memCache, mongoDb);

        var modifierRepo = new TicketModifiers(rulesCache);

        var rule = GenerateFakeRules.GenerateBonusRule(_tenantId);

        await mongoDb.CreateRule(rule);
        await rulesCache.StoreConfiguration(rule.TenantId);

        var ticket = GenerateFakeBettingData.FakeHighTickets(1).Single();
        ticket.TenantId = _tenantId;

        Assert.That(0, Is.EqualTo(ticket.Bonus));
        Assert.That(0, Is.EqualTo(ticket.Tax));

        var result = await modifierRepo.ApplyModifiers(ticket);
        Assert.That(0, Is.Not.EqualTo(result.Bonus));
    }

    [Test]
    public async Task MongoDb_AddNewRule_BonusAndJackpot()
    {
        var memCache = new MemoryCache(new MemoryCacheOptions());

        using var mongoRunner = MongoDbRunner.Start();
        var mongoDb = MongoHelper.GetDb(mongoRunner);
        var rulesCache = new TicketRulesCache(memCache, mongoDb);

        var modifierRepo = new TicketModifiers(rulesCache);

        var bonusRule = GenerateFakeRules.GenerateBonusRule(_tenantId);

        var jackpotRule = GenerateFakeRules.GenerateJackpotRule(_tenantId);

        await mongoDb.CreateRule(bonusRule);
        await mongoDb.CreateRule(jackpotRule);
        await rulesCache.StoreConfiguration(_tenantId);

        var ticket = GenerateFakeBettingData.FakeHighTickets(1).Single();
        ticket.TenantId = _tenantId;

        Assert.That(0, Is.EqualTo(ticket.Bonus));
        Assert.That(0, Is.EqualTo(Jackpot.TotalAmount));

        var result = await modifierRepo.ApplyModifiers(ticket);
        Assert.That(0, Is.Not.EqualTo(result.Bonus));
        Assert.That(0, Is.Not.EqualTo(Jackpot.TotalAmount));
    }
}