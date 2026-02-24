using System.Linq.Expressions;

namespace RavenDB.Specifications
{
    public class StartsWithSpecification<T> : Specification<T>
    {
        private readonly string _propertyName;
        private readonly object _value;

        public StartsWithSpecification(string propertyName, string value)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _value = ConvertValue(value, GetPropertyType(propertyName));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _propertyName);
            var constant = Expression.Constant(_value);
            var startsWithMethod = typeof(string).GetMethod("StartsWith", [typeof(string)])!;
            var startsWithExpression = Expression.Call(property, startsWithMethod, Expression.Convert(constant, typeof(string)));
            return Expression.Lambda<Func<T, bool>>(startsWithExpression, parameter);
        }
    }
}
