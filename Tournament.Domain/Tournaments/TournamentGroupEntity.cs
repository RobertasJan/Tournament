using Tournament.Domain.Games;
using MatchType = Tournament.Domain.Games.MatchType;

namespace Tournament.Domain.Tournaments
{
    public class TournamentGroupEntity : BaseEntity
    {
        public TournamentGroupTypes Type { get; set; }
        public string? OtherTypeName { get; set; }

        public MatchType MatchType { get; set; }
        public string? OtherMatchTypeName { get; set; }

        public Guid TournamentId { get; set; }
        public TournamentEntity Tournament { get; set; }
        public ICollection<MatchEntity> Matches { get; set; }
    }

    public enum TournamentGroupTypes
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5,
    }
}
