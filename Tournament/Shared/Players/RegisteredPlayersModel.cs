using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Shared.Tournaments;

namespace Tournament.Shared.Players
{
    public class RegisteredPlayersModel
    {
        public Guid Player1Id { get; set; }
        public string? Player1Name { get; set; }
        public Guid? Player2Id { get; set; }
        public string? Player2Name { get; set; }
        public TournamentGroupModel TournamentGroup { get; set; }
        public int? Rating { get; set; }

    }
}
