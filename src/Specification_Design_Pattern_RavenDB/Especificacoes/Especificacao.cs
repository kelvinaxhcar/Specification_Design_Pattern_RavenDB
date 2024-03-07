using System.Linq.Expressions;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public abstract class Especificacao<T> : EspecificacaoBase<T>, ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public Especificacao<T> E(Especificacao<T> especificacao)
        {
            return new EspecificacaoE<T>(this, especificacao);
        }

        public Especificacao<T> Ou(Especificacao<T> especificacao)
        {
            return new EspecificacaoOu<T>(this, especificacao);
        }
    }
}
