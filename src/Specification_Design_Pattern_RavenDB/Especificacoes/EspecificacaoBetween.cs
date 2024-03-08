using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class EspecificacaoBetween<T> : Especificacao<T>
    {
        private readonly string _nomePropriedade;
        private readonly object _valorMinimo;
        private readonly object _valorMaximo;

        public EspecificacaoBetween(string nomePropriedade, string valorMinimo, string valorMaximo)
        {
            _nomePropriedade = nomePropriedade ?? throw new ArgumentNullException(nameof(nomePropriedade));
            _valorMinimo = ConverterValor(valorMinimo, typeof(T).GetProperty(nomePropriedade).PropertyType);
            _valorMaximo = ConverterValor(valorMaximo, typeof(T).GetProperty(nomePropriedade).PropertyType);
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parametro = Expression.Parameter(typeof(T), "x");
            var propriedade = Expression.Property(parametro, _nomePropriedade);
            var constanteMinima = Expression.Constant(_valorMinimo);
            var constanteMaxima = Expression.Constant(_valorMaximo);

            var greaterThanOrEqual = Expression.GreaterThanOrEqual(propriedade, constanteMinima);
            var lessThanOrEqual = Expression.LessThanOrEqual(propriedade, constanteMaxima);

            var andExpression = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

            return Expression.Lambda<Func<T, bool>>(andExpression, parametro);
        }
    }
}
