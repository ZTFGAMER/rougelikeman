namespace BestHTTP.Extensions
{
    using BestHTTP;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;

    public static class Extensions
    {
        [CompilerGenerated]
        private static Func<char, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<char, bool> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<char, bool> <>f__am$cache2;

        public static string AsciiToString(this byte[] bytes)
        {
            StringBuilder builder = new StringBuilder(bytes.Length);
            foreach (byte num in bytes)
            {
                builder.Append((num > 0x7f) ? '?' : ((char) num));
            }
            return builder.ToString();
        }

        public static string CalculateMD5Hash(this string input) => 
            input.GetASCIIBytes().CalculateMD5Hash();

        public static string CalculateMD5Hash(this byte[] input)
        {
            byte[] buffer = MD5.Create().ComputeHash(input);
            StringBuilder builder = new StringBuilder();
            foreach (byte num in buffer)
            {
                builder.Append(num.ToString("x2"));
            }
            return builder.ToString();
        }

        public static string[] FindOption(this string str, string option)
        {
            char[] separator = new char[] { ',' };
            string[] strArray = str.ToLower().Split(separator, StringSplitOptions.RemoveEmptyEntries);
            option = option.ToLower();
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].Contains(option))
                {
                    char[] chArray2 = new char[] { '=' };
                    return strArray[i].Split(chArray2, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            return null;
        }

        public static byte[] GetASCIIBytes(this string str)
        {
            byte[] buffer = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                buffer[i] = (ch >= '\x0080') ? ((byte) 0x3f) : ((byte) ch);
            }
            return buffer;
        }

        public static string GetRequestPathAndQueryURL(this Uri uri)
        {
            string components = uri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped);
            if (string.IsNullOrEmpty(components))
            {
                components = "/";
            }
            return components;
        }

        internal static List<HeaderValue> ParseOptionalHeader(this string str)
        {
            List<HeaderValue> list = new List<HeaderValue>();
            if (str != null)
            {
                int pos = 0;
                while (pos < str.Length)
                {
                    if (<>f__am$cache1 == null)
                    {
                        <>f__am$cache1 = ch => (ch != '=') && (ch != ',');
                    }
                    HeaderValue item = new HeaderValue(str.Read(ref pos, <>f__am$cache1, true).TrimAndLower());
                    if (str[pos - 1] == '=')
                    {
                        item.Value = str.ReadPossibleQuotedText(ref pos);
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        internal static List<HeaderValue> ParseQualityParams(this string str)
        {
            List<HeaderValue> list = new List<HeaderValue>();
            if (str != null)
            {
                int pos = 0;
                while (pos < str.Length)
                {
                    if (<>f__am$cache2 == null)
                    {
                        <>f__am$cache2 = ch => (ch != ',') && (ch != ';');
                    }
                    HeaderValue item = new HeaderValue(str.Read(ref pos, <>f__am$cache2, true).TrimAndLower());
                    if (str[pos - 1] == ';')
                    {
                        str.Read(ref pos, '=', false);
                        item.Value = str.Read(ref pos, ',', true);
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        internal static char? Peek(this string str, int pos)
        {
            if ((pos >= 0) && (pos < str.Length))
            {
                return new char?(str[pos]);
            }
            return null;
        }

        internal static string Read(this string str, ref int pos, char block, bool needResult = true)
        {
            <Read>c__AnonStorey0 storey = new <Read>c__AnonStorey0 {
                block = block
            };
            return str.Read(ref pos, new Func<char, bool>(storey.<>m__0), needResult);
        }

        internal static string Read(this string str, ref int pos, Func<char, bool> block, bool needResult = true)
        {
            if (pos >= str.Length)
            {
                return string.Empty;
            }
            str.SkipWhiteSpace(ref pos);
            int startIndex = pos;
            while ((pos < str.Length) && block(str[pos]))
            {
                pos++;
            }
            string str2 = !needResult ? null : str.Substring(startIndex, pos - startIndex);
            pos++;
            return str2;
        }

        public static void ReadBuffer(this Stream stream, byte[] buffer)
        {
            int offset = 0;
            do
            {
                int num2 = stream.Read(buffer, offset, buffer.Length - offset);
                if (num2 <= 0)
                {
                    throw ExceptionHelper.ServerClosedTCPStream();
                }
                offset += num2;
            }
            while (offset < buffer.Length);
        }

        internal static string ReadPossibleQuotedText(this string str, ref int pos)
        {
            string str2 = string.Empty;
            if (str == null)
            {
                return str2;
            }
            if (str[pos] == '"')
            {
                str.Read(ref pos, '"', false);
                str2 = str.Read(ref pos, '"', true);
                str.Read(ref pos, ',', false);
                return str2;
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = ch => (ch != ',') && (ch != ';');
            }
            return str.Read(ref pos, <>f__am$cache0, true);
        }

        public static void SendAsASCII(this BinaryWriter stream, string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                stream.Write((ch >= '\x0080') ? ((byte) 0x3f) : ((byte) ch));
            }
        }

        internal static void SkipWhiteSpace(this string str, ref int pos)
        {
            if (pos < str.Length)
            {
                while ((pos < str.Length) && char.IsWhiteSpace(str[pos]))
                {
                    pos++;
                }
            }
        }

        public static DateTime ToDateTime(this string str, DateTime defaultValue = new DateTime())
        {
            if (str == null)
            {
                return defaultValue;
            }
            try
            {
                DateTime.TryParse(str, out defaultValue);
                return defaultValue.ToUniversalTime();
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int ToInt32(this string str, int defaultValue = 0)
        {
            if (str == null)
            {
                return defaultValue;
            }
            try
            {
                return int.Parse(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long ToInt64(this string str, long defaultValue = 0L)
        {
            if (str == null)
            {
                return defaultValue;
            }
            try
            {
                return long.Parse(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ToStrOrEmpty(this string str)
        {
            if (str == null)
            {
                return string.Empty;
            }
            return str;
        }

        internal static string TrimAndLower(this string str)
        {
            if (str == null)
            {
                return null;
            }
            char[] chArray = new char[str.Length];
            int length = 0;
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (!char.IsWhiteSpace(c) && !char.IsControl(c))
                {
                    chArray[length++] = char.ToLowerInvariant(c);
                }
            }
            return new string(chArray, 0, length);
        }

        public static void WriteAll(this MemoryStream ms, byte[] buffer)
        {
            ms.Write(buffer, 0, buffer.Length);
        }

        public static void WriteArray(this Stream stream, byte[] array)
        {
            stream.Write(array, 0, array.Length);
        }

        public static void WriteLine(this FileStream fs)
        {
            fs.Write(HTTPRequest.EOL, 0, 2);
        }

        public static void WriteLine(this MemoryStream ms)
        {
            ms.WriteAll(HTTPRequest.EOL);
        }

        public static void WriteLine(this FileStream fs, string line)
        {
            byte[] aSCIIBytes = line.GetASCIIBytes();
            fs.Write(aSCIIBytes, 0, aSCIIBytes.Length);
            fs.WriteLine();
        }

        public static void WriteLine(this MemoryStream ms, string str)
        {
            ms.WriteString(str);
            ms.WriteLine();
        }

        public static void WriteLine(this FileStream fs, string format, params object[] values)
        {
            byte[] aSCIIBytes = string.Format(format, values).GetASCIIBytes();
            fs.Write(aSCIIBytes, 0, aSCIIBytes.Length);
            fs.WriteLine();
        }

        public static void WriteString(this MemoryStream ms, string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            ms.WriteAll(bytes);
        }

        [CompilerGenerated]
        private sealed class <Read>c__AnonStorey0
        {
            internal char block;

            internal bool <>m__0(char ch) => 
                (ch != this.block);
        }
    }
}

