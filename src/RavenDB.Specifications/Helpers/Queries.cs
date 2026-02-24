using System.Linq.Expressions;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Queries;
using RavenDB.Specifications;

namespace RavenDB.Specifications
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
                    if (specification is ISearchSpecification<T> searchSpec)
                    {
                        var parameter = Expression.Parameter(typeof(T), "x");
                        var property = Expression.Property(parameter, searchSpec.PropertyName);
                        var conversion = Expression.Convert(property, typeof(object));
                        var fieldSelector = Expression.Lambda<Func<T, object>>(conversion, parameter);

                        query = query.Search(fieldSelector, searchSpec.SearchTerm, @operator: SearchOperator.And);
                    }
                    else
                    {
                        query = query.Where(specification.ToExpression());
                    }
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
                    if (specification is ISearchSpecification<T> searchSpec)
                    {
                        var parameter = Expression.Parameter(typeof(T), "x");
                        var property = Expression.Property(parameter, searchSpec.PropertyName);
                        var conversion = Expression.Convert(property, typeof(object));
                        var fieldSelector = Expression.Lambda<Func<T, object>>(conversion, parameter);

                        query = query.Search(fieldSelector, searchSpec.SearchTerm, @operator: SearchOperator.And);
                    }
                    else
                    {
                        query = query.Where(specification.ToExpression());
                    }
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
