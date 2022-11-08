namespace Tournament.Domain
{
    public class BaseEntity
    {
        public Guid? Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
