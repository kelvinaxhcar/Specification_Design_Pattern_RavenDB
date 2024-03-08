using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class EspecificacaoEndsWith<T> : Especificacao<T>
    {
        private readonly string _nomePropriedade;
        private readonly object _valor;

        public EspecificacaoEndsWith(string nomePropriedade, string valor)
        {
            _nomePropriedade = nomePropriedade ?? throw new ArgumentNullException(nameof(nomePropriedade));
            _valor = ConverterValor(valor, typeof(T).GetProperty(nomePropriedade).PropertyType);
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parametro = Expression.Parameter(typeof(T), "x");
            var propriedade = Expression.Property(parametro, _nomePropriedade);
            var constante = Expression.Constant(_valor);
            var startsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            var startsWithExpression = Expression.Call(propriedade, startsWithMethod, Expression.Convert(constante, typeof(string)));
            return Expression.Lambda<Func<T, bool>>(startsWithExpression, parametro);
        }
    }
}
