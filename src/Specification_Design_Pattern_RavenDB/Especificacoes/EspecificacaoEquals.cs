using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class EspecificacaoEquals<T> : ISpecification<T>
    {
        private readonly Expression<Func<T, string>> _expressaoGetPropriedade;
        private readonly string _valor;

        public EspecificacaoEquals(Expression<Func<T, string>> expressaoGetPropriedade, string valor)
        {
            _expressaoGetPropriedade = expressaoGetPropriedade ?? throw new ArgumentNullException(nameof(expressaoGetPropriedade));
            _valor = valor?.ToLower() ?? throw new ArgumentNullException(nameof(valor));
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var parametro = _expressaoGetPropriedade.Parameters[(int)decimal.Zero];
            var acessoPropriedade = _expressaoGetPropriedade.Body;

            var expressaoValor = Expression.Constant(_valor, typeof(string));
            var metodoContem = typeof(object).GetMethod("Equals", new[] { typeof(string) });
            var chamadaContem = Expression.Call(acessoPropriedade, metodoContem, expressaoValor);

            return Expression.Lambda<Func<T, bool>>(chamadaContem, parametro);
        }
    }
}
