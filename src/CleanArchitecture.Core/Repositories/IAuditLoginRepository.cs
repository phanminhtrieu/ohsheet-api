using CleanArchitecture.Core.Domain.Entities.AuditLogin;
using CleanArchitecture.Core.UnitOfWork;

namespace CleanArchitecture.Core.Repositories
{
    public interface IAuditLoginRepository : IRepository<AuditLogin>
    {
    }
}
