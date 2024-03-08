﻿using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class EspecificacaoLessThanOrEqual<T> : Especificacao<T>
    {
        private readonly string _nomePropriedade;
        private readonly object _valor;

        public EspecificacaoLessThanOrEqual(string nomePropriedade, string valor)
        {
            _nomePropriedade = nomePropriedade ?? throw new ArgumentNullException(nameof(nomePropriedade));
            _valor = ConverterValor(valor, typeof(T).GetProperty(nomePropriedade).PropertyType);
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var parametro = Expression.Parameter(typeof(T), "x");
            var propriedade = Expression.Property(parametro, _nomePropriedade);
            var constante = Expression.Constant(_valor);
            var menorQue = Expression.LessThanOrEqual(propriedade, Expression.Convert(constante, propriedade.Type));
            return Expression.Lambda<Func<T, bool>>(menorQue, parametro);
        }
    }
}