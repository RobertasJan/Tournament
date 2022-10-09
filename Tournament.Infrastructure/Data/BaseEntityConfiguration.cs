using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tournament.Domain;

namespace Tournament.Infrastructure.Data
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().HasDefaultValueSql("NEWID()");

            ConfigureEntity(builder);

            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Property(x => x.ModifiedAt).IsRequired().HasDefaultValueSql("SYSDATETIMEOFFSET()");
        }

        protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }
}
