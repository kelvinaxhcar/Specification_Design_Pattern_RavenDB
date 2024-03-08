using System.Collections.Generic;

namespace Specification_Design_Pattern_RavenDB.Filters
{
    public class FilterField
    {
        public static readonly string Behaviour_VALIDVALUES = "VALIDVALUES";

        public static readonly string Behaviour_CHOOSEFROMLIST = "CHOOSEFROMLIST";

        public static readonly string Behaviour_VALUE = "VALUE";

        public static readonly string Fieldtype_DATE = "DATE";

        public static readonly string Fieldtype_INTEGER = "INTEGER";

        public static readonly string Fieldtype_DOUBLE = "DOUBLE";

        public static readonly string Fieldtype_STRING = "STRING";

        public string Fieldname { get; set; }
        public string? Label { get; set; }
        //public string Data { get; set; }
        public string? Behaviour { get; set; }
        public string Fieldtype { get; set; }
        public bool AllowOrder { get; set; }
        public bool AllowGroup { get; set; }
        public int VisOrder { get; set; }
        public bool Visible { get; set; }
        public List<FilterQuery> Filter { get; set; }
    }
}
