using Tournament.Domain.Tournaments;
using MatchType = Tournament.Domain.Games.MatchType;

namespace Tournament.Shared.Tournaments
{
    public class TournamentGroupModel
    {
        public TournamentGroupTypes Type { get; set; }
        public string? OtherTypeName { get; set; }

        public MatchType MatchType { get; set; }
        public string? OtherMatchTypeName { get; set; }

        public override string? ToString()
        {
            return $"Match type: {MatchType}, Type; {Type}";
        }
    }
}
