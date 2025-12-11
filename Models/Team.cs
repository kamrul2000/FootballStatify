using System.Text.Json.Serialization;

namespace MyApp.Models
{
    public class Team
    {
        public int Id { get; set; }         // Primary Key
        public string Name { get; set; }    // Team Name
        public string Coach { get; set; }   // Coach Name

        public int FoundingYear { get; set; } // Year the team was founded

        // Navigation property: One team has many players
        [JsonIgnore]

        public List<Player> Players { get; set; }
    }
}
