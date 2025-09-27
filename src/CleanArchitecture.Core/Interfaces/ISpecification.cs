using System.Linq.Expressions;

namespace CleanArchitecture.Core.Interfaces
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);
        Expression<Func<T, bool>> ToExpression();
    }
}
