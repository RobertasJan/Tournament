namespace Tournament.Domain.Games
{
    public class GameEntity : BaseEntity
    {
        public int Team1Score { get; set; } = 0;
        public int Team2Score { get; set; } = 0;

        public string Scores { get; set; } // JSON Serialization of Point

        public GameResult? Result { get; set; } = GameResult.Undetermined;

        public MatchEntity? Match { get; set; }
        public Guid MatchId { get; set; }
        public bool Team1LeftSide { get; set; }
        public bool Team1Switched { get; set; }
        public bool Team2Switched { get; set; }
    }
    public enum GameResult : byte
    {
        Team1Victory = 1,
        Team2Victory = 2,

        Undetermined = 0
    }
}
