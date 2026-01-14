using CleanArchitecture.Core.Domain.Models.MusicSheet;

namespace CleanArchitecture.Core.Domain.Models.Profile
{
    public class RecentlyViewedMusicSheetDto : MusicSheetResponse
    {
        public DateTime LastViewedAt { get; set; }
    }
}
