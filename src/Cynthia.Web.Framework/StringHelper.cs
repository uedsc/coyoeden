/// Author:					Joe Audette
/// Created:				2007-11-20
/// Last Modified:			2010-01-26
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Cynthia.Web.Framework
{
    
    public static class StringHelper
    {

        public static bool IsCaseInsensitiveMatch(string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase);
            //return (string.Compare(str1, str2, false, CultureInfo.InvariantCulture) == 0);

        }

        public static string ToJsonString(object jsonObj)
        {
            return new JavaScriptSerializer().Serialize(jsonObj);
        }

        //private static string SafeJson(string sIn)
        //{
        //    StringBuilder sbOut = new StringBuilder(sIn.Length);
        //    foreach (char ch in sIn)
        //    {
        //        if (Char.IsControl(ch) || ch == '\'')
        //        {
        //            int ich = (int)ch;
        //            sbOut.Append(@"\u" + ich.ToString("x4"));
        //            continue;
        //        }
        //        else if (ch == '\"' || ch == '\\' || ch == '/')
        //        {
        //            sbOut.Append('\\');
        //        }
        //        sbOut.Append(ch);
        //    }
        //    return sbOut.ToString();
        //}

        
        /////  FUNCTION Enquote Public Domain 2002 JSON.org
        /////  @author JSON.org
        /////  @version 0.1
        /////  Ported to C# by Are Bjolseth, teleplan.no
        //public static string JsonEncode(this string s)
        //{
        //    if (s == null || s.Length == 0)
        //    {
        //        //return "\"\"";
        //        return s;
        //    }
        //    char c;
        //    int i;
        //    int len = s.Length;
        //    StringBuilder sb = new StringBuilder(len + 4);
        //    string t;

        //    sb.Append('"');
        //    for (i = 0; i < len; i += 1)
        //    {
        //        c = s[i];
        //        if ((c == '\\') || (c == '"') || (c == '>'))
        //        {
        //            sb.Append('\\');
        //            sb.Append(c);
        //        }
        //        else if (c == '\b')
        //            sb.Append("\\b");
        //        else if (c == '\t')
        //            sb.Append("\\t");
        //        else if (c == '\n')
        //            sb.Append("\\n");
        //        else if (c == '\f')
        //            sb.Append("\\f");
        //        else if (c == '\r')
        //            sb.Append("\\r");
        //        else
        //        {
        //            if (c < ' ')
        //            {
        //                //t = "000" + Integer.toHexString(c);
        //                string tmp = new string(c, 1);
        //                t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
        //                sb.Append("\\u" + t.Substring(t.Length - 4));
        //            }
        //            else
        //            {
        //                sb.Append(c);
        //            }
        //        }
        //    }
        //    sb.Append('"');
        //    return sb.ToString();
        //}


        //private static string EscapHtmlToJson(string html)
        //{
        //    if(string.IsNullOrEmpty(html)){ return html;}

        //    return "'" + html.Replace("</", "' + '</' + '").Replace("<", "' + '<' + '").Replace(">", "' + '>' + '").Replace("+ '\"}","}");

        //}

        public static string DecodeBase64String(string base64String, Encoding encoding)
        {
            if (string.IsNullOrEmpty(base64String)) { return base64String; }
            byte[] encodedBytes = Convert.FromBase64String(base64String);
            return encoding.GetString(encodedBytes, 0, encodedBytes.Length);

        }



        #region string Extension Methods


        //public static string JsonEscapeHtml(this string s)
        //{
        //    if (string.IsNullOrEmpty(s)) { return s; }

        //    return EscapHtmlToJson(s);

        //}


        public static string HtmlEscapeQuotes(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            return s.Replace("'","&#39;").Replace("\"","&#34;");

        }

        public static string RemoveAngleBrackets(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            return s.Replace("<", string.Empty).Replace(">", string.Empty);

        }

        public static string Coalesce(this string s, string alt)
        {
            if (string.IsNullOrEmpty(s)) { return alt; }
            return s;
        }

        /// <summary>
        /// Converts a unicode string into its closest ascii equivalent
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToAscii(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            
            try
            {
                
                string normalized = s.Normalize(NormalizationForm.FormKD);

                Encoding ascii = Encoding.GetEncoding(
                      "us-ascii",
                      new EncoderReplacementFallback(string.Empty),
                      new DecoderReplacementFallback(string.Empty));

                byte[] encodedBytes = new byte[ascii.GetByteCount(normalized)];
                int numberOfEncodedBytes = ascii.GetBytes(normalized, 0, normalized.Length,
                encodedBytes, 0);

                string newString = ascii.GetString(encodedBytes);

                return newString;

                //Encoding ascii = Encoding.ASCII;
                //Encoding unicode = Encoding.Unicode;
                //byte[] unicodeBytes = unicode.GetBytes(s);
                //byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

                //// Convert the new byte[] into a char[] and then into a string.
                //// This is a slightly different approach to converting to illustrate
                //// the use of GetCharCount/GetChars.
                //char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
                //ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
                //string asciiString = new string(asciiChars);
                //return asciiString;
            }
            catch
            {
                return s;
            }


        }

        /// <summary>
        /// Converts a unicode string into its closest ascii equivalent.
        /// If the ascii encode string length is less than or equal to 1 returns the original string
        /// as this means the string is probably i a language with no ascii equivalents
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToAsciiIfPossible(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            try
            {

                string normalized = s.Normalize(NormalizationForm.FormKD);

                Encoding ascii = Encoding.GetEncoding(
                      "us-ascii",
                      new EncoderReplacementFallback(string.Empty),
                      new DecoderReplacementFallback(string.Empty));

                byte[] encodedBytes = new byte[ascii.GetByteCount(normalized)];
                int numberOfEncodedBytes = ascii.GetBytes(normalized, 0, normalized.Length,
                encodedBytes, 0);

                if (numberOfEncodedBytes <= 1) { return s; } // wasn't able to get ascii equivalent chars

                string newString = ascii.GetString(encodedBytes);

                return newString;

            }
            catch
            {
                return s;
            }


        }

        public static string RemoveNonNumeric(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            char[] result = new char[s.Length];
            int resultIndex = 0;
            foreach (char c in s)
            {
                if (char.IsNumber(c))
                    result[resultIndex++] = c;
            }
            if (0 == resultIndex)
                s = string.Empty;
            else if (result.Length != resultIndex)
                s = new string(result, 0, resultIndex);
            
            return s;
        }

        public static string RemoveLineBreaks(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            return s.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty);
        }


        public static string EscapeXml(this string s)
        {
            string xml = s;
            if (!string.IsNullOrEmpty(xml))
            {
                // replace literal values with entities
                xml = xml.Replace("&", "&amp;");
                xml = xml.Replace("&lt;", "&lt;");
                xml = xml.Replace("&gt;", "&gt;");
                xml = xml.Replace("\"", "&quot;");
                xml = xml.Replace("'", "&apos;");
            }
            return xml;
        }

        public static string UnescapeXml(this string s)
        {
            string unxml = s;
            if (!string.IsNullOrEmpty(unxml))
            {
                // replace entities with literal values
                unxml = unxml.Replace("&apos;", "'");
                unxml = unxml.Replace("&quot;", "\"");
                unxml = unxml.Replace("&gt;", "&gt;");
                unxml = unxml.Replace("&lt;", "&lt;");
                unxml = unxml.Replace("&amp;", "&");
            }
            return unxml;
        }


        public static List<string> SplitOnChar(this string s, char c)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(s)) { return list; }

            string[] a = s.Split(c);
            foreach (string item in a)
            {
                if (!string.IsNullOrEmpty(item)) { list.Add(item); }
            }


            return list;
        }

        public static List<string> SplitOnPipes(string s)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(s)) { return list; }

            string[] a = s.Split('|');
            foreach (string item in a)
            {
                if (!string.IsNullOrEmpty(item)) { list.Add(item); }
            }


            return list;
        }

        #endregion

    }
}
