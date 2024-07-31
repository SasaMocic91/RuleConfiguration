using RuleConfiguration.Models;
using RuleConfiguration.Modifiers;
using RuleConfiguration.Storage;

namespace RuleConfiguration.Handlers;

public interface ITicketHandler
{
    Task<Ticket> CheckTicket(Ticket ticket);
}

public class TicketHandler : ITicketHandler
{
    private readonly IModifierRepo _modifierRepo;

    public TicketHandler(IModifierRepo modifierRepo)
    {
        _modifierRepo = modifierRepo;
    }


    public async Task<Ticket> CheckTicket(Ticket ticket)
    {
        var result =await _modifierRepo.ApplyModifiers(ticket);
        return result;
    }
}