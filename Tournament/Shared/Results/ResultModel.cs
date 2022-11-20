using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Players;
using Tournament.Domain.Tournaments;

namespace Tournament.Shared.Results
{
    public class ResultModel
    {
        public Guid TournamentGroupId { get; set; }
        public string TournamentName { get; set; }
        public Guid TournamentId { get; set; }
        public Guid PlayerId { get; set; }
        public int Position { get; set; }
        public int RatingPoints { get; set; }
    }
}
