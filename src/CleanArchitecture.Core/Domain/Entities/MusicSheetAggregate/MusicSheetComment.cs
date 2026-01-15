namespace CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate
{
    public class MusicSheetComment : EntityBase<int>
    {
        public int MusicSheetId { get; private set; }
        public Guid UserId { get; private set; }
        public string Content { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public int? ParentId { get; private set; }
        public MusicSheetComment? Parent { get; private set; }
        private readonly List<MusicSheetComment> _replies = new();
        public IReadOnlyCollection<MusicSheetComment> Replies => _replies.AsReadOnly();

        private MusicSheetComment() { }

        public MusicSheetComment(MusicSheet sheet, Guid userId, string content, int? parentId = null)
        {
            MusicSheetId = sheet.Id;
            UserId = userId;
            Content = content;
            ParentId = parentId;
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
