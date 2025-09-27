using CleanArchitecture.Core.Common.Specifications;
using CleanArchitecture.Core.Interfaces;
using System.Linq.Expressions;

namespace CleanArchitecture.Core.Common
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public bool IsSatisfiedBy(T entity)
        {
            var predicate = ToExpression().Compile();
            return predicate(entity);
        }

        public abstract Expression<Func<T, bool>> ToExpression();

        public Specification<T> And(Specification<T> otherSpecification)
            => new AndSpecification<T>(this, otherSpecification);

        public Specification<T> Or(Specification<T> otherSpecification)
            => new OrSpecification<T>(this, otherSpecification);

        public Specification<T> Not()
            => new NotSpecification<T>(this);
    }
}
