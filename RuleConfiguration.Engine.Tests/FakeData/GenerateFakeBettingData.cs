using Bogus;
using RuleConfigurator.Application.Models;

namespace RuleConfiguration.Engine.Tests.FakeData;

public static class GenerateFakeBettingData
{
    private static readonly List<string> Outcomes = new() { "1", "X", "2" };


    public static List<Tournament> FakeTournaments(int count)
    {
        var tournaments = new Faker<Tournament>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => $"Turnir{f.Random.Int(1, 20)}")
            .Generate(count);
        return tournaments;
    }

    public static List<Match> FakeMatches(int count)
    {
        var matches = new Faker<Match>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Away, f => $"AwayTeam{f.Random.Int(1, 30)}")
            .RuleFor(x => x.Home, f => $"HomeTeam{f.Random.Int(1, 30)}")
            .RuleFor(x => x.Tournament, f => f.PickRandom(FakeTournaments(3)))
            .Generate(count);
        return matches;
    }

    public static List<Event> FakeEvents(int count, decimal minOdd, decimal maxOdd)
    {
        var events = new Faker<Event>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Match, f => f.PickRandom(FakeMatches(1)))
            .RuleFor(x => x.Odd, f => f.Random.Decimal(minOdd, maxOdd))
            .RuleFor(x => x.Outcome, f => f.PickRandom(Outcomes))
            .Generate(count);
        return events;
    }


    public static List<Ticket> FakeTickets(int count)
    {
        var events = FakeEvents(4, 1M, 5M);

        var odds = events.Aggregate(1M, (current, ev) => current * ev.Odd);

        var tickets = new Faker<Ticket>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Events, f => FakeEvents(4, 1M, 5M))
            .RuleFor(x => x.Odds, f => odds)
            .RuleFor(x => x.Payin, f => f.Random.Decimal(1M, 11M))
            .RuleFor(x => x.WinAmount, (f, t) => t.Odds * t.Payin)
            .Generate(count);

        return tickets;
    }

    public static List<Ticket> FakeHighTickets(int count)
    {
        var events = FakeEvents(4, 3M, 6M);

        var odds = events.Aggregate(1M, (current, ev) => current * ev.Odd);

        events[0].Match.Tournament.Id = 2;
        events[0].Match.Tournament.Name = "TurnirTest";

        var tickets = new Faker<Ticket>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Events, events)
            .RuleFor(x => x.Odds, odds)
            .RuleFor(x => x.Payin, 10M)
            .RuleFor(x => x.WinAmount, (f, t) => t.Odds * t.Payin)
            .Generate(count);

        return tickets;
    }
}