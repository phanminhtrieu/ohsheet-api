namespace CleanArchitecture.Core.Interfaces.MusicSheetServices
{
    public interface IMusicSheetViewService
    {
        Task IncrementViewCount(int musicSheetId);
    }
}
