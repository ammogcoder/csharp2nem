using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public static class StringUtils
    {
        public static string ConvertToUnsecureString(this SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException(nameof(securePassword));

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static SecureString ToSecureString(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            var result = new SecureString();

            foreach (var c in source.ToCharArray())
                result.AppendChar(c);
            return result;
        }

        internal static bool OnlyHexInString(this SecureString data)
        {
            if (null == data)
                throw new ArgumentNullException(nameof(data));
            return Regex.IsMatch(ConvertToUnsecureString(data), @"\A\b[0-9a-fA-F]+\b\Z");
        }

        internal static bool OnlyHexInString(this string data)
        {
            if (null == data)
                throw new ArgumentNullException(nameof(data));
            return Regex.IsMatch(data, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public static string GetResultsWithHyphen(this string input)
        {
            var output = "";
            var start = 0;

            while (start < input.Length)
            {
                output += input.Substring(start, Math.Min(6, input.Length - start)) + "-";
                start += 6;
            }

            return output.Trim('-');
        }

        public static string GetResultsWithoutHyphen(this string input)
        {
            return input.Replace("-", "");
        }
    }
}