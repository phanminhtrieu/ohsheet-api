namespace CleanArchitecture.UseCases.MusicSheets.Comments
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public int? ParentId { get; set; }
    }
}
