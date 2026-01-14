using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;

namespace CleanArchitecture.UseCases.Backoffice.Users.Queries
{
    public class UserPagingRequest : PagingRequestBase
    {
        public string? TextSearch { get; set; }
    }
}
