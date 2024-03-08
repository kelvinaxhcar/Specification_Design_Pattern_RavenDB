using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Specification_Design_Pattern_RavenDB.Filters
{
    public abstract class CustomFilter
    {
        public Dictionary<string, FilterQuery> FilterQueries;

        public CustomFilter(string queryParams)
        {
            FilterQueries = new Dictionary<string, FilterQuery>();
            var parsed = HttpUtility.ParseQueryString(queryParams);

            foreach (var key in parsed.AllKeys)
            {
                var value = parsed
                    .GetValues(key)
                    .FirstOrDefault();

                var filters = value
                    .TransformarEmFilterQuery();

                var grouped = filters
                    .GroupBy(x => x.Operator); // deveria ser possivel pesquisar por multiplos operadores de uma so vez? faz sentido isso?

                var first = grouped.FirstOrDefault();

                if (first is null)
                    continue;

                FilterQueries.Add(key, new FilterQuery()
                {
                    Operator = first.Key,
                    Values = first.SelectMany(x => x.Values).ToList()
                });
            }
        }
    }
}
