using System.Collections.Generic;

namespace CleanArchitecture.Core.Domain.Models.MusicSheet
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int MusicSheetId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserAvatar { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public int? ParentId { get; set; }
        public List<CommentModel> Replies { get; set; } = new();
    }
}
