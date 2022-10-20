using System.Text.Json;
using Tournament.Client.Services;
using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Server.Models;
using Tournament.Shared.Games;
using MatchType = Tournament.Domain.Games.MatchType;

namespace Tournament.Client.Models
{
    public class MatchViewModel
    {
        public Guid MatchId { get; set; } = Guid.Empty;

        public string Player1 { get; set; } = "Player 1";
        public string Player2 { get; set; } = "Player 2";
        public string Player3 { get; set; } = "Player 3";
        public string Player4 { get; set; } = "Player 4";

        public bool Team1LeftSide { get; set; } = true;
        public bool EndGame { get; set; } = false;
        public bool EndMatch { get; set; } = false;

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

        private readonly GameService service;

        public MatchViewModel(GameService service, MatchModel matchModel, ICollection<GameModel> games)
        {
            if (matchModel != null)
            {
                this.GamesToWin = matchModel.GamesToWin;
                this.PointsToFinalize = matchModel.PointsToFinalize;
                this.PointsToWin = matchModel.PointsToWin;
                this.Type = matchModel.Type;
                this.Record = matchModel.Record;
                this.Result = matchModel.Result;
                this.MatchId = matchModel.Id;
                if (games.Count > 0)
                {
                    var lastGame = games.Last();
                    _pointList = lastGame.Scores != null ? JsonSerializer.Deserialize<Stack<Point>>(games.Last().Scores) : new Stack<Point>();
                    CurrentGame = GameViewModel.FromGameModel(lastGame);
                    ServeLocation = ServeLocation.SW;
                    var pointLast = _pointList?.LastOrDefault();
                    if (pointLast != null)
                    {
                        ServeLocation = pointLast.ServeLocation;
                    }
          
                    _gameList = new List<GameViewModel>();
                    foreach (var game in games)
                    {
                        _gameList.Add(GameViewModel.FromGameModel(game));
                    }
                }
                else
                {
                    _pointList = new Stack<Point>();
                    CurrentGame = new GameViewModel();
                    _gameList = new List<GameViewModel>() { CurrentGame };
                }
            }
            this.service = service;

        }

        public async Task AddTeam1Score()
        {
            CurrentGame.Team1Score++;
            this.ServeLocation = CurrentGame.Team1Score % 2 == 0 ? ServeLocation.SW : ServeLocation.NW;
            if (_pointList.TryPeek(out Point point))
            {
                if (point.Scorer == Team.Team1)
                {
                    SwitchLeftsidePlayers();
                }
            }
            _pointList.Push(new Point(this.ServeLocation, Team.Team1));

            await CheckEndGame();
        }

        public async Task AddTeam2Score()
        {
            CurrentGame.Team2Score++;
            this.ServeLocation = CurrentGame.Team2Score % 2 == 0 ? ServeLocation.NE : ServeLocation.SE;
            if (_pointList.TryPeek(out Point point))
            {
                if (point.Scorer == Team.Team2)
                {
                    SwitchRightsidePlayers();
                }
            }
            _pointList.Push(new Point(this.ServeLocation, Team.Team2));

            await CheckEndGame();
        }

        private async Task SetMatchGame()
        {
            if (CurrentGame.Id != null)
            {
                await service.UpdateGame(new GameModel()
                {
                    Id = CurrentGame.Id,
                    Result = CurrentGame.Result,
                    Scores = JsonSerializer.Serialize(_pointList),
                    Team1Score = CurrentGame.Team1Score,
                    Team2Score = CurrentGame.Team2Score
                }, MatchId);
            }
            else
            {
                CurrentGame.Id = await service.CreateGame(new GameModel(), MatchId);
            }
        }

        private async Task CheckEndGame()
        {
            if (CurrentGame.Team2Score == PointsToFinalize || (CurrentGame.Team2Score >= PointsToWin && CurrentGame.Team2Score > CurrentGame.Team1Score + 1))
            {
                this.CurrentGame.Result = GameResult.Team2Victory;
                EndGame = true;
            }
            else if (CurrentGame.Team1Score == PointsToFinalize || (CurrentGame.Team1Score >= PointsToWin && CurrentGame.Team1Score > CurrentGame.Team2Score + 1))
            {
                EndGame = true;
                this.CurrentGame.Result = GameResult.Team1Victory;
            }
            else
            {
                this.CurrentGame.Result = GameResult.Undetermined;
                EndGame = false;
                EndMatch = false;
            }
            await SetMatchGame();
        }

        public async Task ReturnPoint()
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
            await CheckEndGame();
        }

        public void SwitchLeftsidePlayers()
        {
            (Player2, Player1) = (Player1, Player2);
        }
        public void SwitchRightsidePlayers()
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