namespace CleanArchitecture.Core.Domain.Entities
{
    public class Notification : EntityBase<int>
    {
        public Guid UserId { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public bool IsRead { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Type { get; private set; } = string.Empty;
        public string? RelatedId { get; private set; }

        private Notification() { } // For EF Core

        public Notification(Guid userId, string message, string type, string? relatedId = null)
        {
            UserId = userId;
            Message = message;
            Type = type;
            RelatedId = relatedId;
            IsRead = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}
