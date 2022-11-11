using Tournament.Domain.Games;

namespace Tournament.Shared.Games
{
    public class GameModel
    {
        public Guid? Id { get; set; }
        public int Team1Score { get; set; } = 0;
        public int Team2Score { get; set; } = 0;
        public string? Scores { get; set; } // JSON Serialization of Point
        public GameResult? Result { get; set; } = GameResult.Undetermined;
        public Guid MatchId { get; set; }
        public bool Team1LeftSide { get; set; }
        public bool Team1Switched { get; set; }
        public bool Team2Switched { get; set; }

    }
}
