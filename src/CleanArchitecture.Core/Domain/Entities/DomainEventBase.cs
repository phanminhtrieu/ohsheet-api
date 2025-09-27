using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Core.Domain.Entities
{
    /// <summary>
    /// We dont wanna this class to be mapped to a table in the database.
    /// </summary>
    [NotMapped]
    public class DomainEventBase : INotification
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    }
}
