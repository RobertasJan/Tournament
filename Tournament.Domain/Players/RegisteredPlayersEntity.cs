using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Domain.Tournaments;

namespace Tournament.Domain.Players
{
    public class RegisteredPlayersEntity : BaseEntity
    {
        public Guid TournamentGroupId { get; set; }
        public TournamentGroupEntity TournamentGroup { get; set; }

        public Guid Player1Id { get; set; }
        public PlayerEntity Player1 { get; set; }

        public Guid? Player2Id { get; set; }
        public PlayerEntity? Player2 { get; set; }
        public int Rating { get; set; } = 0;
    }
}
