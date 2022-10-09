using Tournament.Domain.Players;

namespace Tournament.Domain.Games
{
    public class MatchEntity
    {
        public int Id { get; set; }
        public int GamesToWin { get; set; } // 2
        public int PointsToWin { get; set; } // 21
        public int PointsToFinalize { get; set; } // 30
        public MatchType Type { get; set; }
        public MatchRecord Record { get; set; } = MatchRecord.ToBePlayed;
        public MatchResult Result { get; set; } = MatchResult.Undetermined;
        public IEnumerable<GameEntity>? Games { get; set; }
        public IEnumerable<PlayerMatchEntity>? PlayersMatches { get; set; }
    }

    public enum MatchType : byte
    {
        MensSingles = 1,
        WomensSingles = 2,
        MensDoubles = 3,
        WomensDoubles = 4,
        MixedDoubles = 5,

        Other = 6
    }

    public enum MatchRecord : byte
    {
        Undetermined = 0,
        ToBePlayed = 1,

        Played = 2,
        Disqualified = 3,
        Walkover = 4,
        Injury = 5
    }

    public enum MatchResult : byte
    {
        Team1Victory = 1,
        Team2Victory = 2,

        Undetermined = 0
    }
}
