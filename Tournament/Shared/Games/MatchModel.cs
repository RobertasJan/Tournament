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

        public MatchTeamModel? Team1 { get; set; }
        public MatchTeamModel? Team2 { get; set; }

        public int? GroupPosition { get; set; }
    }

    public class MatchTeamModel
    {
        public Guid? Player1Id { get; set; }
        public string Player1Name { get; set; }
        public Guid? Player2Id { get; set; }
        public string? Player2Name { get; set; }
        public int Seed { get; set; }
        public int? Rating { get; set; }
    }
}
