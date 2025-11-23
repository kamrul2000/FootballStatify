namespace MyApp.Dto
{
    public class CreateMatchDto
    {
        public string Title { get; set; }
        public int TeamAId { get; set; }
        public int TeamBId { get; set; }
        public DateTime MatchDate { get; set; }
        public string Venue { get; set; }

    }
}
