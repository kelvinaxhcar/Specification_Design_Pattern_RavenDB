﻿using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class EspecificacaoOu<T> : EspecificacaoBase<T>
    {
        private readonly ISpecification<T>[] _especificacoes;

        public EspecificacaoOu(params ISpecification<T>[] especificacoes)
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
                    var parametro = expressaoCombinada.Parameters.Single();
                    var substituirParametro = new SubstituirParametroVisitor(parametro, expressaoAtual.Parameters.Single());
                    var bodyCombinado = Expression.OrElse(substituirParametro.Visit(expressaoCombinada.Body), expressaoAtual.Body);
                    expressaoCombinada = Expression.Lambda<Func<T, bool>>(bodyCombinado, parametro);
                }
            }

            return expressaoCombinada;
        }
    }
}