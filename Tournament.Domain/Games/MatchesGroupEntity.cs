using Tournament.Domain.Tournaments;

namespace Tournament.Domain.Games
{
    public class MatchesGroupEntity : BaseEntity
    {
        public int Round { get; set; } // final - 0, semifinal - 1, quarter - 2, r16 - 3, r32 - 4, r64 - 5
        public int GroupName { get; set; } // alphabetical 1 - a, 2 - b, 3 - c and so on
        public RoundType RoundType { get; set; }
        public Guid TournamentGroupId { get; set; }
        public TournamentGroupEntity TournamentGroup { get; set; }
        public ICollection<MatchEntity> Matches { get; set; }
    }

    public enum RoundType
    {
        Tree = 1
    }
}
