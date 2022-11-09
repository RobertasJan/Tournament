using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Games;
using Tournament.Domain.Tournaments;
using Tournament.Server.Models;

namespace Tournament.Shared.Games
{
    public class MatchesGroupModel
    {
        public int Round { get; set; } // final - 0, semifinal - 1, quarter - 2, r16 - 3, r32 - 4, r64 - 5
        public int GroupName { get; set; } // alphabetical 1 - a, 2 - b, 3 - c and so on

        public RoundType RoundType { get; set; }

        public Guid TournamentGroupId { get; set; }
        public ICollection<MatchModel>? Matches { get; set; }
        public MatchesGroupModel? WinnersGroup { get; set; }
        public MatchesGroupModel? LosersGroup { get; set; }
    }
}
