
using Tournament.Domain.Games;

namespace Tournament.Shared.Games
{
    public class UpdateMatchModel
    {
        public MatchRecord Record { get; set; }
        public MatchResult Result { get; set; }
    }
}
