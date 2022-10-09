using Tournament.Domain.Games;

namespace Tournament.Domain.Players
{
    public class PlayerMatchEntity
    {
        public int Id { get; set; }
        public Guid PlayerId { get; set; }
        public PlayerEntity Player { get; set; }
        public int MatchId { get; set; }
        public MatchEntity Match { get; set; }

        public Team Team { get; set; }
    }

    public enum Team
    {
        Team1 = 1,
        Team2 = 2
    }
}
