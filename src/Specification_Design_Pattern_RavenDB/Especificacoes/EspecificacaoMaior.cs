using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class EspecificacaoMaior<T> : Especificacao<T>
    {
        private readonly Expression<Func<T, object>> _expressaoGetPropriedade;
        private readonly object _valor;

        public EspecificacaoMaior(Expression<Func<T, object>> expressaoGetPropriedade, object valor)
        {
            _expressaoGetPropriedade = expressaoGetPropriedade ?? throw new ArgumentNullException(nameof(expressaoGetPropriedade));
            _valor = valor;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parametro = _expressaoGetPropriedade.Parameters[0];
            var acessoPropriedade = _expressaoGetPropriedade.Body;

            return ObterExpression(_valor, parametro, acessoPropriedade, Expression.GreaterThan);
        }
    }
}
