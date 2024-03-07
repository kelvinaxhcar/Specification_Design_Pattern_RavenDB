using Specification_Design_Pattern_RavenDB.Controllers;
using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class EspecificacaoProduto : EspecificacaoBase<Produto>
    {
        private readonly string _termoPesquisa;

        public EspecificacaoProduto(string termoPesquisa)
        {
            _termoPesquisa = termoPesquisa?.ToLower() ?? throw new ArgumentNullException(nameof(termoPesquisa));
        }

        public override Expression<Func<Produto, bool>> ToExpression()
        {
            var termoPesquisaExpression = Expression.Constant(_termoPesquisa);
            var parameterExpression = Expression.Parameter(typeof(Produto), nameof(Produto).ToLower());

            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            Expression finalExpression = null;

            foreach (var property in typeof(Produto).GetProperties())
            {
                if (property.PropertyType == typeof(string) || property.PropertyType == typeof(decimal))
                {
                    var propertyExpression = Expression.Property(parameterExpression, property);
                    var toLowerMethod = Expression.Call(propertyExpression, "ToLower", null);
                    var containsCall = Expression.Call(toLowerMethod, containsMethod, termoPesquisaExpression);

                    finalExpression = finalExpression == null ? containsCall : Expression.Or(finalExpression, containsCall);
                }
            }

            return finalExpression == null ? null : Expression.Lambda<Func<Produto, bool>>(finalExpression, parameterExpression);
        }
    }
}
