using Specification_Design_Pattern_RavenDB.Controllers;
using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class EspecificacaoE<T> : Especificacao<T>
    {
        private readonly ISpecification<T>[] _especificacoes;

        public EspecificacaoE(params ISpecification<T>[] especificacoes)
        {
            _especificacoes = especificacoes ?? throw new ArgumentNullException(nameof(especificacoes));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            if (_especificacoes == null || _especificacoes.Length == 0)
                return null;

            Expression<Func<T, bool>> expressaoCombinada = null;

            foreach (var especificacao in _especificacoes)
            {
                var expressaoAtual = especificacao.ToExpression();

                if (expressaoCombinada == null)
                    expressaoCombinada = expressaoAtual;
                else
                {
                    expressaoCombinada = ObterExpression(expressaoCombinada, expressaoAtual, Expression.AndAlso);
                }
            }

            return expressaoCombinada;
        }
    }
}
