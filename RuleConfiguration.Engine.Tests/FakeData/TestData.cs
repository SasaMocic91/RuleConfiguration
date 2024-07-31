using RuleConfiguration.Models;

namespace RuleConfiguration.Engine.Tests.FakeData;

public static class TestData
{
    public static List<Match> Matches = new()
    {
        new Match
        {
            Id = 1,
            Home = "HT",
            Away = "AT",
            Tournament = new Tournament
            {
                Id = 1,
                Name = "USA"
            }
        },
        new Match
        {
            Id = 2,
            Home = "Hteam",
            Away = "Ateam",
            Tournament = new Tournament
            {
                Id = 2,
                Name = "HT"
            }
        }
    };
}