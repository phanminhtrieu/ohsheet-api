namespace CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging
{
    public class DataTablePagedResult<T> : PagedResultBase where T : class
    {
        public IEnumerable<T>? Items { get; set; }

        public DataTablePagedResult() { }

        public DataTablePagedResult(IEnumerable<T> items, int pageIndex = 0, int? pageSize = 10, int totalRecords = 0)
        {
            PageIndex = pageIndex;
            PageSize = (int)pageSize!;
            TotalRecords = totalRecords;
            Items = items;
        }
    }
}
