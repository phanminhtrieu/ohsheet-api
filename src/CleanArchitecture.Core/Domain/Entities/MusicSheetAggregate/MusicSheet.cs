using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate.Events;
using CleanArchitecture.Core.Domain.Enums;
using CleanArchitecture.Core.Domain.Interfaces;
using CleanArchitecture.Core.Exceptions.Specifics.MusicSheetExceptions;
using CleanArchitecture.Core.Helper.GaurdClause;
using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate
{
    public class MusicSheet : EntityBase<int>, IAggregateRoot, IHasDateTracking
    {
        // Properties
        public Guid UserId { get; private set; }
        public MusicSheetTitle Title { get; private set; }
        public int ParentId { get; private set; }
        public string? Description { get; private set; }
        public string? FilePath { get; private set; } // local or blob
        public long FileSize { get; private set; }
        public MusicSheetStatus Status { get; private set; } // Draft | Published | Deleted
        public MusicSheetVisibility MusicSheetVisibility { get; private set; } // Private | Public
        public MidiBinaryData? MidiData { get; private set; }
        public int ViewCount { get; private set; } = 0;
        public int LikeCount { get; private set; } = 0;
        public int CommentCount { get; private set; } = 0;
        public int ShareCount { get; private set; } = 0;
        public bool IsForked { get; private set; }

        // Navigation 1 - N with MusicSheetComment
        private readonly List<MusicSheetComment> _comments = new();
        public IReadOnlyCollection<MusicSheetComment> Comments => _comments.AsReadOnly();

        // Navigation 1 - N with MusicSheetLike
        private readonly List<MusicSheetLike> _likes = new();
        public IReadOnlyCollection<MusicSheetLike> Likes => _likes.AsReadOnly();

        // Navigation N - N with MusicSheetTag (there is a list MusicSheet on a MusicTag)
        private readonly List<MusicSheetTag> _tags = new();
        public IReadOnlyCollection<MusicSheetTag> Tags => _tags.AsReadOnly();

        public DateTimeOffset CreatedDate { get; private set; }

        public DateTimeOffset ModifiedDate { get; private set; }

        private MusicSheet() { }

        private MusicSheet(
            Guid userId,
            int parentId,
            string title, 
            string? description, 
            string? filePath, 
            long fileSize, 
            int status,
            int viewCount,
            int likeCount,
            int commentCount,
            int shareCount,
            bool isForked)
        {
            Guard.AgainstNullOrEmpty<UserIdEmptyException>(userId);

            UserId = userId;
            Title = new MusicSheetTitle(title);
            Description = description;
            FilePath = filePath;
            FileSize = fileSize;
            Status = (MusicSheetStatus)status;
            ViewCount = viewCount;
            LikeCount = likeCount;
            CommentCount = commentCount;
            ShareCount = shareCount;
            IsForked = isForked;

            CreatedDate = DateTimeOffset.UtcNow;
            ModifiedDate = DateTimeOffset.UtcNow;
        }

        public static MusicSheet Create(
            Guid userId,
            int parentId,
            string title,
            string? description,
            string? filePath,
            long fileSize,
            int status,
            int viewCount,
            int likeCount,
            int commentCount,
            int shareCount,
            bool isForked)
        {
            var sheet = new MusicSheet(
                userId, 
                parentId,
                title, 
                description, 
                filePath, 
                fileSize, 
                status, 
                viewCount, 
                likeCount, 
                commentCount, 
                shareCount, 
                isForked);

            sheet.AddDomainEvent(new MusicSheetCreatedEvent(sheet));

            return sheet;
        }

        public void UpdateMetadata(string title, string description)
        {
            Title = new MusicSheetTitle(title);
            Description = description;
            ModifiedDate = DateTimeOffset.UtcNow;
        }

        public void AttachBinary(byte[] data)
        {
            MidiData = new MidiBinaryData(data);
            FileSize = data.Length;
            ModifiedDate = DateTimeOffset.UtcNow;
        }

        public void AddComment(Guid userId, string content)
        {
            var comment = new MusicSheetComment(this, userId, content);
            _comments.Add(comment);
        }

        public void AddLike(Guid userId, string content)
        {
            var like = new MusicSheetLike(this, userId);
            _likes.Add(like);
        }
    }

    [Owned]
    public record MusicSheetTitle : IValueObject
    {
        [Column("Title")]
        public string Value { get; private set; }

        private MusicSheetTitle() { }

        public MusicSheetTitle(string title)
        {
            Guard.AgainstNullOrEmpty<MusicSheetTitleEmptyException>(title);
            Value = title;
        }
    }

    [Owned]
    public record MidiBinaryData : IValueObject
    {
        [Column("MidiData")]
        public byte[] Value { get; private set; }

        private MidiBinaryData() { }

        public MidiBinaryData(byte[] data)
        {
            Guard.AgainstNullOrEmpty<MidiBinaryDataEmptyException>(data);
            Value = data;
        }
    }
}
