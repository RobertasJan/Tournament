namespace Tournament.Domain.Players
{
    public class PlayerEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<PlayerMatchEntity> PlayerMatches { get; set; }
    }
}
