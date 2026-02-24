using System.Linq.Expressions;

namespace RavenDB.Specifications
{
    public class AndSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T>[] _specifications;

        public AndSpecification(params ISpecification<T>[] specifications)
        {
            _specifications = specifications ?? throw new ArgumentNullException(nameof(specifications));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            if (_specifications == null || _specifications.Length == 0)
                return null;

            Expression<Func<T, bool>> combinedExpression = null;

            foreach (var specification in _specifications)
            {
                var currentExpression = specification.ToExpression();

                if (combinedExpression == null)
                    combinedExpression = currentExpression;
                else
                {
                    combinedExpression = GetExpression(combinedExpression, currentExpression, Expression.AndAlso);
                }
            }

            return combinedExpression;
        }
    }
}
