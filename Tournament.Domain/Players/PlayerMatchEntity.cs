using Tournament.Domain.Games;

namespace Tournament.Domain.Players
{
    public class PlayerMatchEntity : BaseEntity
    {
        public Guid PlayerId { get; set; }
        public PlayerEntity Player { get; set; }
        public Guid MatchId { get; set; }
        public MatchEntity Match { get; set; }

        public Team Team { get; set; }
    }

    public enum Team
    {
        Team1 = 1,
        Team2 = 2
    }
}
