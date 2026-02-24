using System.Linq.Expressions;

namespace RavenDB.Specifications
{
    public class NotSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> specification)
        {
            _specification = specification ?? throw new ArgumentNullException(nameof(specification));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var expression = _specification.ToExpression();
            var notExpression = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters);
        }
    }
}
