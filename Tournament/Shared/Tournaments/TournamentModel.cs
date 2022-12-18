using Tournament.Domain.Tournaments;

namespace Tournament.Shared.Tournaments
{
    public class TournamentModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LongDescription { get; set; }
        public string? Address { get; set; }
        public bool Public { get; set; }
        public bool Rated { get; set; }
        public int GamesToWin { get; set; } // 2
        public int PointsToWin { get; set; } // 21
        public int PointsToFinalize { get; set; } // 30

        public TournamentState State { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CourtsAvailable { get; set; } = 1;
        public TimeSpan? AverageTimePerMatch { get; set; } = new TimeSpan(00, 30, 00);
        public Guid TournamentCreatorId { get; set; }
        public ICollection<TournamentGroupModel> Groups { get; set; }
            
        public IEnumerable<TournamentGroupTypes> GetGroupTypes()
            => this.Groups.GroupBy(x => new { x.Type }).Select(x => x.Key.Type).OrderBy(x => x);

        public IEnumerable<Domain.Games.MatchType> GetMatchTypes()
            => this.Groups.GroupBy(x => new { x.MatchType }).Select(x => x.Key.MatchType).OrderBy(x => x);
    }
}
