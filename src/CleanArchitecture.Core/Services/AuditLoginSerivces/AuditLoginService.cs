using CleanArchitecture.Core.Domain.Entities.AuditLogin;
using CleanArchitecture.Core.Domain.Models.AuditLogins;
using CleanArchitecture.Core.Interfaces.AuditLoginServices;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace CleanArchitecture.Core.Services.AuditLoginSerivces
{
    public class AuditLoginService : IAuditLoginService
    {
        private readonly IAuditLoginRepository _auditLoginRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        private string UserAgent = string.Empty;
        private string RemoteIpAddress = string.Empty;
        private string HostName = string.Empty;
        private string Method = string.Empty;
        private string Path = string.Empty;

        public AuditLoginService(
            IAuditLoginRepository auditLoginRepository, 
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _auditLoginRepository = auditLoginRepository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;

            _httpContextAccessor = httpContextAccessor;

            UserAgent = _httpContextAccessor.HttpContext != null ?
                _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString() : string.Empty;

            RemoteIpAddress = _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Connection != null &&
                _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null ?
                _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString() : string.Empty;

            HostName = _httpContextAccessor.HttpContext != null ? _httpContextAccessor.HttpContext.Request.Host.Value : string.Empty;
            Method = _httpContextAccessor.HttpContext != null ? _httpContextAccessor.HttpContext.Request.Method : string.Empty;
            Path = _httpContextAccessor.HttpContext != null ? _httpContextAccessor.HttpContext.Request.Path : string.Empty;
        }
        
        public async Task<DataTablePagedResult<AuditLoginResponse>> ListByPaging(ManageAuditLoginPagingRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<AuditLogin, bool>> filter = x => true;

            // We can search by UserId, UserName, Email, UserAgent
            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                filter = x => (x.UserId != null && x.UserId.Contains(request.TextSearch)) ||
                              (x.UserName != null && x.UserName.Contains(request.TextSearch)) ||
                              (x.UserAgent != null && x.UserAgent.Contains(request.TextSearch)) ||
                              (x.IpAddress != null && x.IpAddress.Contains(request.TextSearch));
            }

            return await _auditLoginRepository.ListByPagingAsync(request, filter, auditLogin => MapResponse(auditLogin), cancellationToken);
        }

        public async Task<ApiResult<int>> AddAsync(AuditLoginRequest request)
        {
            var auditLogin = new AuditLogin()
            {
                Url = $"{this.HostName}{this.Path}",
                Domain = this.HostName,
                UserAgent = $"{this.UserAgent}",
                IpAddress = this.RemoteIpAddress,
                UserName = request.UserName,
                IsSuccessded = request.IsSuccessded,
                Notes = request.Notes,
                UserId = request.UserId,
                DateCreated = DateTime.Now
            };

            await _auditLoginRepository.AddAsync(auditLogin);
            await _unitOfWork.SaveChangesAsync();

            return new ApiSuccessResult<int>(auditLogin.Id);   
        }

        private static AuditLoginResponse MapResponse(AuditLogin auditLogin)
        {
            return new AuditLoginResponse
            {
                Id = auditLogin.Id,
                UserId = auditLogin.UserId,
                Email = auditLogin.UserName,
                UserAgent = auditLogin.UserAgent,
                Domain = auditLogin.Domain,
                IpAddress = auditLogin.IpAddress,
                Url = auditLogin.Url,
                Notes = auditLogin.Notes,
                IsSuccessded = auditLogin.IsSuccessded,
                DateCreated = auditLogin.DateCreated
            };
        }
    }
}
