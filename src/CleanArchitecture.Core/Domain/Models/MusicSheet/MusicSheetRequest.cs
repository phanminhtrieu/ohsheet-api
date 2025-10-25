using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Core.Domain.Models.MusicSheet
{
    public class MusicSheetRequest
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public int ParentId { get; set; }
        public string? Description { get; set; }
        public string? FilePath { get; set; } // local or blob
        public long FileSize { get; set; }
        public int Status { get; set; } // Draft | Published | Deleted
        public int MusicSheetVisibility { get; set; } // Private | Public
        public IFormFile? MidiFile { get; set; }
        public int ViewCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public int ShareCount { get; set; } = 0;
        public bool IsForked { get; set; }
        public List<MusicSheetComment>? Comments { get; set; }
        public List<MusicSheetLike>? Likes { get; set; }
        public List<MusicSheetTag>? Tags { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
    }
}
