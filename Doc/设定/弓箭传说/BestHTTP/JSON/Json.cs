namespace BestHTTP.JSON
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    public class Json
    {
        private const int TOKEN_NONE = 0;
        private const int TOKEN_CURLY_OPEN = 1;
        private const int TOKEN_CURLY_CLOSE = 2;
        private const int TOKEN_SQUARED_OPEN = 3;
        private const int TOKEN_SQUARED_CLOSE = 4;
        private const int TOKEN_COLON = 5;
        private const int TOKEN_COMMA = 6;
        private const int TOKEN_STRING = 7;
        private const int TOKEN_NUMBER = 8;
        private const int TOKEN_TRUE = 9;
        private const int TOKEN_FALSE = 10;
        private const int TOKEN_NULL = 11;
        private const int BUILDER_CAPACITY = 0x7d0;

        public static object Decode(string json)
        {
            bool success = true;
            return Decode(json, ref success);
        }

        public static object Decode(string json, ref bool success)
        {
            success = true;
            if (json != null)
            {
                char[] chArray = json.ToCharArray();
                int index = 0;
                return ParseValue(chArray, ref index, ref success);
            }
            return null;
        }

        protected static void EatWhitespace(char[] json, ref int index)
        {
            while (index < json.Length)
            {
                if (" \t\n\r".IndexOf(json[index]) == -1)
                {
                    break;
                }
                index++;
            }
        }

        public static string Encode(object json)
        {
            StringBuilder builder = new StringBuilder(0x7d0);
            return (!SerializeValue(json, builder) ? null : builder.ToString());
        }

        protected static int GetLastIndexOfNumber(char[] json, int index)
        {
            int num = index;
            while (num < json.Length)
            {
                if ("0123456789+-.eE".IndexOf(json[num]) == -1)
                {
                    break;
                }
                num++;
            }
            return (num - 1);
        }

        protected static int LookAhead(char[] json, int index)
        {
            int num = index;
            return NextToken(json, ref num);
        }

        protected static int NextToken(char[] json, ref int index)
        {
            EatWhitespace(json, ref index);
            if (index != json.Length)
            {
                char ch = json[index];
                index++;
                switch (ch)
                {
                    case ',':
                        return 6;

                    case '-':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        return 8;

                    case ':':
                        return 5;

                    case '[':
                        return 3;

                    case ']':
                        return 4;

                    case '{':
                        return 1;

                    case '}':
                        return 2;

                    case '"':
                        return 7;
                }
                index--;
                int num = json.Length - index;
                if ((((num >= 5) && (json[index] == 'f')) && ((json[index + 1] == 'a') && (json[index + 2] == 'l'))) && ((json[index + 3] == 's') && (json[index + 4] == 'e')))
                {
                    index += 5;
                    return 10;
                }
                if ((((num >= 4) && (json[index] == 't')) && ((json[index + 1] == 'r') && (json[index + 2] == 'u'))) && (json[index + 3] == 'e'))
                {
                    index += 4;
                    return 9;
                }
                if ((((num >= 4) && (json[index] == 'n')) && ((json[index + 1] == 'u') && (json[index + 2] == 'l'))) && (json[index + 3] == 'l'))
                {
                    index += 4;
                    return 11;
                }
            }
            return 0;
        }

        protected static List<object> ParseArray(char[] json, ref int index, ref bool success)
        {
            List<object> list = new List<object>();
            NextToken(json, ref index);
            bool flag = false;
            while (!flag)
            {
                int num = LookAhead(json, index);
                if (num == 0)
                {
                    success = false;
                    return null;
                }
                if (num == 6)
                {
                    NextToken(json, ref index);
                }
                else
                {
                    if (num == 4)
                    {
                        NextToken(json, ref index);
                        return list;
                    }
                    object item = ParseValue(json, ref index, ref success);
                    if (!success)
                    {
                        return null;
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        protected static double ParseNumber(char[] json, ref int index, ref bool success)
        {
            EatWhitespace(json, ref index);
            int lastIndexOfNumber = GetLastIndexOfNumber(json, index);
            int length = (lastIndexOfNumber - index) + 1;
            success = double.TryParse(new string(json, index, length), NumberStyles.Any, CultureInfo.InvariantCulture, out double num3);
            index = lastIndexOfNumber + 1;
            return num3;
        }

        protected static Dictionary<string, object> ParseObject(char[] json, ref int index, ref bool success)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            NextToken(json, ref index);
            bool flag = false;
            while (!flag)
            {
                switch (LookAhead(json, index))
                {
                    case 0:
                        success = false;
                        return null;

                    case 6:
                    {
                        NextToken(json, ref index);
                        continue;
                    }
                    case 2:
                        NextToken(json, ref index);
                        return dictionary;
                }
                string str = ParseString(json, ref index, ref success);
                if (!success)
                {
                    success = false;
                    return null;
                }
                if (NextToken(json, ref index) != 5)
                {
                    success = false;
                    return null;
                }
                object obj2 = ParseValue(json, ref index, ref success);
                if (!success)
                {
                    success = false;
                    return null;
                }
                dictionary[str] = obj2;
            }
            return dictionary;
        }

        protected static string ParseString(char[] json, ref int index, ref bool success)
        {
            StringBuilder builder = new StringBuilder(0x7d0);
            EatWhitespace(json, ref index);
            char ch = json[index++];
            bool flag = false;
            while (!flag)
            {
                if (index == json.Length)
                {
                    break;
                }
                ch = json[index++];
                if (ch == '"')
                {
                    flag = true;
                    break;
                }
                if (ch == '\\')
                {
                    if (index == json.Length)
                    {
                        break;
                    }
                    ch = json[index++];
                    if (ch == '"')
                    {
                        builder.Append('"');
                    }
                    else
                    {
                        if (ch == '\\')
                        {
                            builder.Append('\\');
                            continue;
                        }
                        if (ch == '/')
                        {
                            builder.Append('/');
                            continue;
                        }
                        if (ch == 'b')
                        {
                            builder.Append('\b');
                            continue;
                        }
                        if (ch == 'f')
                        {
                            builder.Append('\f');
                            continue;
                        }
                        if (ch == 'n')
                        {
                            builder.Append('\n');
                            continue;
                        }
                        if (ch == 'r')
                        {
                            builder.Append('\r');
                            continue;
                        }
                        if (ch == 't')
                        {
                            builder.Append('\t');
                        }
                        else if (ch == 'u')
                        {
                            int num2 = json.Length - index;
                            if (num2 < 4)
                            {
                                break;
                            }
                            if (!(success = uint.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint num3)))
                            {
                                return string.Empty;
                            }
                            builder.Append(char.ConvertFromUtf32((int) num3));
                            index += 4;
                        }
                    }
                }
                else
                {
                    builder.Append(ch);
                }
            }
            if (!flag)
            {
                success = false;
                return null;
            }
            return builder.ToString();
        }

        protected static object ParseValue(char[] json, ref int index, ref bool success)
        {
            switch (LookAhead(json, index))
            {
                case 1:
                    return ParseObject(json, ref index, ref success);

                case 3:
                    return ParseArray(json, ref index, ref success);

                case 7:
                    return ParseString(json, ref index, ref success);

                case 8:
                    return ParseNumber(json, ref index, ref success);

                case 9:
                    NextToken(json, ref index);
                    return true;

                case 10:
                    NextToken(json, ref index);
                    return false;

                case 11:
                    NextToken(json, ref index);
                    return null;
            }
            success = false;
            return null;
        }

        protected static bool SerializeArray(IList anArray, StringBuilder builder)
        {
            builder.Append("[");
            bool flag = true;
            for (int i = 0; i < anArray.Count; i++)
            {
                object obj2 = anArray[i];
                if (!flag)
                {
                    builder.Append(", ");
                }
                if (!SerializeValue(obj2, builder))
                {
                    return false;
                }
                flag = false;
            }
            builder.Append("]");
            return true;
        }

        protected static bool SerializeNumber(double number, StringBuilder builder)
        {
            builder.Append(Convert.ToString(number, CultureInfo.InvariantCulture));
            return true;
        }

        protected static bool SerializeObject(IDictionary anObject, StringBuilder builder)
        {
            builder.Append("{");
            IDictionaryEnumerator enumerator = anObject.GetEnumerator();
            for (bool flag = true; enumerator.MoveNext(); flag = false)
            {
                string aString = enumerator.Key.ToString();
                object obj2 = enumerator.Value;
                if (!flag)
                {
                    builder.Append(", ");
                }
                SerializeString(aString, builder);
                builder.Append(":");
                if (!SerializeValue(obj2, builder))
                {
                    return false;
                }
            }
            builder.Append("}");
            return true;
        }

        protected static bool SerializeString(string aString, StringBuilder builder)
        {
            builder.Append("\"");
            foreach (char ch in aString.ToCharArray())
            {
                switch (ch)
                {
                    case '"':
                        builder.Append("\\\"");
                        break;

                    case '\\':
                        builder.Append(@"\\");
                        break;

                    case '\b':
                        builder.Append(@"\b");
                        break;

                    case '\f':
                        builder.Append(@"\f");
                        break;

                    case '\n':
                        builder.Append(@"\n");
                        break;

                    case '\r':
                        builder.Append(@"\r");
                        break;

                    case '\t':
                        builder.Append(@"\t");
                        break;

                    default:
                    {
                        int num2 = Convert.ToInt32(ch);
                        if ((num2 >= 0x20) && (num2 <= 0x7e))
                        {
                            builder.Append(ch);
                        }
                        else
                        {
                            builder.Append(@"\u" + Convert.ToString(num2, 0x10).PadLeft(4, '0'));
                        }
                        break;
                    }
                }
            }
            builder.Append("\"");
            return true;
        }

        protected static bool SerializeValue(object value, StringBuilder builder)
        {
            bool flag = true;
            switch (value)
            {
                case (string _):
                    return SerializeString((string) value, builder);
                    break;
            }
            if (value is IDictionary)
            {
                return SerializeObject((IDictionary) value, builder);
            }
            if (value is IList)
            {
                return SerializeArray(value as IList, builder);
            }
            if ((value is bool) && ((bool) value))
            {
                builder.Append("true");
                return flag;
            }
            if ((value is bool) && !((bool) value))
            {
                builder.Append("false");
                return flag;
            }
            if (value is ValueType)
            {
                return SerializeNumber(Convert.ToDouble(value), builder);
            }
            if (value == null)
            {
                builder.Append("null");
                return flag;
            }
            return false;
        }
    }
}

