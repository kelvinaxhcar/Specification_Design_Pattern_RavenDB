using System.Linq.Expressions;

namespace RavenDB.Specifications
{
    public class InSpecification<T> : Specification<T>
    {
        private readonly string _propertyName;
        private readonly List<object> _values;

        public InSpecification(string propertyName, IEnumerable<string> values)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            var type = GetPropertyType(propertyName);
            _values = values.Select(v => ConvertValue(v, type)).ToList();
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            if (_values == null || !_values.Any())
                return x => false;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _propertyName);
            
            Expression combined = null;

            foreach (var value in _values)
            {
                var constant = Expression.Constant(value);
                var equality = Expression.Equal(property, Expression.Convert(constant, property.Type));

                if (combined == null)
                    combined = equality;
                else
                    combined = Expression.OrElse(combined, equality);
            }

            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }
    }
}
