using System.Linq.Expressions;

namespace RavenDB.Specifications
{
    public class ContainsSpecification<T> : Specification<T>, ISearchSpecification<T>
    {
        private readonly string _propertyName;
        private readonly string _value;

        public string PropertyName => _propertyName;
        public string SearchTerm => $"*{_value}*";

        public ContainsSpecification(string propertyName, string value)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _propertyName);
            var constant = Expression.Constant(_value);
            
            // Standard String.Contains(string)
            // Note: RavenDB LINQ provider will throw NotSupportedException if this is used in a .Where()
            // but it's needed for general implementation completeness. The Queries<T> helper
            // will intercept this via ISearchSpecification and use .Search() instead.
            var method = typeof(string).GetMethod("Contains", [typeof(string)])!;
            var body = Expression.Call(property, method, Expression.Convert(constant, typeof(string)));
            
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
