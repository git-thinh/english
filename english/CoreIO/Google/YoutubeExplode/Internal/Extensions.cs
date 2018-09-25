﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace YoutubeExplode.Internal
{
    internal static class Extensions
    {
        public static bool IsBlank(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotBlank(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static string SubstringUntil(this string str, string sub,
            StringComparison comparison = StringComparison.Ordinal)
        {
            var index = str.IndexOf(sub, comparison);
            return index < 0 ? str : str.Substring(0, index);
        }

        public static string SubstringAfter(this string str, string sub,
            StringComparison comparison = StringComparison.Ordinal)
        {
            var index = str.IndexOf(sub, comparison);
            return index < 0 ? string.Empty : str.Substring(index + sub.Length, str.Length - index - sub.Length);
        }

        public static string StripNonDigit(this string str)
        {
            return Regex.Replace(str, "\\D", "");
        }

        public static double ParseDouble(this string str)
        {
            const NumberStyles styles = NumberStyles.Float | NumberStyles.AllowThousands;
            var format = NumberFormatInfo.InvariantInfo;

            return double.Parse(str, styles, format);
        }

        public static double ParseDoubleOrDefault(this string str, double defaultValue = default(double))
        {
            const NumberStyles styles = NumberStyles.Float | NumberStyles.AllowThousands;
            var format = NumberFormatInfo.InvariantInfo;

            double result = 0;
            return double.TryParse(str, styles, format, out result)
                ? result
                : defaultValue;
        }

        public static int ParseInt(this string str)
        {
            const NumberStyles styles = NumberStyles.AllowThousands;
            var format = NumberFormatInfo.InvariantInfo;

            return int.Parse(str, styles, format);
        }

        public static int ParseIntOrDefault(this string str, int defaultValue = default(int))
        {
            const NumberStyles styles = NumberStyles.AllowThousands;
            var format = NumberFormatInfo.InvariantInfo;

            int result = 0;
            return int.TryParse(str, styles, format, out result) ? result : defaultValue;
        }

        public static long ParseLong(this string str)
        {
            const NumberStyles styles = NumberStyles.AllowThousands;
            var format = NumberFormatInfo.InvariantInfo;

            return long.Parse(str, styles, format);
        }

        public static long ParseLongOrDefault(this string str, long defaultValue = default(long))
        {
            const NumberStyles styles = NumberStyles.AllowThousands;
            var format = NumberFormatInfo.InvariantInfo;
            long result = 0;
            return long.TryParse(str, styles, format, out result)
                ? result
                : defaultValue;
        }

        //public static DateTimeOffset ParseDateTimeOffset(this string str)
        //{
        //    return DateTimeOffset.Parse(str, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal);
        //}

        public static DateTimeOffset ParseDateTimeOffset(this string str, string format = "MM/dd/yyyy")
        {
            string[] a = str.Split(new char[] { '/' });
            string mm = a[0], yy = a[a.Length - 1].Length == 2 ? "yy" : "yyyy";

            if (str.Length < 9) format = "M/d/" + yy;

            int mi = 0;
            if (int.TryParse(mm, out mi) && mi > 12)
            {
                if (str.Length < 9)
                    format = "d/M/" + yy;
                else
                    format = "dd/MM/" + yy;
            }

            return DateTimeOffset.ParseExact(str, format, DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.AssumeUniversal);
        }

        public static string Reverse(this string str)
        {
            var sb = new StringBuilder(str.Length);

            for (var i = str.Length - 1; i >= 0; i--)
                sb.Append(str[i]);

            return sb.ToString();
        }

        public static string UrlEncode(this string url)
        {
            return HttpUtility.UrlEncode(url);
        }

        public static string UrlDecode(this string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        public static string HtmlEncode(this string url)
        {
            return HttpUtility.HtmlEncode(url);
        }

        public static string HtmlDecode(this string url)
        {
            return HttpUtility.HtmlDecode(url);
        }

        public static string JoinToString<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable.Select(x => x.ToString()).ToArray());
        }

        public static string[] Split(this string input, params string[] separators)
        {
            return input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> enumerable,
            Func<TSource, TKey> selector)
        {
            var existing = new HashSet<TKey>();

            foreach (var element in enumerable)
            {
                if (existing.Add(selector(element)))
                    yield return element;
            }
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key,
            TValue defaultValue = default(TValue))
        {
            TValue result;
            return dic.TryGetValue(key, out result) == true ? result : defaultValue;
        }

        public static XElement StripNamespaces(this XElement element)
        {
            // Original code credit: http://stackoverflow.com/a/1147012

            var result = new XElement(element);
            foreach (var e in result.DescendantsAndSelf())
            {
                e.Name = XNamespace.None.GetName(e.Name.LocalName);
                var attributes = e.Attributes()
                    .Where(a => !a.IsNamespaceDeclaration)
                    .Where(a => a.Name.Namespace != XNamespace.Xml && a.Name.Namespace != XNamespace.Xmlns)
                    .Select(a => new XAttribute(XNamespace.None.GetName(a.Name.LocalName), a.Value));
                e.ReplaceAttributes(attributes);
            }

            return result;
        }

        public static string TextEx(this HtmlNode node)
        {
            if (node.NodeType == HtmlNodeType.Text)
                return node.InnerText;

            var sb = new StringBuilder();

            foreach (var child in node.ChildNodes)
                sb.Append(child.TextEx());

            if (node.Name == "BR")
                sb.AppendLine();

            return sb.ToString();
        }

        public static int CopyChunkToAsync(this Stream source, Stream destination,
            CancellationToken cancellationToken, int bufferSize = 81920)
        {
            var buffer = new byte[bufferSize];

            // Read
            //var bytesCopied = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
            var bytesCopied = source.Read(buffer, 0, buffer.Length);

            // Write
            //await destination.WriteAsync(buffer, 0, bytesCopied, cancellationToken).ConfigureAwait(false);
            destination.Write(buffer, 0, bytesCopied);

            return bytesCopied;
        }

        public static void CopyToAsync(this Stream source, Stream destination,
            //IProgress<double> progress = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var totalBytesCopied = 0L;
            int bytesCopied;
            do
            {
                // Copy
                bytesCopied = source.CopyChunkToAsync(destination, cancellationToken); //.ConfigureAwait(false);

                // Report progress
                totalBytesCopied += bytesCopied;
                //progress?.Report(1.0 * totalBytesCopied / source.Length);
            } while (bytesCopied > 0);
        }
    }
}