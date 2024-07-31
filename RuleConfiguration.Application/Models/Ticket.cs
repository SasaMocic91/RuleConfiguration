namespace RuleConfigurator.Application.Models;

public class Ticket
{
    public int Id { get; set; }

    public List<Event> Events { get; set; }

    public decimal Odds { get; set; }

    public decimal Payin { get; set; }

    public decimal WinAmount { get; set; }

    public decimal Bonus { get; set; }

    public decimal Tax { get; set; }

    public Guid TenantId { get; set; }
}