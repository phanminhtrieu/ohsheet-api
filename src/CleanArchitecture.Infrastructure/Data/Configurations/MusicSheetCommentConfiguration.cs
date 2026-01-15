using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations
{
    public class MusicSheetCommentConfiguration : IEntityTypeConfiguration<MusicSheetComment>
    {
        public void Configure(EntityTypeBuilder<MusicSheetComment> builder)
        {
            builder.ToTable("MusicSheetComments");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Replies)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
