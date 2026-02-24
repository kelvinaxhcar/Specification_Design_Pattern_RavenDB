using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Specifications
{
    public class EqualitySpecification<T> : Specification<T>
    {
        private readonly string _propertyName;
        private readonly object _value;

        public EqualitySpecification(string propertyName, string value)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _value = ConvertValue(value, GetPropertyType(propertyName));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _propertyName);
            var constant = Expression.Constant(_value);
            var equality = Expression.Equal(property, Expression.Convert(constant, property.Type));
            return Expression.Lambda<Func<T, bool>>(equality, parameter);
        }
    }
}
