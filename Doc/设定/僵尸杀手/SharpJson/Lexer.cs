namespace SharpJson
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal class Lexer
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <lineNumber>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <parseNumbersAsFloat>k__BackingField;
        private char[] json;
        private int index;
        private bool success = true;
        private char[] stringBuffer = new char[0x1000];

        public Lexer(string text)
        {
            this.Reset();
            this.json = text.ToCharArray();
            this.parseNumbersAsFloat = false;
        }

        private int GetLastIndexOfNumber(int index)
        {
            int num = index;
            while (num < this.json.Length)
            {
                char ch = this.json[num];
                if (((ch < '0') || (ch > '9')) && ((((ch != '+') && (ch != '-')) && ((ch != '.') && (ch != 'e'))) && (ch != 'E')))
                {
                    break;
                }
                num++;
            }
            return (num - 1);
        }

        private string GetNumberString()
        {
            this.SkipWhiteSpaces();
            int lastIndexOfNumber = this.GetLastIndexOfNumber(this.index);
            int length = (lastIndexOfNumber - this.index) + 1;
            string str = new string(this.json, this.index, length);
            this.index = lastIndexOfNumber + 1;
            return str;
        }

        public Token LookAhead()
        {
            this.SkipWhiteSpaces();
            int index = this.index;
            return NextToken(this.json, ref index);
        }

        public Token NextToken()
        {
            this.SkipWhiteSpaces();
            return NextToken(this.json, ref this.index);
        }

        private static Token NextToken(char[] json, ref int index)
        {
            if (index != json.Length)
            {
                switch (json[index++])
                {
                    case ',':
                        return Token.Comma;

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
                        return Token.Number;

                    case ':':
                        return Token.Colon;

                    case '[':
                        return Token.SquaredOpen;

                    case ']':
                        return Token.SquaredClose;

                    case '{':
                        return Token.CurlyOpen;

                    case '}':
                        return Token.CurlyClose;

                    case '"':
                        return Token.String;
                }
                index--;
                int num2 = json.Length - index;
                if ((((num2 >= 5) && (json[index] == 'f')) && ((json[index + 1] == 'a') && (json[index + 2] == 'l'))) && ((json[index + 3] == 's') && (json[index + 4] == 'e')))
                {
                    index += 5;
                    return Token.False;
                }
                if ((((num2 >= 4) && (json[index] == 't')) && ((json[index + 1] == 'r') && (json[index + 2] == 'u'))) && (json[index + 3] == 'e'))
                {
                    index += 4;
                    return Token.True;
                }
                if ((((num2 >= 4) && (json[index] == 'n')) && ((json[index + 1] == 'u') && (json[index + 2] == 'l'))) && (json[index + 3] == 'l'))
                {
                    index += 4;
                    return Token.Null;
                }
            }
            return Token.None;
        }

        public double ParseDoubleNumber()
        {
            if (!double.TryParse(this.GetNumberString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
            {
                return 0.0;
            }
            return num;
        }

        public float ParseFloatNumber()
        {
            if (!float.TryParse(this.GetNumberString(), NumberStyles.Float, CultureInfo.InvariantCulture, out float num))
            {
                return 0f;
            }
            return num;
        }

        public string ParseString()
        {
            int charCount = 0;
            StringBuilder builder = null;
            this.SkipWhiteSpaces();
            char ch = this.json[this.index++];
            bool flag = false;
            bool flag2 = false;
            while (!flag2 && !flag)
            {
                if (this.index == this.json.Length)
                {
                    break;
                }
                ch = this.json[this.index++];
                if (ch == '"')
                {
                    flag2 = true;
                    break;
                }
                if (ch != '\\')
                {
                    goto Label_01F0;
                }
                if (this.index == this.json.Length)
                {
                    break;
                }
                ch = this.json[this.index++];
                switch (ch)
                {
                    case 'r':
                        this.stringBuffer[charCount++] = '\r';
                        goto Label_01FD;

                    case 't':
                        this.stringBuffer[charCount++] = '\t';
                        goto Label_01FD;

                    case 'u':
                    {
                        int num3 = this.json.Length - this.index;
                        if (num3 < 4)
                        {
                            goto Label_01E3;
                        }
                        string str = new string(this.json, this.index, 4);
                        this.stringBuffer[charCount++] = (char) Convert.ToInt32(str, 0x10);
                        this.index += 4;
                        goto Label_01FD;
                    }
                    default:
                        if (ch != '"')
                        {
                            if (ch == '/')
                            {
                                goto Label_011C;
                            }
                            if (ch == '\\')
                            {
                                break;
                            }
                            if (ch == 'b')
                            {
                                goto Label_012F;
                            }
                            if (ch == 'f')
                            {
                                goto Label_0141;
                            }
                            if (ch == 'n')
                            {
                                goto Label_0154;
                            }
                        }
                        else
                        {
                            this.stringBuffer[charCount++] = '"';
                        }
                        goto Label_01FD;
                }
                this.stringBuffer[charCount++] = '\\';
                goto Label_01FD;
            Label_011C:
                this.stringBuffer[charCount++] = '/';
                goto Label_01FD;
            Label_012F:
                this.stringBuffer[charCount++] = '\b';
                goto Label_01FD;
            Label_0141:
                this.stringBuffer[charCount++] = '\f';
                goto Label_01FD;
            Label_0154:
                this.stringBuffer[charCount++] = '\n';
                goto Label_01FD;
            Label_01E3:
                flag = true;
                goto Label_01FD;
            Label_01F0:
                this.stringBuffer[charCount++] = ch;
            Label_01FD:
                if (charCount >= this.stringBuffer.Length)
                {
                    if (builder == null)
                    {
                        builder = new StringBuilder();
                    }
                    builder.Append(this.stringBuffer, 0, charCount);
                    charCount = 0;
                }
            }
            if (!flag2)
            {
                this.success = false;
                return null;
            }
            if (builder != null)
            {
                return builder.ToString();
            }
            return new string(this.stringBuffer, 0, charCount);
        }

        public void Reset()
        {
            this.index = 0;
            this.lineNumber = 1;
            this.success = true;
        }

        private void SkipWhiteSpaces()
        {
            while (this.index < this.json.Length)
            {
                char ch = this.json[this.index];
                if (ch == '\n')
                {
                    this.lineNumber++;
                }
                if (!char.IsWhiteSpace(this.json[this.index]))
                {
                    break;
                }
                this.index++;
            }
        }

        public bool hasError =>
            !this.success;

        public int lineNumber { get; private set; }

        public bool parseNumbersAsFloat { get; set; }

        public enum Token
        {
            None,
            Null,
            True,
            False,
            Colon,
            Comma,
            String,
            Number,
            CurlyOpen,
            CurlyClose,
            SquaredOpen,
            SquaredClose
        }
    }
}

