using CleanArchitecture.Core.Domain.Entities.SubscriptionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations
{
    public class AnonymousSubscriptionConfiguration : IEntityTypeConfiguration<AnonymousSubscription>
    {
        public void Configure(EntityTypeBuilder<AnonymousSubscription> builder)
        {
            builder.ToTable("AnonymousSubscriptions");
        }
    }
}
