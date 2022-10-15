using Tournament.Domain.Games;
using Tournament.Domain.Tournaments;
using MatchType = Tournament.Domain.Games.MatchType;

namespace Tournament.Client.Models
{
    public class TournamentCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }

        public int GamesToWin { get; set; } // 2
        public int PointsToWin { get; set; } // 21

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public IEnumerable<TournamentGroupTypes> Groups { get; set; }
        public IEnumerable<MatchType> MatchTypes { get; set; }
    }
}
