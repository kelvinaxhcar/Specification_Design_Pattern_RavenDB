using System.Linq.Expressions;

namespace RavenDB.Specifications
{
    public class NotEqualSpecification<T> : Specification<T>
    {
        private readonly string _propertyName;
        private readonly object _value;

        public NotEqualSpecification(string propertyName, string value)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _value = ConvertValue(value, GetPropertyType(propertyName));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _propertyName);
            var constant = Expression.Constant(_value);
            var notEqual = Expression.NotEqual(property, Expression.Convert(constant, property.Type));
            return Expression.Lambda<Func<T, bool>>(notEqual, parameter);
        }
    }

    public class GreaterThanOrEqualSpecification<T> : Specification<T>
    {
        private readonly string _propertyName;
        private readonly object _value;

        public GreaterThanOrEqualSpecification(string propertyName, string value)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _value = ConvertValue(value, GetPropertyType(propertyName));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _propertyName);
            var constant = Expression.Constant(_value);
            var gte = Expression.GreaterThanOrEqual(property, Expression.Convert(constant, property.Type));
            return Expression.Lambda<Func<T, bool>>(gte, parameter);
        }
    }

    public class LessThanOrEqualSpecification<T> : Specification<T>
    {
        private readonly string _propertyName;
        private readonly object _value;

        public LessThanOrEqualSpecification(string propertyName, string value)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _value = ConvertValue(value, GetPropertyType(propertyName));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _propertyName);
            var constant = Expression.Constant(_value);
            var lte = Expression.LessThanOrEqual(property, Expression.Convert(constant, property.Type));
            return Expression.Lambda<Func<T, bool>>(lte, parameter);
        }
    }
}
