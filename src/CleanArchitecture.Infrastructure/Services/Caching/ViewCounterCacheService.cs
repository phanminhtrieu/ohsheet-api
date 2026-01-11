using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using System.Collections.Concurrent;

namespace CleanArchitecture.Infrastructure.Services.Caching
{
    public class ViewCounterCacheService : IViewCounterCacheService
    {
        private readonly ConcurrentDictionary<int, int> _viewCounts = new();

        public void IncrementViewCount(int musicSheetId)
        {
            _viewCounts.AddOrUpdate(musicSheetId, 1, (_, old) => old + 1);
        }

        public Dictionary<int, int> GetAndResetViewCounts()
        {
            var snapshot = _viewCounts.ToDictionary(entry => entry.Key, entry => entry.Value);
            _viewCounts.Clear();
            return snapshot;
        }
    }
}
