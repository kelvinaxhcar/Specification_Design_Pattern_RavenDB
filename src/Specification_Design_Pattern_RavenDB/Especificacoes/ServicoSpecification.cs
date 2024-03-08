using Specification_Design_Pattern_RavenDB.Filters;
using System.Reflection;

namespace Specification_Design_Pattern_RavenDB.Especificacoes
{
    public class ServicoSpecification
    {
        public ISpecification<T>?[] ObterQyerys<T>(Dictionary<string, FilterQuery> filterQueries)
        {
            return filterQueries.Select(filtro =>
            {
                const int um = (int)decimal.One;
                const int zero = (int)decimal.Zero;
                var propriedade = typeof(T)
                                    .GetProperties()
                                    .FirstOrDefault(x => x.Name.Equals(filtro.Key, StringComparison.CurrentCultureIgnoreCase));

                var operador = filtro.Value.Operator;

                if (propriedade is null || filtro.Value.Values.Count == zero ||
                    (operador == ConstantesFilterFields.Operator_Between && filtro.Value.Values.Count == um))
                {
                    return null;
                }

                var valor1 = filtro.Value.Values[zero];
                var valor2 = filtro.Value.Values.Count > um ? filtro.Value.Values[um] : string.Empty;
                var especificacoes = ObterEspecificacoes<T>(propriedade, valor1);

                if (!string.IsNullOrEmpty(valor2))
                {
                    especificacoes.Add(new(ConstantesFilterFields.Operator_Between, new EspecificacaoBetween<T>(propriedade.Name, valor1, valor2)));
                }

                return especificacoes
                                .FirstOrDefault(x => x.Key.Equals(operador))
                                .Value;
            })
            .Where(x => x is not null)
            .ToArray() ?? [];
        }

        private static List<KeyValuePair<string, ISpecification<T>>> ObterEspecificacoes<T>(PropertyInfo? propriedade, string valor1)
        {
            return new List<KeyValuePair<string, ISpecification<T>>>()
                {
                    new(ConstantesFilterFields.Operator_Equal, new EspecificacaoEquals<T>(propriedade.Name, valor1)),
                    new(ConstantesFilterFields.Operator_NotEqual, new EspecificacaoNotEqual<T>(propriedade.Name, valor1)),
                    new(ConstantesFilterFields.Operator_StartsWith, new EspecificacaoStartsWith<T>(propriedade.Name, valor1)),
                    new(ConstantesFilterFields.Operator_GreatThen, new EspecificacaoGreatThen<T>(propriedade.Name, valor1)),
                    new(ConstantesFilterFields.Operator_LessThen, new EspecificacaoLessThen<T>(propriedade.Name, valor1)),
                    new(ConstantesFilterFields.Operator_LessEqualThen, new EspecificacaoLessThanOrEqual<T>(propriedade.Name, valor1)),
                    new(ConstantesFilterFields.Operator_GreatEqualThen, new EspecificacaoGreaterThanOrEqual<T>(propriedade.Name, valor1)),
                    new(ConstantesFilterFields.Operator_EndsWith, new EspecificacaoEndsWith<T>(propriedade.Name, valor1)),
                };
        }
    }
}
