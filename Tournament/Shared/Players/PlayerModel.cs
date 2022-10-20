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

        public IEnumerable<MatchType> GetGenderMatchTypes(IEnumerable<MatchType> matchTypes)
        {
            var matchTypesGendered = Gender == Gender.Male
                ? new MatchType[] { MatchType.MixedDoubles, MatchType.MensDoubles, MatchType.MensSingles }.OrderBy(x => x)
                : new MatchType[] { MatchType.MixedDoubles, MatchType.WomensDoubles, MatchType.WomensSingles }.OrderBy(x => x);
            return matchTypesGendered.ToList().Where(x => matchTypes.Contains(x));
        }

        public Gender GetOppositeGender()
            => Gender == Gender.Male ? Gender.Female : Gender.Male;
    }
}
