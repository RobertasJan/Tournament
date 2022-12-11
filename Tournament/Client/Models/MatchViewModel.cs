using Microsoft.AspNetCore.SignalR.Client;
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
        public string Player1 { get; set; } = "Player 1";
        public string Player2 { get; set; } = "Player 2";
        public string Player3 { get; set; } = "Player 3";
        public string Player4 { get; set; } = "Player 4";
        public bool Request { get; set; } = false;

        public bool Team1LeftSide { get; set; } = true;
        public bool EndGame { get; set; } = false;
        public bool EndMatch { get; set; } = false;
        public bool FirstGame { get; set; } = false;

        MatchModel? Data { get; set; } 

        public CourtLocation ServeLocation { get; set; }
        private Stack<Point> _pointList { get; set; }
        public ICollection<GameModel> GameList { get; set; }
        public GameModel CurrentGame { get; set; }

        private readonly GameService service;
        private readonly HubConnection connection;

        public MatchViewModel(GameService service, MatchModel matchModel, ICollection<GameModel> games, HubConnection connection)
        {
            this.connection = connection;
            this.connection.StartAsync();
            if (matchModel != null)
            {
                Data = matchModel;

                this.GameList = games;
                if (GameList.Count > 0)
                {
                    var lastGame = GameList.Last();
                    _pointList = lastGame.Scores != null ? JsonSerializer.Deserialize<Stack<Point>>(GameList.Last().Scores) : new Stack<Point>();
                    CurrentGame = lastGame;
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
                        SetPlayerLocationsForSingles();
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
                        
                    if (GameList.Count == 1)
                    {
                        FirstGame = true;
                    }
                    DoCheckEndGame();
                }
                else
                {
                    _pointList = new Stack<Point>();
                    CurrentGame = new GameModel()
                    {
                        Team1LeftSide = true,
                        MatchId = matchModel.Id,
                    };
                    Player1 = matchModel.Team1?.Player1Name;
                    Player2 = matchModel.Team1?.Player2Name;
                    Player3 = matchModel.Team2?.Player1Name;
                    Player4 = matchModel.Team2?.Player2Name;
                    ServeLocation = CourtLocation.SW;
                    GameList = new List<GameModel>() { CurrentGame };
                    FirstGame = true;
                }
            }
            this.service = service;

        }

        public async Task AddTeam1Score()
        {
            CurrentGame.Team1Score++;
            if (Team1LeftSide)
            {
                this.ServeLocation = CurrentGame.Team1Score % 2 == 0 ? CourtLocation.SW : CourtLocation.NW;
            }
            else
            {
                this.ServeLocation = CurrentGame.Team1Score % 2 == 0 ? CourtLocation.NE : CourtLocation.SE;
            }
            if (IsSingles)
            {
                SetPlayerLocationsForSingles();
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

        private void SetPlayerLocationsForSingles()
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
            if (Team1LeftSide)
            {
                this.ServeLocation = CurrentGame.Team2Score % 2 == 0 ? CourtLocation.NE : CourtLocation.SE;
            } else
            {
                this.ServeLocation = CurrentGame.Team2Score % 2 == 0 ? CourtLocation.SW : CourtLocation.NW;
            }
            if (IsSingles)
            {
                SetPlayerLocationsForSingles();
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
                Request = true;
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
                }, Data.Id);
                Request = false;
            }
            else
            {
                Request = true;
                CurrentGame.Id = await service.CreateGame(new GameModel(), Data.Id);
                Request = false;
            }
            await connection.SendAsync("UpdateMatchScore", GameList.ToList());
        }

        private async Task CheckEndGame()
        {
            DoCheckEndGame();
            await SetMatchGame();
        }

        private void DoCheckEndGame()
        {
            if (CurrentGame.Team2Score == Data.PointsToFinalize || (CurrentGame.Team2Score >= Data.PointsToWin && CurrentGame.Team2Score > CurrentGame.Team1Score + 1))
            {
                this.CurrentGame.Result = GameResult.Team2Victory;
                EndGame = true;
            }
            else if (CurrentGame.Team1Score == Data.PointsToFinalize || (CurrentGame.Team1Score >= Data.PointsToWin && CurrentGame.Team1Score > CurrentGame.Team2Score + 1))
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
                SetPlayerLocationsForSingles();
            }
            CurrentGame.Result = GameResult.Undetermined;
            await CheckEndGame();
        }

        public async Task DoEndGame()
        {
            var previousResult = CurrentGame.Team1Score > CurrentGame.Team2Score ? GameResult.Team1Victory : GameResult.Team2Victory;
            CurrentGame.Result = previousResult;
            await SetMatchGame();
            if (GameList.Where(x => x.Result == GameResult.Team1Victory).Count() == Data.GamesToWin || GameList.Where(x => x.Result == GameResult.Team2Victory).Count() == Data.GamesToWin)
            {
                EndMatch = true;
            }
            else {
                _pointList = new Stack<Point>();
                CurrentGame = new GameModel()
                {
                    Team1LeftSide = !Team1LeftSide,
                };
                if ((Team1LeftSide && previousResult == GameResult.Team1Victory) || (!Team1LeftSide && previousResult == GameResult.Team2Victory))
                {
                    ServeLocation = CourtLocation.NE;
                }
                else
                {
                    ServeLocation = CourtLocation.SW;
                }
                Team1LeftSide = !Team1LeftSide;

                GameList.Add(CurrentGame);
                EndGame = false;
                FirstGame = false;
                await SetMatchGame();
            }
        }

        public async Task DoEndMatch()
        {
            Data.Record = MatchRecord.Played;
            Data.Result = GameList.Last().Result == GameResult.Team1Victory ? MatchResult.Team1Victory : MatchResult.Team2Victory; 
            await service.UpdateMatch(Data);
            await service.SetNextMatch(Data.Id);
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
            Team1LeftSide = !Team1LeftSide;
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
            => Data.Type == MatchType.MensSingles || Data.Type == MatchType.WomensSingles;

    }
}