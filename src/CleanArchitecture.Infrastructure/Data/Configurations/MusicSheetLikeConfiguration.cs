using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations
{
    public class MusicSheetLikeConfiguration : IEntityTypeConfiguration<MusicSheetLike>
    {
        public void Configure(EntityTypeBuilder<MusicSheetLike> builder)
        {
            builder.ToTable("MusicSheetLikes");
        }
    }
}
