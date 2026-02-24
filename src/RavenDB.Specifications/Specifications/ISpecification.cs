using System.Linq.Expressions;

namespace RavenDB.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
    }
}
