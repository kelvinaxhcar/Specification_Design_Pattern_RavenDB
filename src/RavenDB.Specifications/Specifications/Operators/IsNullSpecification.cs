using System.Linq.Expressions;

namespace RavenDB.Specifications
{
    public class IsNullSpecification<T> : Specification<T>
    {
        private readonly string _propertyName;

        public IsNullSpecification(string propertyName)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _propertyName);
            var isNull = Expression.Equal(property, Expression.Constant(null, property.Type));
            return Expression.Lambda<Func<T, bool>>(isNull, parameter);
        }
    }
}
