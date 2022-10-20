using Tournament.Domain.Games;
using Tournament.Shared.Games;

namespace Tournament.Client.Models
{
    public class GameViewModel
    {
        public Guid? Id { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public GameResult Result { get; set; }

        public static GameViewModel FromGameModel(GameModel model)
        {
            return new GameViewModel()
            {
                Id = model.Id,
                Result = model.Result.Value,
                Team1Score = model.Team1Score,
                Team2Score = model.Team2Score
            };
        }
    }
}
