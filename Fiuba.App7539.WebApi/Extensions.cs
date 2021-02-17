using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fiuba.App7539.WebApi
{
    public static class Extensions
    {
        public static string ToStringOrEmpty(this object obj)
        {
            return (obj != null) ? obj.ToString() : string.Empty;
        }

        public static string ToTrimOrEmpty(this string s)
        {
            return s.ToStringOrEmpty().Trim();
        }

        public static string WithoutAccents(this string str)
        {
            return str
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u")
                .Replace("ü", "u")
                .Replace("Á", "A")
                .Replace("É", "E")
                .Replace("Í", "I")
                .Replace("Ó", "O")
                .Replace("Ú", "U")
                .Replace("Ü", "U");
        }
    }
}