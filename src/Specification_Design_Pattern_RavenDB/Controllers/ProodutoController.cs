using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Specification_Design_Pattern_RavenDB.Especificacoes;

namespace Specification_Design_Pattern_RavenDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProodutoController : ControllerBase
    {

        public IActionResult Get()
        {

            return Ok();
        }
    }

    public static class Querys<T>
    {
        public static IQueryable<T> Filtrar(IDocumentSession session, params ISpecification<T>[] especificacao)
        {
            var query = session.Query<T>();
            foreach (var item in especificacao)
            {
                query = query.Where(item.ToExpression());
            }
            return query;
        }
    }

    public class Produto
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public decimal Preco { get; set; }
        public string Marca { get; set; }
    }
}
