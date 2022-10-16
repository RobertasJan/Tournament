using Tournament.Domain.Tournaments;

namespace Tournament.Shared.Tournaments
{
    public class TournamentModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }

        public bool Public { get; set; }
        public bool Rated { get; set; }

        public int GamesToWin { get; set; } // 2
        public int PointsToWin { get; set; } // 21
        public int PointsToFinalize { get; set; } // 30

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<TournamentGroupModel> Groups { get; set; }
    }
}
