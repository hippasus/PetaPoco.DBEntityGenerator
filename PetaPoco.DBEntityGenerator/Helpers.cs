namespace PetaPoco.DBEntityGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class Helpers
    {
        static Regex rxCleanUp = new Regex(@"[^\w\d_]", RegexOptions.Compiled);

        static string[] cs_keywords = { "abstract", "event", "new", "struct", "as", "explicit", "null",
            "switch", "base", "extern", "object", "this", "bool", "false", "operator", "throw",
            "break", "finally", "out", "true", "byte", "fixed", "override", "try", "case", "float",
            "params", "typeof", "catch", "for", "private", "uint", "char", "foreach", "protected",
            "ulong", "checked", "goto", "public", "unchecked", "class", "if", "readonly", "unsafe",
            "const", "implicit", "ref", "ushort", "continue", "in", "return", "using", "decimal",
            "int", "sbyte", "virtual", "default", "interface", "sealed", "volatile", "delegate",
            "internal", "short", "void", "do", "is", "sizeof", "while", "double", "lock",
            "stackalloc", "else", "long", "static", "enum", "namespace", "string" };

        internal static Func<string, string> CleanUp = (str) =>
        {
            str = rxCleanUp.Replace(str, "_");

            if (char.IsDigit(str[0]) || cs_keywords.Contains(str))
                str = "@" + str;

            return str;
        };

        internal static string CheckNullable(Column col)
        {
            string result = "";
            if (col.IsNullable &&
                col.PropertyType != "byte[]" &&
                col.PropertyType != "string" &&
                col.PropertyType != "Microsoft.SqlServer.Types.SqlGeography" &&
                col.PropertyType != "Microsoft.SqlServer.Types.SqlGeometry"
                )
                result = "?";

            return result;
        }

        internal static string zap_password(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return string.Empty;
            }

            var rx = new Regex("password=.*;", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return rx.Replace(connectionString, "password=**zapped**;");
        }

        internal static bool StartsWithAny(string s, IEnumerable<string> items)
        {
            if (s == null || items == null)
            {
                return false;
            }

            return items.Any(i => s.StartsWith(i));
        }
    }
}
