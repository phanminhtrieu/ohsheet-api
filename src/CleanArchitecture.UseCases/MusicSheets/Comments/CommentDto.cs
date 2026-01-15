using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;

namespace CleanArchitecture.UseCases.MusicSheets.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int MusicSheetId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int? ParentId { get; set; }
        public List<CommentDto> Replies { get; set; } = new();

        public static CommentDto FromEntity(MusicSheetComment comment, string userName, string userAvatar)
        {
            return new CommentDto
            {
                Id = comment.Id,
                MusicSheetId = comment.MusicSheetId,
                UserId = comment.UserId,
                UserName = userName,
                UserAvatar = userAvatar,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                ParentId = comment.ParentId,
                Replies = comment.Replies?.Select(r => FromEntity(r, "Unknown", "")).ToList() ?? new List<CommentDto>() // Note: UserName/Avatar for replies needs handling
            };
        }
    }
}
