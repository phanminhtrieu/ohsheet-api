namespace CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate
{
    public class MusicSheetLike : EntityBase<int>
    {
        public int MusicSheetId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        private MusicSheetLike() { }

        public MusicSheetLike(MusicSheet sheet, Guid userId)
        {
            MusicSheetId = sheet.Id;
            UserId = userId;
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
