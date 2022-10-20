using Tournament.Domain.Games;
using Tournament.Domain.Players;
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
        public ICollection<MatchesGroupEntity> MatchesGroups { get; set; }
        public ICollection<RegisteredPlayersEntity> Registrations { get; set; }
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
