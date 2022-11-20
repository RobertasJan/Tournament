using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Domain.Players
{
    public class TournamentPlayerModel
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public bool Singles { get; set; }
        public Guid? PartnerDoublesId { get; set; }
        public string? PartnerDoublesName { get; set; }
        public Guid? PartnerMixedId { get; set; }
        public string? PartnerMixedName { get; set; }
    }
}
