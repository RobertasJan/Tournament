using Tournament.Domain.Games;
using Tournament.Domain.Players;
using Tournament.Shared.Games;
using MatchType = Tournament.Domain.Games.MatchType;

namespace Tournament.Server.Models
{
    public class MatchModel
    {
        public Guid Id { get; set; }
        public int GamesToWin { get; set; } // 2
        public int PointsToWin { get; set; } // 21
        public int PointsToFinalize { get; set; } // 30
        public MatchType Type { get; set; }
        public MatchRecord Record { get; set; } = MatchRecord.ToBePlayed;
        public MatchResult Result { get; set; } = MatchResult.Undetermined;

        public Guid? TournamentGroupId { get; set; }

        private MatchTeamModel? _team1;
        public MatchTeamModel? Team1
        {
            get
            {
                if (_team1 is null)
                {
                    var players = PlayersMatches?.Where(x => x.Team == Team.Team1);
                    if (players?.Count() > 0)
                    {
                        var player1 = players.ElementAtOrDefault(0);
                        var player2 = players.ElementAtOrDefault(1);

                        _team1 = new MatchTeamModel()
                        {
                            Player1Id = player1?.PlayerId,
                            Player1Name = player1?.Player?.FullName,
                            Player2Id = player2?.PlayerId,
                            Player2Name = player2?.Player?.FullName,
                        };
                    }
                }
                return _team1;
            }
            set
            {
                _team1 = value;
            }
        }

        private MatchTeamModel? _team2;
        public MatchTeamModel? Team2
        {
            get
            {
                if (_team2 is null)
                {
                    var players = PlayersMatches?.Where(x => x.Team == Team.Team2);
                    if (players?.Count() > 0)
                    {
                        var player1 = players.ElementAtOrDefault(0);
                        var player2 = players.ElementAtOrDefault(1);

                        _team2 = new MatchTeamModel()
                        {
                            Player1Id = player1?.PlayerId,
                            Player1Name = player1?.Player?.FullName,
                            Player2Id = player2?.PlayerId,
                            Player2Name = player2?.Player?.FullName,
                        };
                    }
                }
                return _team2;
            }
            set
            {
                _team2 = value;
            }
        }
        public MatchesGroupModel? MatchesGroup { get; set; }
        public ICollection<GameModel>? Games { get; set; }
        public Guid? MatchesGroupId { get; set; }

        public int? GroupPosition { get; set; }
        public int? GroupName { get; set; }
        public int Round { get; set; }
        public DateTime? MatchDate { get; set; }

        // from entity
        public ICollection<PlayerMatchEntity>? PlayersMatches { get; set; }
    }

    public class MatchTeamModel
    {
        public Guid? Player1Id { get; set; }
        public string? Player1Name { get; set; }
        public Guid? Player2Id { get; set; }
        public string? Player2Name { get; set; }
        public int Seed { get; set; }
        public int? Rating { get; set; }
    }
}
