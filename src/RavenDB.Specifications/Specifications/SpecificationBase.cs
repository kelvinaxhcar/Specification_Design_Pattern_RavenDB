using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace RavenDB.Specifications
{
    public class SpecificationBase<T> 
    {
        private static readonly ConcurrentDictionary<string, Type> PropertyInfoCache = new ConcurrentDictionary<string, Type>();

        public Expression<Func<T, bool>> GetExpression(object value, ParameterExpression parameter, Expression propertyAccess, Func<Expression, Expression, BinaryExpression> func)
        {
            var propertyType = propertyAccess.Type;
            var convertedValue = Convert.ChangeType(value, propertyType);
            var convertedPropertyAccess = Expression.Convert(propertyAccess, convertedValue.GetType());

            var valueExpression = Expression.Constant(convertedValue);
            var comparison = func.Invoke(convertedPropertyAccess, valueExpression);
            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }

        public Expression<Func<T, bool>> GetExpression(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2, Func<Expression, Expression, BinaryExpression> func)
        {
            var parameter1 = expression1.Parameters.Single();
            var parameter2 = expression2.Parameters.Single();
            var replaceParameter = new ReplaceParameterVisitor(parameter1, parameter2);
            var combinedBody = func.Invoke(replaceParameter.Visit(expression1.Body), expression2.Body);
            return Expression.Lambda<Func<T, bool>>(combinedBody, parameter1);
        }

        public object ConvertValue(string value, Type type)
        {
            var baseType = Nullable.GetUnderlyingType(type) ?? type;
            return Convert.ChangeType(value, baseType);
        }

        public static Type GetPropertyType(string propertyName)
        {
            return PropertyInfoCache.GetOrAdd(propertyName.ToLower(), _ => 
            {
                var propertyInfo = typeof(T)
                    .GetProperties()
                    .FirstOrDefault(x => x.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))?
                    .PropertyType;

                if (propertyInfo == null)
                    throw new Exception($"Property Type not found for [{propertyName}]");

                return propertyInfo;
            });
        }
    }
}
