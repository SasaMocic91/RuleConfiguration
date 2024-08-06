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

    private IRepoWrapper SetupModifierRepo(IMongoDb db)
    {
        var memCache = new MemoryCache(new MemoryCacheOptions());
        var modifierRepo = new RepoWrapper(memCache, db);
        return  modifierRepo;
    }

    [Test]
    public async Task TicketHandler_CheckTicket()
    {
        using var runner = MongoDbRunner.Start();
        var mongoDb = MongoHelper.GetDb(runner);

        var modifierRepo = SetupModifierRepo(mongoDb);
        var ticketHandler = new TicketHandler(modifierRepo);

        var ticket = FakeData.GenerateFakeBettingData.FakeHighTickets(1).First();
        ticket.TenantId = _tenantId;
        
        await ticketHandler.CheckTicket(ticket);
    }
}