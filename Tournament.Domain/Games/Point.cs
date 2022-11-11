using Tournament.Domain.Players;

namespace Tournament.Domain.Games
{
    public enum CourtLocation
    {
        SW = 1,
        NW = 2,
        NE = 3,
        SE = 4
    }

    public class Point
    {
        public Point(CourtLocation serveLocation, Team scorer)
        {
            ServeLocation = serveLocation;
            Scorer = scorer;
        }
        public CourtLocation ServeLocation { get; }
        public Team Scorer { get; }
    }
}
