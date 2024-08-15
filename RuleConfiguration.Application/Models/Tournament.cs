using System.Text.Json.Serialization;

namespace RuleConfiguration.Models;

public class Tournament
{
    [JsonPropertyName("TournamentId")]
    public int Id { get; set; }

    public string Name { get; set; }
}