using System.Linq.Expressions;
using System.Reflection;

namespace RavenDB.Specifications
{
    public class EndsWithSpecification<T> : Specification<T>
    {
        private readonly string _propertyName;
        private readonly string _value;

        public EndsWithSpecification(string propertyName, string value)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _propertyName);
            var constant = Expression.Constant(_value);
            
            var method = typeof(string).GetMethod("EndsWith", [typeof(string)])!;
            var body = Expression.Call(property, method, Expression.Convert(constant, typeof(string)));
            
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
