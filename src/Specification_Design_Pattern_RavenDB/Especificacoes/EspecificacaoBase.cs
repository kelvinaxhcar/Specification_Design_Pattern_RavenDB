using Specification_Design_Pattern_RavenDB.Controllers;
using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public abstract class EspecificacaoBase<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public EspecificacaoBase<T> E(EspecificacaoBase<T> especificacao)
        {
            return new EspecificacaoE<T>(this, especificacao);
        }

        public EspecificacaoBase<T> Ou(EspecificacaoBase<T> especificacao)
        {
            return new EspecificacaoOu<T>(this, especificacao);
        }
    }
}
