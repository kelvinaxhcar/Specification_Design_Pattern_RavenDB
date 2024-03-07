using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class EspecificacaoBase<T> 
    {
        public Expression<Func<T, bool>> ObterExpression(object valor, ParameterExpression parametro, Expression acessoPropriedade, Func<Expression, Expression, BinaryExpression> func)
        {
            var tipoPropriedade = acessoPropriedade.Type;
            var valorConvertido = Convert.ChangeType(valor, tipoPropriedade);
            var acessoPropriedadeConvertido = Expression.Convert(acessoPropriedade, valorConvertido.GetType());

            var expressaoValor = Expression.Constant(valorConvertido);
            var comparacao = func.Invoke(acessoPropriedadeConvertido, expressaoValor);
            return Expression.Lambda<Func<T, bool>>(comparacao, parametro);
        }

        public Expression<Func<T, bool>> ObterExpression(Expression<Func<T, bool>> expressao1, Expression<Func<T, bool>> expressao2, Func<Expression, Expression, BinaryExpression> func)
        {
            var parametro1 = expressao1.Parameters.Single();
            var parametro2 = expressao2.Parameters.Single();
            var substituirParametro = new SubstituirParametroVisitor(parametro1, parametro2);
            var bodyCombinado = func.Invoke(substituirParametro.Visit(expressao1.Body), expressao2.Body);
            return Expression.Lambda<Func<T, bool>>(bodyCombinado, parametro1);
        }

        public object ConverterValor(string valor, Type tipo)
        {
            var tipoBase = Nullable.GetUnderlyingType(tipo) ?? tipo;
            return Convert.ChangeType(valor, tipoBase);
        }
    }
}
