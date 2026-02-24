using System.Linq.Expressions;

namespace RavenDB.Specifications
{
    public interface ISearchSpecification<T>
    {
        string PropertyName { get; }
        string SearchTerm { get; }
    }
}
