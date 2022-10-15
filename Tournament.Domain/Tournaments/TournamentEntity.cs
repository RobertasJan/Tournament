using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Domain.Tournaments
{
    public class TournamentEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }

        public bool Public { get; set; }

        #region Match rules
        public int GamesToWin { get; set; } // 2
        public int PointsToWin { get; set; } // 21
        public int PointsToFinalize { get; set; } // 30
        #endregion

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public ICollection<TournamentGroupEntity> Groups { get; set; }
    }
}
