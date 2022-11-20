using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Players;
using Tournament.Domain.Tournaments;

namespace Tournament.Domain.Results
{
    public class ResultEntity : BaseEntity
    {
        public Guid TournamentGroupId { get; set; }
        public TournamentGroupEntity TournamentGroup { get; set; }

        public Guid PlayerId { get; set; }
        public PlayerEntity Player { get; set; }

        public int Position { get; set; }
        public int RatingPoints { get; set; }
    }
}
