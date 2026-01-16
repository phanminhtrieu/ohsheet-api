using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;

namespace CleanArchitecture.Core.Domain.Models.MusicSheet
{
    public class MusicSheetPagingRequest : PagingRequestBase
    {
        public string? FilterBy { get; set; } // top_like | hot_week | hot_month
        public List<string>? Tags { get; set; }
    }
}
