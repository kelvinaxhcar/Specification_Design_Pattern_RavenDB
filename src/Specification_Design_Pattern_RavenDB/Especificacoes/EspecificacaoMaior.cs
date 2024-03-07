using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class EspecificacaoMaior<T> : Especificacao<T>
    {
        private readonly string _nomePropriedade;
        private readonly object _valor;

        public EspecificacaoMaior(string nomePropriedade, object valor)
        {
            _nomePropriedade = nomePropriedade ?? throw new ArgumentNullException(nameof(nomePropriedade));
            _valor = valor;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parametro = Expression.Parameter(typeof(T), "x");
            var propriedade = Expression.Property(parametro, _nomePropriedade);
            var constante = Expression.Constant(_valor);
            var maiorQue = Expression.GreaterThan(propriedade, Expression.Convert(constante, propriedade.Type));
            return Expression.Lambda<Func<T, bool>>(maiorQue, parametro);
        }
    }

}
