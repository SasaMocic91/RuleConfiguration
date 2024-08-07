using RuleConfiguration.Models;
using RuleConfiguration.Repos;

namespace RuleConfiguration.Handlers;

public interface ITicketHandler
{
    Task<Ticket> CheckTicket(Ticket ticket);
}

public class TicketHandler : ITicketHandler
{
    private readonly IRepositoryWrapper _repository;

    public TicketHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }


    public async Task<Ticket> CheckTicket(Ticket ticket)
    {
        var result =await _repository.TicketModifiers.ApplyModifiers(ticket);
        return result;
    }
}