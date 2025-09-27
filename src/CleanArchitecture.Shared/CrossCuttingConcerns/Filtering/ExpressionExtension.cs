using System.Linq.Expressions;

namespace CleanArchitecture.Shared.CrossCuttingConcerns.Filtering
{
    public static class ExpressionExtension
    {
        public static Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(
                Expression.Invoke(left, parameter),
                Expression.Invoke(right, parameter)
            );
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
