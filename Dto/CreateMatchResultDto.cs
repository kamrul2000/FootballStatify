namespace MyApp.Dto
{
    public class CreateMatchResultDto
    {
        public int MatchId { get; set; }
        public int TeamAGoals { get; set; }
        public int TeamBGoals { get; set; }
        public List<PlayerGoalDto> Scorers { get; set; } = new List<PlayerGoalDto>();

    }
    public class PlayerGoalDto
    {
        public int PlayerId { get; set; }
        public int Goals { get; set; }   // Can be 1 or more if needed
    }
}
