namespace RuleConfiguration.Models;

public class Ticket
{
    public int Id { get; set; }

    public List<Event> Events { get; set; }

    public double Odds { get; set; }

    public double Payin { get; set; }

    public double WinAmount { get; set; }

    public double Bonus { get; set; }

    public double Tax { get; set; }

    public Guid TenantId { get; set; }
}