namespace MyApp.Models
{
    public class Match
    {
        public int Id { get; set; }

        // Match Title (optional)
        public string Title { get; set; }

        // Foreign Keys
        public int TeamAId { get; set; }
        public int TeamBId { get; set; }

        // Navigation Properties
        public Team TeamA { get; set; }
        public Team TeamB { get; set; }

        public DateTime MatchDate { get; set; }
        public string Venue { get; set; }
    }
}
