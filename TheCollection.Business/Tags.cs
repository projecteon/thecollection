﻿using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TheCollection.Business
{
    public static class Tags
    {

        private static string[] StopWords { get { return new[] { "as", "at", "the", "was" }; } }
        private static string[] replacement = { "a", "a", "a", "a", "a", "a", "c", "e", "e", "e", "e", "i", "i", "i", "i", "n", "o", "o", "o", "o", "o", "ss", "u", "u", "u", "u", "y", "y" };
        private static char[] accents = { 'à', 'á', 'â', 'ã', 'ä', 'å', 'ç', 'é', 'è', 'ê', 'ë', 'ì', 'í', 'î', 'ï', 'ñ', 'ò', 'ó', 'ô', 'ö', 'õ', 'ß', 'ù', 'ú', 'û', 'ü', 'ý', 'ÿ' };


        public static string[] Generate(string tagString)
        {
            var clean = StripAccents(tagString.ToLower());
            clean = Regex.Replace(clean, @"[^-&a-z0-9\s]", string.Empty);
            var stopWords = new Regex($"^({string.Join("|", StopWords)})$");
            return clean.Split(' ').Where(x => String.IsNullOrWhiteSpace(x) == false && !stopWords.IsMatch(x.Trim())).Distinct().ToArray();
        }

        public static string StripAccents(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s.ToCharArray())
            {
                var index = Array.IndexOf(accents, c);
                if (index > 0)
                {
                    sb.Append(replacement[index]);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}