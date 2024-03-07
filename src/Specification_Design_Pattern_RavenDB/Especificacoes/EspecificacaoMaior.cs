using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class EspecificacaoMaior<T> : EspecificacaoBase<T>
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

            var tipoPropriedade = acessoPropriedade.Type;
            var valorConvertido = Convert.ChangeType(_valor, tipoPropriedade);
            var acessoPropriedadeConvertido = Expression.Convert(acessoPropriedade, valorConvertido.GetType());
            var expressaoValor = Expression.Constant(valorConvertido);
            var comparacao = Expression.GreaterThan(acessoPropriedadeConvertido, expressaoValor);
            var lambda = Expression.Lambda<Func<T, bool>>(comparacao, parametro);

            return lambda;
        }
    }
}
