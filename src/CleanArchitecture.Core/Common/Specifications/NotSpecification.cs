using System.Linq.Expressions;

namespace CleanArchitecture.Core.Common.Specifications
{
    public class NotSpecification<T> : Specification<T>
    {
        private Specification<T> _specification;

        public NotSpecification(Specification<T> specification)
        {
            _specification = specification;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var expression = _specification.ToExpression();
            var param = Expression.Parameter(typeof(T));
            var body = Expression.Not(Expression.Invoke(expression, param));

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}