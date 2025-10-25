namespace CleanArchitecture.Core.Interfaces.MusicSheetServices
{
    public interface IViewCounterCacheService
    {
        void IncrementViewCount(int musicSheetId);
        Dictionary<int, int> GetAndResetViewCounts();
    }
}
