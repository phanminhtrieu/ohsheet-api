using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Domain.Enums;

namespace CleanArchitecture.Core.Domain.Models.MusicSheet
{
    public class MusicSheetResponse
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get;  set; }
        public string UploaderName { get; set; } = string.Empty;
        public string UploaderAvatar { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public string? Description { get; set; }
        public string? TranscriptionId { get; set; }
        public string? Thumbnail { get; set; }
        public MusicSheetStatus Status { get; set; } // Draft | Published | Deleted
        public MusicSheetVisibility MusicSheetVisibility { get; set; } // Private | Public
        public MidiBinaryData? MidiData { get;  set; }
        public int ViewCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public int ShareCount { get; set; } = 0;
        public bool IsForked { get; set; }
        public IReadOnlyCollection<MusicSheetComment>? Comments { get; set; }
        public IReadOnlyCollection<MusicSheetLike>? Likes { get; set; }
        public IReadOnlyCollection<string>? Tags { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public MusicSheetUIState MusicSheetUIState { get; set; }
    }
}
