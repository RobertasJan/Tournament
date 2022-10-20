using Tournament.Domain.Tournaments;

namespace Tournament.Domain.Games
{
    public class MatchesGroupEntity : BaseEntity
    {
        public int Round { get; set; } // final - 1, semifinal - 2, quarter - 3, r16 - 4, r32 - 5, r64 - 6
        public int GroupName { get; set; } // alphabetical 1 - a, 2 - b, 3 - c and so on

        public Guid TournamentGroupId { get; set; }
        public TournamentGroupEntity TournamentGroup { get; set; }
        public ICollection<MatchEntity> Matches { get; set; }
    }
}
