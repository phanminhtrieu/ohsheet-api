using MediatR;

namespace CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate.Events
{
    internal sealed class MusicSheetCreatedEvent : DomainEventBase
    {
        public MusicSheet MusicSheet { get; }
        public MusicSheetCreatedEvent(MusicSheet musicSheet)
        {
            MusicSheet = musicSheet;
        }
    }

    internal class MusicSheetCreatedEventHandler() : INotificationHandler<MusicSheetCreatedEvent>
    {
        public Task Handle(MusicSheetCreatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
