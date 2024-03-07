using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class EspecificacaoEquals<T> : EspecificacaoBase<T>, ISpecification<T>
    {
        private readonly Expression<Func<T, object>> _expressaoGetPropriedade;
        private readonly object _valor;

        public EspecificacaoEquals(Expression<Func<T, object>> expressaoGetPropriedade, object valor)
        {
            _expressaoGetPropriedade = expressaoGetPropriedade ?? throw new ArgumentNullException(nameof(expressaoGetPropriedade));
            _valor = valor ?? throw new ArgumentNullException(nameof(valor));
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var parametro = _expressaoGetPropriedade.Parameters[0];
            var acessoPropriedade = _expressaoGetPropriedade.Body;
            return ObterExpression(_valor, parametro, acessoPropriedade, Expression.Equal);
        }
    }
}
