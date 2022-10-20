using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Domain.Tournaments
{
    public class TournamentRoundEntity : BaseEntity
    {
        // for tree: 1 - finals, 2 - semifinals, 3 - quarter, 4 - r16, 5 - r32,  6 - r64
        public int Depth { get; set; }

        public int TournamentGroupId { get; set; }
        public TournamentGroupEntity TournamentGroup { get; set; }

        public RoundType Type { get; set; }
    }

    public enum RoundType
    {
        Tree
    }

    public enum Round
    {

    }
}
