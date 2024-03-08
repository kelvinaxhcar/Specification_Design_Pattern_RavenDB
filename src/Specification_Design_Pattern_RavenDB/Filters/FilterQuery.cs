using System.Collections.Generic;

namespace Specification_Design_Pattern_RavenDB.Filters
{
    public class FilterQuery
    {
        public string Operator { get; set; }
        public List<string> Values { get; set; }
    }
}
