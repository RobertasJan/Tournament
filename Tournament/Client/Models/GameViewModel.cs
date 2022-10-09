using Tournament.Domain.Games;

namespace Tournament.Client.Models
{
    public class GameViewModel
    {
        public uint Team1Score { get; set; }
        public uint Team2Score { get; set; }
        public ServeLocation ServeLocation { get; set; }
    }
}
