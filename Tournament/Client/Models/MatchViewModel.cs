namespace Tournament.Client.Models
{
    public class MatchViewModel
    {
        public uint Team1Score { get; set; }
        public uint Team2Score { get; set; }

        public string Player1 { get; set; } = "Player 1";
        public string Player2 { get; set; } = "Player 2";
        public string Player3 { get; set; } = "Player 3";
        public string Player4 { get; set; } = "Player 4";

        public ServeLocation ServeLocation { get; set; }
        private Stack<Point> _pointList { get; set; }

        public MatchViewModel()
        {
            _pointList = new Stack<Point>();
        }

        private void SetDefaultValues()
        {
            ServeLocation = ServeLocation.SW;
            Team1Score = 0;
            Team2Score = 0;
        }


        public void AddTeam1Score()
        {
            Team1Score++;
            this.ServeLocation = Team1Score % 2 == 0 ? ServeLocation.SW : ServeLocation.NW;
            if (_pointList.TryPeek(out Point point))
            {
                if (point.Scorer == Scorer.Team1)
                {
                    SwitchTeam1Players();
                }
            }
            _pointList.Push(new Point(this.ServeLocation, Scorer.Team1));
        }

        public void AddTeam2Score()
        {
            Team2Score++;
            this.ServeLocation = Team2Score % 2 == 0 ? ServeLocation.NE : ServeLocation.SE;
            if (_pointList.TryPeek(out Point point))
            {
                if (point.Scorer == Scorer.Team2)
                {
                    SwitchTeam2Players();
                }
            }
            _pointList.Push(new Point(this.ServeLocation, Scorer.Team2));
        }

        public void ReturnPoint()
        {
            var lastPoint = _pointList.Peek();
            if (lastPoint.Scorer == Scorer.Team1)
            {
                Team1Score--;
            }
            else
            {
                Team2Score--;
            }
            _pointList.Pop();
            if (_pointList.Count == 0)
            {
                SetDefaultValues();
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

        private class Point
        {
            public Point(ServeLocation serveLocation, Scorer scorer)
            {
                ServeLocation = serveLocation;
                Scorer = scorer;
            }
            public ServeLocation ServeLocation { get; }
            public Scorer Scorer { get; }
        }

        private enum Scorer
        {
            Team1,
            Team2
        }
    }
}

public enum ServeLocation
{
    SW,
    NW,
    NE,
    SE
}