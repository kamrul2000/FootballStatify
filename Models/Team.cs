namespace MyApp.Models
{
    public class Team
    {
        public int Id { get; set; }         // Primary Key
        public string Name { get; set; }    // Team Name
        public string Coach { get; set; }   // Coach Name

        // Navigation property: One team has many players
        public List<Player> Players { get; set; }
    }
}
