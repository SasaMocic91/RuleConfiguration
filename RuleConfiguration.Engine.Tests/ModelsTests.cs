using NUnit.Framework;
using RuleConfiguration.Models;

namespace RuleConfiguration.Engine.Tests;

[TestFixture]
public class ModelsTests
{
    [Test]
    public void Models_Ticket()
    {
        
        var match = new Match();
        match.Away = "abc";
        match.Home = "bac";
        match.Id = 1;
        match.Tournament = new Tournament
        {
            Id = 1,
            Name = "Test"
        };
        
        var ev = new Event();
        ev.Id = 1;
        ev.Match = match;
        ev.Odd = 1.3M;
        ev.Outcome = "1";

        Assert.That(match.Away, Is.Not.Empty);
        Assert.That(match.Home, Is.Not.Empty);
        Assert.That(match.Id , Is.EqualTo(1));
        Assert.That(match.Tournament, Is.Not.Null);
        Assert.That(match.Tournament.Id, Is.EqualTo(1));
        Assert.That(match.Tournament.Name, Is.EqualTo("Test"));
        
        Assert.That(ev.Id, Is.EqualTo(1));
        Assert.That(ev.Match, Is.TypeOf<Match>());
        Assert.That(1.3M, Is.EqualTo(ev.Odd));
        Assert.That("1", Is.EqualTo(ev.Outcome));
        
        var ticket = new Ticket();
        ticket.TenantId = Guid.NewGuid();
        ticket.Id = 1;
        ticket.Bonus = 10M;
        ticket.Tax = 10M;
        ticket.Payin = 100M;
        ticket.Odds = 10M;
        ticket.WinAmount = 1000M;
        ticket.Events = new List<Event>() { ev };
        Assert.That(1, Is.EqualTo(ticket.Id));
        Assert.That(10M, Is.EqualTo(ticket.Tax));
        Assert.That(10M, Is.EqualTo(ticket.Bonus));
        Assert.That(100M, Is.EqualTo(ticket.Payin));
        Assert.That(1000M, Is.EqualTo(ticket.WinAmount));
        Assert.That(1, Is.EqualTo(ticket.Events.Count));

       
        
        
      
    }
}