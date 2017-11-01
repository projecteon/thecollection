namespace TheCollection.Data.DocumentDB.Extensions {
    using System.Text.RegularExpressions;

    public static class SQLStringExtensions {
        public static string SQLSafeString(this string sWhere) {
            var sParsed = sWhere;

            // Remove SQL command words
            sParsed = Regex.Replace(sParsed, "\\b(SELECT|UPDATE|DELETE|CREATE|DROP|TRUNCATE|PRINT)\\b", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // remove SQL statement terminator
            sParsed = sParsed.Replace(";", "");

            // Encode SQL quote/apostrophe
            sParsed = sParsed.Replace("'", "''");

            // remove comment
            sParsed = sParsed.Replace("--", "");

            return sParsed;
        }

        public static string SQLReplaceLogicalOperators(this string sWhere) {
            var sParsed = sWhere;
            sParsed = sParsed.Replace("&", " AND ");
            sParsed = sParsed.Replace("|", " OR ");
            sParsed = sParsed.Replace("!", " NOT ");
            return sParsed;
        }

        public static string SQLReplaceWildCardOperator(this string sWhere) {
            return sWhere.Replace('*', '%');
        }
    }
}
