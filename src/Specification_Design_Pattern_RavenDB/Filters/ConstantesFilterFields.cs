namespace Specification_Design_Pattern_RavenDB.Filters
{
    public class ConstantesFilterFields
    {
        public static readonly string Operator_Equal = "EQ";
        public static readonly string Operator_NotEqual = "NE";
        public static readonly string Operator_LessThen = "LT";
        public static readonly string Operator_GreatThen = "GT";
        public static readonly string Operator_LessEqualThen = "LE";
        public static readonly string Operator_GreatEqualThen = "GE";
        public static readonly string Operator_Between = "BT";
        public static readonly string Operator_StartsWith = "SW";
        public static readonly string Operator_EndsWith = "EW";
        public static readonly string Operator_Contains = "CT";

        public static readonly Dictionary<string, string?> Regex = new Dictionary<string, string?>()
        {
            { Operator_Equal, "^=(.*)$" },
            { Operator_NotEqual, "^[!]?\\([!]?=(.*)\\)$" },
            { Operator_LessThen, "^<(.*)$" },
            { Operator_GreatThen, "^>(.*)$" },
            { Operator_LessEqualThen,"^<=(.*)$"  },
            { Operator_GreatEqualThen, "^>=(.*)$" },
            { Operator_Between, "^(.*)\\.\\.\\.(.*)$" },
            { Operator_StartsWith, null },
            { Operator_EndsWith, null },
            { Operator_Contains, null }
        };

        public static readonly Dictionary<string, string?> Formatos = new Dictionary<string, string?>()
        {
            { Operator_Equal, "={0}" },
            { Operator_NotEqual, "(!={0})" },
            { Operator_LessThen, "<{0}" },
            { Operator_GreatThen, ">{0}" },
            { Operator_LessEqualThen, "<={0}"  },
            { Operator_GreatEqualThen, ">={0}" },
            { Operator_Between, "{0}...{1}" },
            { Operator_StartsWith, "{0}" },
            { Operator_EndsWith, "{0}" },
            { Operator_Contains, "{0}" }
        };
    }
}
