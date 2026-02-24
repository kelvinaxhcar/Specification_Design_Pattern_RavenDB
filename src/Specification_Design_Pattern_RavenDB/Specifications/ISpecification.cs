using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
    }
}
