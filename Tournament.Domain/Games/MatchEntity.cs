namespace Tournament.Domain.Games
{
    public class MatchEntity
    {

    }

    public enum MatchType : byte
    {
        MensSingles = 1,
        WomensSingles = 2,
        MensDoubles = 3,
        WomensDoubles = 4,
        MixedDoubles = 5,

        Other = 6
    }

    public enum MatchRecord : byte
    {
        Undetermined = 0,
        ToBePlayed = 1,

        Played = 2,
        Disqualified = 3,
        Walkover = 4,
        Injury = 5
    }

    public enum MatchResult : byte
    {
        Team1Victory = 1,
        Team2Victory = 2,

        Undetermined = 0
    }
}
