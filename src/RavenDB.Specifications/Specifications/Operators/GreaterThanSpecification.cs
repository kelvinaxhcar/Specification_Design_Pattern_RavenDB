using System.Linq.Expressions;

namespace RavenDB.Specifications
{
    public class GreaterThanSpecification<T> : Specification<T>
    {
        private readonly string _propertyName;
        private readonly object _value;

        public GreaterThanSpecification(string propertyName, string value)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _value = ConvertValue(value, GetPropertyType(propertyName));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _propertyName);
            var constant = Expression.Constant(_value);
            var greaterThan = Expression.GreaterThan(property, Expression.Convert(constant, property.Type));
            return Expression.Lambda<Func<T, bool>>(greaterThan, parameter);
        }
    }
}
