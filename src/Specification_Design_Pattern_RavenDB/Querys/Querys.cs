using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Specification_Design_Pattern_RavenDB.Especificacoes;

namespace Specification_Design_Pattern_RavenDB.Querys
{
    public static class Querys<T>
    {
        public static IQueryable<T> Filtrar(IDocumentSession session, params ISpecification<T>[] especificacao)
        {
            var query = session.Query<T>();
            for (int i = 0; i < especificacao.Length; i++)
            {
                ISpecification<T>? item = especificacao[i];
                query = query.Where(item.ToExpression());
            }
            return query;
        }
    }
}
