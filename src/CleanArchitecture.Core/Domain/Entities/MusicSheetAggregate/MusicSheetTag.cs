namespace CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate
{
    public class MusicSheetTag : EntityBase<int>
    {
        public string Name { get; private set; }

        // Navigation N - N with MusicSheet (there is a list MusicSheetTag on a MusicSheet)
        private readonly List<MusicSheet> _musicSheets = new();
        public IReadOnlyCollection<MusicSheet> MusicSheets => _musicSheets.AsReadOnly();

        private MusicSheetTag() { }

        public MusicSheetTag(string name)
        {
            Name = name;
        }
    }
}
