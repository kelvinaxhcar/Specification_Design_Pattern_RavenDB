using System.Text.RegularExpressions;
using System.Collections.Generic;
using RavenDB.Specifications;

namespace RavenDB.Specifications
{
    public static class SqlToSpecificationConverter
    {
        public static Specification<T> Convert<T>(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return null;

            sql = sql.Trim();
            if (sql.StartsWith("WHERE ", StringComparison.OrdinalIgnoreCase))
                sql = sql.Substring(6).Trim();

            return ParseExpression<T>(sql);
        }

        private static Specification<T> ParseExpression<T>(string expression)
        {
            expression = expression.Trim();

            // Handle OR (lowest precedence)
            var partsOr = SplitByTopLevelOperator(expression, "OR");
            if (partsOr.Length > 1)
            {
                var spec = ParseExpression<T>(partsOr[0]);
                for (int i = 1; i < partsOr.Length; i++)
                {
                    spec = spec.Or(ParseExpression<T>(partsOr[i]));
                }
                return spec;
            }

            // Handle AND
            var partsAnd = SplitByTopLevelOperator(expression, "AND");
            if (partsAnd.Length > 1)
            {
                var spec = ParseExpression<T>(partsAnd[0]);
                for (int i = 1; i < partsAnd.Length; i++)
                {
                    spec = spec.And(ParseExpression<T>(partsAnd[i]));
                }
                return spec;
            }

            // Handle NOT
            if (expression.StartsWith("NOT ", StringComparison.OrdinalIgnoreCase))
            {
                return ParseExpression<T>(expression.Substring(4)).Not();
            }

            // Handle Parentheses
            if (expression.StartsWith("(") && expression.EndsWith(")"))
            {
                // Verify if it's a single set of parentheses or multiple
                if (IsMatchingParentheses(expression))
                {
                    return ParseExpression<T>(expression.Substring(1, expression.Length - 2));
                }
            }

            // Handle Leaf conditions
            return ParseLeafCondition<T>(expression);
        }

        private static bool IsMatchingParentheses(string expression)
        {
            int level = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(') level++;
                else if (expression[i] == ')') level--;
                
                if (level == 0 && i < expression.Length - 1)
                    return false;
            }
            return level == 0;
        }

        private static string[] SplitByTopLevelOperator(string expression, string op)
        {
            var results = new List<string>();
            int level = 0;
            int start = 0;
            string pattern = $@"\s+{op}\s+";

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(') level++;
                else if (expression[i] == ')') level--;
                else if (level == 0)
                {
                    var remaining = expression.Substring(i);
                    var match = Regex.Match(remaining, pattern, RegexOptions.IgnoreCase);
                    if (match.Success && match.Index == 0)
                    {
                        results.Add(expression.Substring(start, i - start));
                        i += match.Length - 1;
                        start = i + 1;
                    }
                }
            }
            results.Add(expression.Substring(start));
            return results.ToArray();
        }

        private static Specification<T> ParseLeafCondition<T>(string leaf)
        {
            leaf = leaf.Trim();

            // Support IN separately
            var inMatch = Regex.Match(leaf, @"(\w+)\s+IN\s*\((.*)\)", RegexOptions.IgnoreCase);
            if (inMatch.Success)
            {
                string propertyNameIn = inMatch.Groups[1].Value.Trim();
                string listStr = inMatch.Groups[2].Value.Trim();
                var values = ParseValueList(listStr);
                return new InSpecification<T>(propertyNameIn, values);
            }
            
            // Regex for Field Operator Value
            // Operators: =, !=, <>, >, <, >=, <=, LIKE
            var match = Regex.Match(leaf, @"(\w+)\s*(=|!=|<>|>=|<=|>|<|LIKE)\s*(.*)", RegexOptions.IgnoreCase);
            if (!match.Success)
                throw new ArgumentException($"Invalid condition format: {leaf}");

            string propertyNameLeaf = match.Groups[1].Value.Trim();
            string op = match.Groups[2].Value.ToUpper().Trim();
            string value = match.Groups[3].Value.Trim();

            value = StripQuotes(value);

            return op switch
            {
                "=" => new EqualitySpecification<T>(propertyNameLeaf, value),
                "!=" or "<>" => new NotEqualSpecification<T>(propertyNameLeaf, value),
                ">" => new GreaterThanSpecification<T>(propertyNameLeaf, value),
                "<" => new LessThanSpecification<T>(propertyNameLeaf, value),
                ">=" => new GreaterThanOrEqualSpecification<T>(propertyNameLeaf, value),
                "<=" => new LessThanOrEqualSpecification<T>(propertyNameLeaf, value),
                "LIKE" => new StartsWithSpecification<T>(propertyNameLeaf, value.Replace("%", "")),
                _ => throw new NotSupportedException($"Operator {op} is not supported.")
            };
        }

        private static IEnumerable<string> ParseValueList(string listStr)
        {
            // Simple split by comma, respecting quotes
            var values = new List<string>();
            var current = "";
            bool inQuotes = false;
            char quoteChar = '\0';

            for (int i = 0; i < listStr.Length; i++)
            {
                char c = listStr[i];
                if ((c == '\'' || c == '"') && (!inQuotes || c == quoteChar))
                {
                    inQuotes = !inQuotes;
                    quoteChar = inQuotes ? c : '\0';
                }
                else if (c == ',' && !inQuotes)
                {
                    values.Add(StripQuotes(current.Trim()));
                    current = "";
                }
                else
                {
                    current += c;
                }
            }
            if (!string.IsNullOrWhiteSpace(current))
                values.Add(StripQuotes(current.Trim()));

            return values;
        }

        private static string StripQuotes(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            if ((value.StartsWith("'") && value.EndsWith("'")) || (value.StartsWith("\"") && value.EndsWith("\"")))
            {
                return value.Substring(1, value.Length - 2);
            }
            return value;
        }
    }
}
