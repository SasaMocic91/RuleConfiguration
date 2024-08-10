using Microsoft.Extensions.Caching.Memory;
using Mongo2Go;
using NUnit.Framework;
using RuleConfiguration.Engine.Tests.Helpers;
using RuleConfiguration.Handlers;
using RuleConfiguration.Repos;
using RuleConfiguration.Storage;

namespace RuleConfiguration.Engine.Tests;

[TestFixture]
public class TicketHandlerTests
{
    private readonly Guid _tenantId = Guid.NewGuid();

    private IRepositoryWrapper SetupModifierRepo(IRuleRepository db)
    {
        var memCache = new MemoryCache(new MemoryCacheOptions());
        var modifierRepo = new RepositoryWrapper(memCache, db);
        return modifierRepo;
    }

    [Test]
    public async Task TicketHandler_CheckTicket()
    {
        using var mongoRunner = MongoDbRunner.Start();
        var mongoDb = MongoHelper.GetDb(mongoRunner);

        var modifierRepo = SetupModifierRepo(mongoDb);
        var ticketHandler = new TicketHandler(modifierRepo);

        var ticket = FakeData.GenerateFakeBettingData.FakeHighTickets(1).First();
        ticket.TenantId = _tenantId;

        await ticketHandler.CheckTicket(ticket);
    }
}