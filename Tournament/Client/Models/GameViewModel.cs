using Tournament.Domain.Games;

namespace Tournament.Client.Models
{
    public class GameViewModel
    {
        public Guid? Id { get; set; }
        public uint Team1Score { get; set; }
        public uint Team2Score { get; set; }
        public ServeLocation ServeLocation { get; set; }
        public GameResult Result { get; set; }
    }
}
