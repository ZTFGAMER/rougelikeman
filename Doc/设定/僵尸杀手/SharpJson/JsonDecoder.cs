namespace SharpJson
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class JsonDecoder
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <errorMessage>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <parseNumbersAsFloat>k__BackingField;
        private Lexer lexer;

        public JsonDecoder()
        {
            this.errorMessage = null;
            this.parseNumbersAsFloat = false;
        }

        public object Decode(string text)
        {
            this.errorMessage = null;
            this.lexer = new Lexer(text);
            this.lexer.parseNumbersAsFloat = this.parseNumbersAsFloat;
            return this.ParseValue();
        }

        public static object DecodeText(string text)
        {
            JsonDecoder decoder = new JsonDecoder();
            return decoder.Decode(text);
        }

        private T EvalLexer<T>(T value)
        {
            if (this.lexer.hasError)
            {
                this.TriggerError("Lexical error ocurred");
            }
            return value;
        }

        private IList<object> ParseArray()
        {
            Lexer.Token token;
            List<object> list = new List<object>();
            this.lexer.NextToken();
        Label_0012:
            token = this.lexer.LookAhead();
            if (token != Lexer.Token.None)
            {
                if (token == Lexer.Token.Comma)
                {
                    this.lexer.NextToken();
                }
                else
                {
                    if (token == Lexer.Token.SquaredClose)
                    {
                        this.lexer.NextToken();
                        return list;
                    }
                    object item = this.ParseValue();
                    if (this.errorMessage != null)
                    {
                        return null;
                    }
                    list.Add(item);
                }
                goto Label_0012;
            }
            this.TriggerError("Invalid token");
            return null;
        }

        private IDictionary<string, object> ParseObject()
        {
            Lexer.Token token;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            this.lexer.NextToken();
        Label_0012:
            token = this.lexer.LookAhead();
            if (token != Lexer.Token.None)
            {
                if (token == Lexer.Token.Comma)
                {
                    this.lexer.NextToken();
                }
                else
                {
                    if (token == Lexer.Token.CurlyClose)
                    {
                        this.lexer.NextToken();
                        return dictionary;
                    }
                    string str = this.EvalLexer<string>(this.lexer.ParseString());
                    if (this.errorMessage != null)
                    {
                        return null;
                    }
                    if (this.lexer.NextToken() != Lexer.Token.Colon)
                    {
                        this.TriggerError("Invalid token; expected ':'");
                        return null;
                    }
                    object obj2 = this.ParseValue();
                    if (this.errorMessage != null)
                    {
                        return null;
                    }
                    dictionary[str] = obj2;
                }
                goto Label_0012;
            }
            this.TriggerError("Invalid token");
            return null;
        }

        private object ParseValue()
        {
            switch (this.lexer.LookAhead())
            {
                case Lexer.Token.Null:
                    this.lexer.NextToken();
                    return null;

                case Lexer.Token.True:
                    this.lexer.NextToken();
                    return true;

                case Lexer.Token.False:
                    this.lexer.NextToken();
                    return false;

                case Lexer.Token.String:
                    return this.EvalLexer<string>(this.lexer.ParseString());

                case Lexer.Token.Number:
                    if (!this.parseNumbersAsFloat)
                    {
                        return this.EvalLexer<double>(this.lexer.ParseDoubleNumber());
                    }
                    return this.EvalLexer<float>(this.lexer.ParseFloatNumber());

                case Lexer.Token.CurlyOpen:
                    return this.ParseObject();

                case Lexer.Token.SquaredOpen:
                    return this.ParseArray();
            }
            this.TriggerError("Unable to parse value");
            return null;
        }

        private void TriggerError(string message)
        {
            this.errorMessage = $"Error: '{message}' at line {this.lexer.lineNumber}";
        }

        public string errorMessage { get; private set; }

        public bool parseNumbersAsFloat { get; set; }
    }
}

