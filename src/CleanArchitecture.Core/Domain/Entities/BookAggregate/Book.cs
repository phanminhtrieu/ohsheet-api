using CleanArchitecture.Core.Domain.Entities.BookAggregate.Events;
using CleanArchitecture.Core.Domain.Enums;
using CleanArchitecture.Core.Exceptions.Specifics.BookExceptions;
using CleanArchitecture.Core.Helper.GaurdClause;
using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Core.Domain.Entities.BookAggregate
{
    /// <summary>
    /// In DDD, we dont have a public constructor for an Aggregate Root.
    /// Because we want to enforce the invariants of the Aggregate.
    /// </summary>
    public class Book : EntityBase<int>, IAggregate
    {
        public BookTitle Title { get; private set; }
        public BookAuthor Author { get; private set; }
        public BookStatus Status { get; private set; } = BookStatus.Publish;

        private Book() { } // EF use this for create Books table

        public Book(BookTitle title, BookAuthor author, BookStatus status = BookStatus.Publish)
        {
            Title = title;
            Author = author;
            Status = status;
        }

        public static Book Create(BookTitle title, BookAuthor author, BookStatus status = BookStatus.Publish)
        {
            var book = new Book(title, author, status);
            book.AddDomainEvent(new BookCreatedEvent(book));

            return book;
        }

        public void Update(BookTitle title, BookAuthor author, BookStatus status)
        {
            Title = title;
            Author = author;
            Status = status;

            AddDomainEvent(new BookUpdatedEvent(this));
        }
    }

    [Owned]
    public record BookTitle : IValueObject
    {
        // If dont do this, the column will be named "Title_Title
        [Column("Title")]
        public string Value { get; set; }

        private BookTitle() { }

        public BookTitle(string title)
        {
            // Implememt Guard Clause
            Guard.AgainstNullOrEmpty<BookTitleEmptyException>(title);
            Value = title;
        }
    }

    [Owned]
    public record BookAuthor : IValueObject 
    {
        [Column("Author")]
        public string Value { get; set; }

        private BookAuthor() { }

        public BookAuthor(string author)
        {
            Guard.AgainstNullOrEmpty<BookAuthorEmptyException>(author);
            Value = author;
        }
    }
}
