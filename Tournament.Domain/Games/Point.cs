using Tournament.Domain.Players;

namespace Tournament.Domain.Games
{
    public enum ServeLocation
    {
        SW,
        NW,
        NE,
        SE
    }

    public class Point
    {
        public Point(ServeLocation serveLocation, Team scorer)
        {
            ServeLocation = serveLocation;
            Scorer = scorer;
        }
        public ServeLocation ServeLocation { get; }
        public Team Scorer { get; }
    }
}
