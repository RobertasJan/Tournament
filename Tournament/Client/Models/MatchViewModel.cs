using Tournament.Domain.Games;
using Tournament.Domain.Players;
using MatchType = Tournament.Domain.Games.MatchType;

namespace Tournament.Client.Models
{
    public class MatchViewModel
    {
        public string Player1 { get; set; } = "Player 1";
        public string Player2 { get; set; } = "Player 2";
        public string Player3 { get; set; } = "Player 3";
        public string Player4 { get; set; } = "Player 4";

        public int GamesToWin { get; set; } // 2
        public int PointsToWin { get; set; } // 21
        public int PointsToFinalize { get; set; } // 30
        public MatchType Type { get; set; }
        public MatchRecord Record { get; set; } = MatchRecord.ToBePlayed;
        public MatchResult Result { get; set; } = MatchResult.Undetermined;

        public ServeLocation ServeLocation { get; set; }
        private Stack<Point> _pointList { get; set; }
        private ICollection<GameViewModel> _gameList { get; set; }
        public GameViewModel CurrentGame { get; set; }

        public MatchViewModel()
        {
            _pointList = new Stack<Point>();
            CurrentGame = new GameViewModel();
            _gameList = new List<GameViewModel>() { CurrentGame };

        }

        public void AddTeam1Score()
        {
            CurrentGame.Team1Score++;
            this.ServeLocation = CurrentGame.Team1Score % 2 == 0 ? ServeLocation.SW : ServeLocation.NW;
            if (_pointList.TryPeek(out Point point))
            {
                if (point.Scorer == Team.Team1)
                {
                    SwitchTeam1Players();
                }
            }
            _pointList.Push(new Point(this.ServeLocation, Team.Team1));
        }

        public void AddTeam2Score()
        {
            CurrentGame.Team2Score++;
            this.ServeLocation = CurrentGame.Team2Score % 2 == 0 ? ServeLocation.NE : ServeLocation.SE;
            if (_pointList.TryPeek(out Point point))
            {
                if (point.Scorer == Team.Team2)
                {
                    SwitchTeam2Players();
                }
            }
            _pointList.Push(new Point(this.ServeLocation, Team.Team2));
        }

        public void ReturnPoint()
        {
            var lastPoint = _pointList.Peek();
            if (lastPoint.Scorer == Team.Team1)
            {
                CurrentGame.Team1Score--;
            }
            else
            {
                CurrentGame.Team2Score--;
            }
            _pointList.Pop();
            if (_pointList.Count == 0)
            {
                ServeLocation = ServeLocation.SW;
            }
            else
            {
                var previousPoint = _pointList.Peek();
                this.ServeLocation = previousPoint.ServeLocation;
            }
        }

        public void SwitchTeam1Players()
        {
            (Player2, Player1) = (Player1, Player2);
        }
        public void SwitchTeam2Players()
        {
            (Player4, Player3) = (Player3, Player4);
        }

        public void SwitchTeams()
        {
            (Player3, Player4, Player1, Player2) = (Player1, Player2, Player3, Player4);
        }

        public void ChangeServeSide()
        {
            if (this.ServeLocation == ServeLocation.SW)
            {
                this.ServeLocation = ServeLocation.NE;
            }
            else
            {
                this.ServeLocation = ServeLocation.SW;
            }
        }
    }
}