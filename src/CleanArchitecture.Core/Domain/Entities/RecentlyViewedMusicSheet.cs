using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Core.Domain.Entities
{
    public class RecentlyViewedMusicSheet : EntityBase<int>
    {
        public Guid UserId { get; private set; }
        public int MusicSheetId { get; private set; }
        public DateTime LastViewedAt { get; private set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; private set; }

        [ForeignKey(nameof(MusicSheetId))]
        public virtual MusicSheet MusicSheet { get; private set; }

        private RecentlyViewedMusicSheet() { }

        public RecentlyViewedMusicSheet(Guid userId, int musicSheetId)
        {
            UserId = userId;
            MusicSheetId = musicSheetId;
            LastViewedAt = DateTime.UtcNow;
        }

        public void UpdateLastViewedAt()
        {
            LastViewedAt = DateTime.UtcNow;
        }
    }
}
