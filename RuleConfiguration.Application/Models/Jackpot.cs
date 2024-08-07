namespace RuleConfiguration.Models;

public static class Jackpot
{
    public static double TotalAmount { get; set; }

    public static double DailyJackpot { get; set; }

    public static double TournamentJackpot { get; set; }

    public static void Increase(Ticket ticket)
    {
        var forJackpot = ticket.Payin * 0.1;

        TotalAmount += forJackpot;

        DailyJackpot += forJackpot * 0.3;

        TournamentJackpot += forJackpot * 0.7;
    }

    public static Dictionary<string, double> CurrentAmount()
    {
        return new Dictionary<string, double>
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