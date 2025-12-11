using MyApp.Models;
using System.Text.Json.Serialization;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Coach { get; set; } = null!;
    public int FoundingYear { get; set; }

    [JsonIgnore]                // Ignore in JSON (optional)
    public List<Player> Players { get; set; } = new List<Player>();
}
