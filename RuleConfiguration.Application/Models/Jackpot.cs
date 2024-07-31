namespace RuleConfiguration.Models;

public static class Jackpot
{
    public static decimal TotalAmount { get; set; }

    public static decimal DailyJackpot { get; set; }

    public static decimal TournamentJackpot { get; set; }

    public static void Increase(Ticket ticket)
    {
        var forJackpot = ticket.Payin * 0.1M;

        TotalAmount += forJackpot;

        DailyJackpot += forJackpot * 0.3M;

        TournamentJackpot += forJackpot * 0.7M;
    }

    public static Dictionary<string, decimal> CurrentAmount()
    {
        return new Dictionary<string, decimal>
        {
            {
                nameof(TotalAmount), TotalAmount
            },
            {
                nameof(DailyJackpot), DailyJackpot
            },
            {
                nameof(TournamentJackpot), TournamentJackpot
            }
        };
    }
}