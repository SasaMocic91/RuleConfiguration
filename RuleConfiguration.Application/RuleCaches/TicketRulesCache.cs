using Microsoft.Extensions.Caching.Memory;
using RuleConfiguration.Models;
using RuleConfiguration.Storage;
using RuleConfiguration.Storage.Repositories.Base;

namespace RuleConfiguration.RuleCaches;

public interface ITicketRulesCache : IBaseRulesCache<Ticket>
{
    
}

public class TicketRulesCache(IMemoryCache memoryCache, IMongoDb mongoDb) : BaseRulesCache<Ticket>(memoryCache, mongoDb), ITicketRulesCache
{
    
}