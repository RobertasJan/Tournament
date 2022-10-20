using Tournament.Domain.Games;
using MatchType = Tournament.Domain.Games.MatchType;

namespace Tournament.Server.Models
{
    public class MatchModel
    {
        public Guid Id { get; set; }
        public int GamesToWin { get; set; } // 2
        public int PointsToWin { get; set; } // 21
        public int PointsToFinalize { get; set; } // 30
        public MatchType Type { get; set; }
        public MatchRecord Record { get; set; } = MatchRecord.ToBePlayed;
        public MatchResult Result { get; set; } = MatchResult.Undetermined;

        public Guid? TournamentGroupId { get; set; }


    }
}
