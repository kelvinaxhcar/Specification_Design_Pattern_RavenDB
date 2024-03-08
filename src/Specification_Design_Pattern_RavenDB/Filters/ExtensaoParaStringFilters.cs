using System.Text.RegularExpressions;

namespace Specification_Design_Pattern_RavenDB.Filters
{
    public static class ExtensaoParaStringFilters
    {
        public static List<FilterQuery> TransformarEmFilterQuery(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new List<FilterQuery>() { };
            }

            var internalSeparator = "|";
            return value
                .Split(internalSeparator)
                .Select(val =>
                {
                    foreach (var def in ConstantesFilterFields.Regex)
                    {
                        if (def.Value is null)
                            continue;

                        var regex = new Regex(def.Value);
                        var match = regex.Match(val);
                        if (match.Success)
                        {
                            var captGroupsVals = match.Groups.Values.Select(x => x.Value).Skip(1).ToList();
                            return new FilterQuery
                            {
                                Operator = def.Key,
                                Values = captGroupsVals
                            };
                        }
                    }

                    return null;
                })
                .Where(x => x != null)
                .OfType<FilterQuery>()
                .ToList();
        }
    }
}
