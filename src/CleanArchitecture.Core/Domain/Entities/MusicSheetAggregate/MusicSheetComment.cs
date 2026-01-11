namespace CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate
{
    public class MusicSheetComment : EntityBase<int>
    {
        public int MusicSheetId { get; private set; }
        public Guid UserId { get; private set; }
        public string Content { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        private MusicSheetComment() { }

        public MusicSheetComment(MusicSheet sheet, Guid userId, string content)
        {
            MusicSheetId = sheet.Id;
            UserId = userId;
            Content = content;
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
