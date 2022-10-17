using Tournament.Domain.Players;
using MatchType = Tournament.Domain.Games.MatchType;

namespace Tournament.Shared.Players
{
    public class PlayerModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string? UserId { get; set; }

        //public IEnumerable<MatchType> GetGenderMatchTypes()
        //{
        //    return Gender == Gender.Male
        //        ? new MatchType[] { MatchType.MixedDoubles, MatchType.MensDoubles, MatchType.MensSingles }
        //        : new MatchType[] { MatchType.MixedDoubles, MatchType.WomensDoubles, MatchType.WomensSingles };
        //}
    }
}
