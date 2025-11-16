namespace MyApp.Models
{
    public class MatchResult
    {
        public int Id { get; set; }

        // Link with Match table
        public int MatchId { get; set; }
        public Match Match { get; set; }

        // Goals
        public int TeamAGoals { get; set; }
        public int TeamBGoals { get; set; }

        // Winner (Optional)
        public string Winner { get; set; }  // "Team A", "Team B", "Draw"
    }
}
