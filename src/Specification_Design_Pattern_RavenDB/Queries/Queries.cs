using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Specification_Design_Pattern_RavenDB.Specifications;

namespace Specification_Design_Pattern_RavenDB.Queries
{
    public static class Queries<T>
    {
        public static IQueryable<T> Filter(IDocumentSession session, params ISpecification<T>[] specifications)
        {
            IRavenQueryable<T> query = session.Query<T>();
            foreach (var specification in specifications)
            {
                if (specification != null)
                {
                    query = query.Where(specification.ToExpression());
                }
            }
            return query;
        }

        public static IAsyncDocumentQuery<T> Filter(IAsyncDocumentSession session, params ISpecification<T>[] specifications)
        {
            IRavenQueryable<T> query = session.Query<T>();
            foreach (var specification in specifications)
            {
                if (specification != null)
                {
                    query = query.Where(specification.ToExpression());
                }
            }
            return query.ToAsyncDocumentQuery();
        }
        public static IQueryable<T> FilterBySql(IDocumentSession session, string sqlQuery)
        {
            var specification = SqlToSpecificationConverter.Convert<T>(sqlQuery);
            return Filter(session, specification);
        }

        public static IAsyncDocumentQuery<T> FilterBySql(IAsyncDocumentSession session, string sqlQuery)
        {
            var specification = SqlToSpecificationConverter.Convert<T>(sqlQuery);
            return Filter(session, specification);
        }
    }
}
