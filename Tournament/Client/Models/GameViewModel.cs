using System.Drawing;

namespace Tournament.Client.Models
{
    public class GameViewModel
    {
        public uint Team1Score { get; set; }
        public uint Team2Score { get; set; }
        public ServeLocation ServeLocation { get; set; }
        private Stack<Point> _pointList { get; set; }

        public GameViewModel()
        {
            _pointList = new Stack<Point>();
        }
    }
}
