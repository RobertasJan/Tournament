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

        public CourtLocation ServeLocation { get; set; }
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
                    Team1LeftSide = CurrentGame.Team1LeftSide;
                    var pointLast = _pointList?.LastOrDefault();
                    if (pointLast != null)
                    {
                        ServeLocation = pointLast.ServeLocation;
                    }
                    else
                    {
                        ServeLocation = CourtLocation.SW;
                    }
                    Player1 = matchModel.Team1?.Player1Name;
                    Player2 = matchModel.Team1?.Player2Name;
                    Player3 = matchModel.Team2?.Player1Name;
                    Player4 = matchModel.Team2?.Player2Name;

                    if (IsSingles)
                    {
                        SetServeLocationsForSingles();
                    }
                    else
                    {
                        if (CurrentGame.Team1Switched)
                        {
                            SwitchTeam1Players();
                        }
                        if (CurrentGame.Team2Switched)
                        {
                            SwitchTeam2Players();
                        }
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
                    CurrentGame = new GameViewModel()
                    {
                        Team1LeftSide = true,
                    };
                    _gameList = new List<GameViewModel>() { CurrentGame };
                }
            }
            this.service = service;

        }

        public async Task AddTeam1Score()
        {
            CurrentGame.Team1Score++;
            this.ServeLocation = CurrentGame.Team1Score % 2 == 0 ? CourtLocation.SW : CourtLocation.NW;
            if (IsSingles)
            {
                SetServeLocationsForSingles();
            }
            else
            {
                if (_pointList.TryPeek(out Point point))
                {
                    if (point.Scorer == Team.Team1)
                    {
                        SwitchTeam1Players();
                    }
                }
            }
            _pointList.Push(new Point(this.ServeLocation, Team.Team1));

            await CheckEndGame();
        }

        private void SetServeLocationsForSingles()
        {
            switch (this.ServeLocation)
            {
                case CourtLocation.SW:
                case CourtLocation.NE:
                    //if (Team1LeftSide)
                    {
                        if (!string.IsNullOrWhiteSpace(Player2))
                        {
                            Player1 = Player2;
                            Player2 = "";
                        }
                        if (!string.IsNullOrWhiteSpace(Player4))
                        {
                            Player3 = Player4;
                            Player4 = "";
                        }
                    }
                    break;
                case CourtLocation.NW:
                case CourtLocation.SE:
                    if (!string.IsNullOrWhiteSpace(Player1))
                    {
                        Player2 = Player1;
                        Player1 = "";
                    }
                    if (!string.IsNullOrWhiteSpace(Player3))
                    {
                        Player4 = Player3;
                        Player3 = "";
                    }
                    break;
            }
        }
        
        public async Task AddTeam2Score()
        {
            CurrentGame.Team2Score++;
            this.ServeLocation = CurrentGame.Team2Score % 2 == 0 ? CourtLocation.NE : CourtLocation.SE;
            if (IsSingles)
            {
                SetServeLocationsForSingles();
            }
            else
            {
                if (_pointList.TryPeek(out Point point))
                {
                    if (point.Scorer == Team.Team2)
                    {
                        SwitchTeam2Players();
                    }
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
                    Team2Score = CurrentGame.Team2Score,
                    Team1Switched = CurrentGame.Team1Switched,
                    Team2Switched = CurrentGame.Team2Switched,
                    Team1LeftSide = CurrentGame.Team1LeftSide,
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
                ServeLocation = CourtLocation.SW;
            }
            else
            {
                var previousPoint = _pointList.Peek();
                this.ServeLocation = previousPoint.ServeLocation;
            }
            if (IsSingles)
            {
                SetServeLocationsForSingles();
            }
            CurrentGame.Result = GameResult.Undetermined;
            await CheckEndGame();
        }

        public async Task DoEndGame()
        {
            CurrentGame.Result = CurrentGame.Team1Score > CurrentGame.Team2Score ? GameResult.Team1Victory : GameResult.Team2Victory;
            await SetMatchGame();
            _pointList = new Stack<Point>();
            CurrentGame = new GameViewModel()
            {
                Team1LeftSide = !CurrentGame.Team1LeftSide,
            };
            _gameList.Add(CurrentGame);
            EndGame = false;
        }

        public async Task DoEndMatch()
        {

        }

        public void SwitchTeam1Players()
        {
            
            (Player2, Player1) = (Player1, Player2);
          //  (Player2Id, Player1Id) = (Player1Id, Player2Id);
        }
        public void SwitchTeam2Players()
        {
            (Player4, Player3) = (Player3, Player4);
            //(Player4Id, Player3Id) = (Player3Id, Player4Id);
        }

        public void SwitchTeams()
        {
            Team1LeftSide = !Team1LeftSide;
            //(Player3, Player4, Player1, Player2) = (Player1, Player2, Player3, Player4);
            //(Player3Id, Player4Id, Player1Id, Player2Id) = (Player1Id, Player2Id, Player3Id, Player4Id);
        }

        public void ChangeServeSide()
        {
            if (this.ServeLocation == CourtLocation.SW)
            {
                this.ServeLocation = CourtLocation.NE;
            }
            else
            {
                this.ServeLocation = CourtLocation.SW;
            }
        }

        public bool IsSingles
            => Type == MatchType.MensSingles || Type == MatchType.WomensSingles;

    }
}