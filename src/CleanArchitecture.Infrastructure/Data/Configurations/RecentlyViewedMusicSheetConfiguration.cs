using CleanArchitecture.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations
{
    public class RecentlyViewedMusicSheetConfiguration : IEntityTypeConfiguration<RecentlyViewedMusicSheet>
    {
        public void Configure(EntityTypeBuilder<RecentlyViewedMusicSheet> builder)
        {
            builder.ToTable("RecentlyViewedMusicSheets");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.UserId, x.MusicSheetId })
                .IsUnique();

            builder.Property(x => x.LastViewedAt)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.MusicSheet)
                .WithMany()
                .HasForeignKey(x => x.MusicSheetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
