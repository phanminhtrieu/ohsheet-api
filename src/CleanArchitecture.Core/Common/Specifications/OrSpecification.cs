using System.Linq.Expressions;

namespace CleanArchitecture.Core.Common.Specifications
{
    public class OrSpecification<T> : Specification<T>
    {
        private Specification<T> _left;
        private Specification<T> _right;

        public OrSpecification(Specification<T> left, Specification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpr = _left.ToExpression();
            var rightExpr = _right.ToExpression();

            var param = Expression.Parameter(typeof(T));
            var body = Expression.OrElse(
                Expression.Invoke(leftExpr, param),
                Expression.Invoke(rightExpr, param)
            );

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}