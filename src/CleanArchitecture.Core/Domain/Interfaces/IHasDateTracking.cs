namespace CleanArchitecture.Core.Domain.Interfaces
{
    public interface IHasDateTracking
    {
        public DateTimeOffset CreatedDate { get; }
        public DateTimeOffset ModifiedDate { get; }
    }
}
