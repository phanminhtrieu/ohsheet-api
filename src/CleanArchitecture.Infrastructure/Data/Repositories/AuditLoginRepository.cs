using CleanArchitecture.Core.Domain.Entities.AuditLogin;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Infrastructure.Data.UnitOfWork;

namespace CleanArchitecture.Infrastructure.Data.Repositories
{
    public class AuditLoginRepository : Repository<AuditLogin>, IAuditLoginRepository
    {
        public AuditLoginRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
