using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate.Events;
using CleanArchitecture.Core.Domain.Enums;
using CleanArchitecture.Core.Domain.Interfaces;
using CleanArchitecture.Core.Exceptions.Specifics.MusicSheetExceptions;
using CleanArchitecture.Core.Helper.GaurdClause;
using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
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
            bool isForked,
            int status,
            int visibility,
            int viewCount,
            int likeCount,
            int commentCount,
            int shareCount)
        {
            Guard.AgainstNullOrEmpty<UserIdEmptyException>(userId);

            UserId = userId;
            Title = new MusicSheetTitle(title);
            Description = description;
            IsForked = isForked;
            Status = (MusicSheetStatus)status;
            MusicSheetVisibility = (MusicSheetVisibility)visibility;
            ViewCount = viewCount;
            LikeCount = likeCount;
            CommentCount = commentCount;
            ShareCount = shareCount;

            CreatedDate = DateTimeOffset.UtcNow;
            ModifiedDate = DateTimeOffset.UtcNow;
        }

        public static MusicSheet Create(
            Guid userId,
            int parentId,
            string title,
            string? description,
            bool isForked,
            int status = 0,
            int visibility = 0,
            int viewCount = 0,
            int likeCount = 0,
            int commentCount = 0,
            int shareCount = 0)
        {
            var sheet = new MusicSheet(
                userId, 
                parentId,
                title, 
                description, 
                isForked,
                status, 
                visibility,
                viewCount, 
                likeCount, 
                commentCount, 
                shareCount);

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

        public void SetFilePath(string filePath)
        {
            FilePath = filePath;
            ModifiedDate = DateTimeOffset.UtcNow;
        }

        public void SetFileSize(long fileSize)
        {
            FileSize = fileSize;
            ModifiedDate = DateTimeOffset.UtcNow;
        }

        public void AddComment(Guid userId, string content)
        {
            var comment = new MusicSheetComment(this, userId, content);
            _comments.Add(comment);
            CommentCount++;
            ModifiedDate = DateTimeOffset.UtcNow;
        }

        public void AddLike(Guid userId, string content)
        {
            var like = new MusicSheetLike(this, userId);
            _likes.Add(like);
            LikeCount++;
            ModifiedDate = DateTimeOffset.UtcNow;
        }

        public void AddTag(MusicSheetTag tag)
        {
            if (_tags.Any(t => t.Id == tag.Id))
            {
                return;
            }

            _tags.Add(tag);
            ModifiedDate = DateTimeOffset.UtcNow;
        }

        public void AddTags(IEnumerable<MusicSheetTag> tags)
        {
            foreach (var tag in tags)
            {
                _tags.Add(tag);
            }
            ModifiedDate = DateTimeOffset.UtcNow;
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
