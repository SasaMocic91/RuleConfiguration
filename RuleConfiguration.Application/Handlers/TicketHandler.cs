using RuleConfiguration.Models;
using RuleConfiguration.Repos;

namespace RuleConfiguration.Handlers;

public interface ITicketHandler
{
    Task<Ticket> CheckTicket(Ticket ticket);
}

public class TicketHandler : ITicketHandler
{
    private readonly IRepoWrapper _repository;

    public TicketHandler(IRepoWrapper repository)
    {
        _repository = repository;
    }


    public async Task<Ticket> CheckTicket(Ticket ticket)
    {
        var result =await _repository.TicketModifiers.ApplyModifiers(ticket);
        return result;
    }
}