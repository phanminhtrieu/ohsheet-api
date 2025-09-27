namespace CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging
{
    public class PagedResultBase
    {
        public int? PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }
        public string? Message { set; get; }

        public int PageCount
        {
            get
            {
                var pageCount = TotalRecords > 0 ? (PageIndex != null ? (double)TotalRecords / PageSize : 1) : 0;
                return (int)Math.Ceiling(pageCount);
            }
            set {; }
        }   
    }
}
