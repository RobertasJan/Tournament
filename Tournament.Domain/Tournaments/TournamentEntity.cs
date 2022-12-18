namespace Tournament.Domain.Tournaments
{
    public class TournamentEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string Address { get; set; }
        public bool Public { get; set; }
        public bool Rated { get; set; }

        #region Match rules
        public int GamesToWin { get; set; } // 2
        public int PointsToWin { get; set; } // 21
        public int PointsToFinalize { get; set; } // 30
        #endregion

        public int CourtsAvailable { get; set; }
        public TimeSpan AverageTimePerMatch { get; set; }

        public TournamentState State { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid TournamentCreatorId { get; set; }
        public ICollection<TournamentGroupEntity> Groups { get; set; }
    }

    public enum TournamentState
    {
        Created = 0,
        Registration = 1,
        Draws = 2,
        Ongoing = 3,
        Finished = 4
    }
}
