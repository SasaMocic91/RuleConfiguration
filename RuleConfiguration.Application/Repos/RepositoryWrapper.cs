using Microsoft.Extensions.Caching.Memory;
using RuleConfiguration.Modifiers.ModelModifiers;
using RuleConfiguration.RuleCaches;
using RuleConfiguration.Storage;

namespace RuleConfiguration.Repos;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly IMemoryCache _cache;
    private readonly IMongoDb _mongoDb;
    private ITicketRulesCache _ticketRules;
    private ITicketModifiers _ticketModifiers;

    public RepositoryWrapper(IMemoryCache cache, IMongoDb mongoDb)
    {
        _cache = cache;
        _mongoDb = mongoDb;
    }

    public ITicketRulesCache TicketRules
    {
        get
        {
            if (_ticketRules == null)
            {
                _ticketRules = new TicketRulesCache(_cache, _mongoDb);
            }

            return _ticketRules;
        }
    }
    public ITicketModifiers TicketModifiers
    {
        get
        {
            if (_ticketModifiers == null)
            {
                _ticketModifiers = new TicketModifiers(TicketRules);
            }

            return _ticketModifiers;
        }
    }


    

}