namespace MyApp.Models
{
    public class PlayerStat
    {
        public int Id { get; set; }

        // Relationship with Player
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        // Relationship with Match
        public int MatchId { get; set; }
        public Match Match { get; set; }

        // Number of goals scored by this player in this match
        public int Goals { get; set; }
    }
}
