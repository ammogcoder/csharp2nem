using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;



namespace CSharp2nem.Utils
{
    public static class StringUtils
    {
        public static string ConvertToUnsecureString(SecureString securePassword)
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

        public static SecureString ToSecureString(string source)
        {
            if (source == null)
                throw new ArgumentException("key cannot be null");

            var result = new SecureString();

            foreach (var c in source.ToCharArray())
                result.AppendChar(c);
            return result;
        }

        public static bool OnlyHexInString(SecureString data)
        {
            if (null == data)
                throw new ArgumentNullException(nameof(data));
            return Regex.IsMatch(ConvertToUnsecureString(data), @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public static bool OnlyHexInString(string data)
        {
            if (null == data)
                throw new ArgumentNullException(nameof(data));
            return Regex.IsMatch(data, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public static string GetResultsWithHyphen(string input)
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

        public static string GetResultsWithoutHyphen(string input)
        {
            return input.Replace("-", "");
        }
    }
}