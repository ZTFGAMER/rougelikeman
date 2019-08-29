namespace LitJson
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal class Lexer
    {
        private static int[] fsm_return_table;
        private static StateHandler[] fsm_handler_table;
        private bool allow_comments = true;
        private bool allow_single_quoted_strings = true;
        private bool end_of_input = false;
        private FsmContext fsm_context;
        private int input_buffer = 0;
        private int input_char;
        private TextReader reader;
        private int state = 1;
        private StringBuilder string_buffer = new StringBuilder(0x80);
        private string string_value;
        private int token;
        private int unichar;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache0;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache1;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache2;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache3;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache4;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache5;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache6;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache7;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache8;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache9;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cacheA;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cacheB;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cacheC;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cacheD;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cacheE;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cacheF;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache10;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache11;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache12;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache13;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache14;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache15;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache16;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache17;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache18;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache19;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache1A;
        [CompilerGenerated]
        private static StateHandler <>f__mg$cache1B;

        static Lexer()
        {
            PopulateFsmTables();
        }

        public Lexer(TextReader reader)
        {
            this.reader = reader;
            this.fsm_context = new FsmContext();
            this.fsm_context.L = this;
        }

        private bool GetChar()
        {
            this.input_char = this.NextChar();
            if (this.input_char != -1)
            {
                return true;
            }
            this.end_of_input = true;
            return false;
        }

        private static int HexValue(int digit)
        {
            switch (digit)
            {
                case 0x41:
                case 0x61:
                    return 10;

                case 0x42:
                case 0x62:
                    return 11;

                case 0x43:
                case 0x63:
                    return 12;

                case 0x44:
                case 100:
                    return 13;

                case 0x45:
                case 0x65:
                    return 14;

                case 70:
                case 0x66:
                    return 15;
            }
            return (digit - 0x30);
        }

        private int NextChar()
        {
            if (this.input_buffer != 0)
            {
                int num = this.input_buffer;
                this.input_buffer = 0;
                return num;
            }
            return this.reader.Read();
        }

        public bool NextToken()
        {
            this.fsm_context.Return = false;
            while (true)
            {
                StateHandler handler = fsm_handler_table[this.state - 1];
                if (!handler(this.fsm_context))
                {
                    throw new JsonException(this.input_char);
                }
                if (this.end_of_input)
                {
                    return false;
                }
                if (this.fsm_context.Return)
                {
                    this.string_value = this.string_buffer.ToString();
                    this.string_buffer.Remove(0, this.string_buffer.Length);
                    this.token = fsm_return_table[this.state - 1];
                    if (this.token == 0x10006)
                    {
                        this.token = this.input_char;
                    }
                    this.state = this.fsm_context.NextState;
                    return true;
                }
                this.state = this.fsm_context.NextState;
            }
        }

        private static void PopulateFsmTables()
        {
            StateHandler[] handlerArray1 = new StateHandler[0x1c];
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new StateHandler(Lexer.State1);
            }
            handlerArray1[0] = <>f__mg$cache0;
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new StateHandler(Lexer.State2);
            }
            handlerArray1[1] = <>f__mg$cache1;
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new StateHandler(Lexer.State3);
            }
            handlerArray1[2] = <>f__mg$cache2;
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new StateHandler(Lexer.State4);
            }
            handlerArray1[3] = <>f__mg$cache3;
            if (<>f__mg$cache4 == null)
            {
                <>f__mg$cache4 = new StateHandler(Lexer.State5);
            }
            handlerArray1[4] = <>f__mg$cache4;
            if (<>f__mg$cache5 == null)
            {
                <>f__mg$cache5 = new StateHandler(Lexer.State6);
            }
            handlerArray1[5] = <>f__mg$cache5;
            if (<>f__mg$cache6 == null)
            {
                <>f__mg$cache6 = new StateHandler(Lexer.State7);
            }
            handlerArray1[6] = <>f__mg$cache6;
            if (<>f__mg$cache7 == null)
            {
                <>f__mg$cache7 = new StateHandler(Lexer.State8);
            }
            handlerArray1[7] = <>f__mg$cache7;
            if (<>f__mg$cache8 == null)
            {
                <>f__mg$cache8 = new StateHandler(Lexer.State9);
            }
            handlerArray1[8] = <>f__mg$cache8;
            if (<>f__mg$cache9 == null)
            {
                <>f__mg$cache9 = new StateHandler(Lexer.State10);
            }
            handlerArray1[9] = <>f__mg$cache9;
            if (<>f__mg$cacheA == null)
            {
                <>f__mg$cacheA = new StateHandler(Lexer.State11);
            }
            handlerArray1[10] = <>f__mg$cacheA;
            if (<>f__mg$cacheB == null)
            {
                <>f__mg$cacheB = new StateHandler(Lexer.State12);
            }
            handlerArray1[11] = <>f__mg$cacheB;
            if (<>f__mg$cacheC == null)
            {
                <>f__mg$cacheC = new StateHandler(Lexer.State13);
            }
            handlerArray1[12] = <>f__mg$cacheC;
            if (<>f__mg$cacheD == null)
            {
                <>f__mg$cacheD = new StateHandler(Lexer.State14);
            }
            handlerArray1[13] = <>f__mg$cacheD;
            if (<>f__mg$cacheE == null)
            {
                <>f__mg$cacheE = new StateHandler(Lexer.State15);
            }
            handlerArray1[14] = <>f__mg$cacheE;
            if (<>f__mg$cacheF == null)
            {
                <>f__mg$cacheF = new StateHandler(Lexer.State16);
            }
            handlerArray1[15] = <>f__mg$cacheF;
            if (<>f__mg$cache10 == null)
            {
                <>f__mg$cache10 = new StateHandler(Lexer.State17);
            }
            handlerArray1[0x10] = <>f__mg$cache10;
            if (<>f__mg$cache11 == null)
            {
                <>f__mg$cache11 = new StateHandler(Lexer.State18);
            }
            handlerArray1[0x11] = <>f__mg$cache11;
            if (<>f__mg$cache12 == null)
            {
                <>f__mg$cache12 = new StateHandler(Lexer.State19);
            }
            handlerArray1[0x12] = <>f__mg$cache12;
            if (<>f__mg$cache13 == null)
            {
                <>f__mg$cache13 = new StateHandler(Lexer.State20);
            }
            handlerArray1[0x13] = <>f__mg$cache13;
            if (<>f__mg$cache14 == null)
            {
                <>f__mg$cache14 = new StateHandler(Lexer.State21);
            }
            handlerArray1[20] = <>f__mg$cache14;
            if (<>f__mg$cache15 == null)
            {
                <>f__mg$cache15 = new StateHandler(Lexer.State22);
            }
            handlerArray1[0x15] = <>f__mg$cache15;
            if (<>f__mg$cache16 == null)
            {
                <>f__mg$cache16 = new StateHandler(Lexer.State23);
            }
            handlerArray1[0x16] = <>f__mg$cache16;
            if (<>f__mg$cache17 == null)
            {
                <>f__mg$cache17 = new StateHandler(Lexer.State24);
            }
            handlerArray1[0x17] = <>f__mg$cache17;
            if (<>f__mg$cache18 == null)
            {
                <>f__mg$cache18 = new StateHandler(Lexer.State25);
            }
            handlerArray1[0x18] = <>f__mg$cache18;
            if (<>f__mg$cache19 == null)
            {
                <>f__mg$cache19 = new StateHandler(Lexer.State26);
            }
            handlerArray1[0x19] = <>f__mg$cache19;
            if (<>f__mg$cache1A == null)
            {
                <>f__mg$cache1A = new StateHandler(Lexer.State27);
            }
            handlerArray1[0x1a] = <>f__mg$cache1A;
            if (<>f__mg$cache1B == null)
            {
                <>f__mg$cache1B = new StateHandler(Lexer.State28);
            }
            handlerArray1[0x1b] = <>f__mg$cache1B;
            fsm_handler_table = handlerArray1;
            fsm_return_table = new int[] { 
                0x10006, 0, 0x10001, 0x10001, 0, 0x10001, 0, 0x10001, 0, 0, 0x10002, 0, 0, 0, 0x10003, 0,
                0, 0x10004, 0x10005, 0x10006, 0, 0, 0x10005, 0x10006, 0, 0, 0, 0
            };
        }

        private static char ProcessEscChar(int esc_char)
        {
            switch (esc_char)
            {
                case 0x72:
                    return '\r';

                case 0x74:
                    return '\t';
            }
            if (((esc_char != 0x22) && (esc_char != 0x27)) && ((esc_char != 0x2f) && (esc_char != 0x5c)))
            {
                if (esc_char == 0x62)
                {
                    return '\b';
                }
                if (esc_char == 0x66)
                {
                    return '\f';
                }
                if (esc_char == 110)
                {
                    return '\n';
                }
                return '?';
            }
            return Convert.ToChar(esc_char);
        }

        private static bool State1(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if ((ctx.L.input_char == 0x20) || ((ctx.L.input_char >= 9) && (ctx.L.input_char <= 13)))
                {
                    continue;
                }
                if ((ctx.L.input_char >= 0x31) && (ctx.L.input_char <= 0x39))
                {
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    ctx.NextState = 3;
                    return true;
                }
                int num = ctx.L.input_char;
                switch (num)
                {
                    case 0x2c:
                    case 0x5b:
                    case 0x5d:
                    case 0x7b:
                    case 0x7d:
                    case 0x3a:
                        ctx.NextState = 1;
                        ctx.Return = true;
                        return true;

                    case 0x2d:
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        ctx.NextState = 2;
                        return true;

                    case 0x2f:
                        if (ctx.L.allow_comments)
                        {
                            break;
                        }
                        return false;

                    case 0x30:
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        ctx.NextState = 4;
                        return true;

                    case 0x22:
                        ctx.NextState = 0x13;
                        ctx.Return = true;
                        return true;

                    case 0x27:
                        if (!ctx.L.allow_single_quoted_strings)
                        {
                            return false;
                        }
                        ctx.L.input_char = 0x22;
                        ctx.NextState = 0x17;
                        ctx.Return = true;
                        return true;

                    case 0x66:
                        ctx.NextState = 12;
                        return true;

                    case 110:
                        ctx.NextState = 0x10;
                        return true;

                    default:
                        if (num != 0x74)
                        {
                            return false;
                        }
                        ctx.NextState = 9;
                        return true;
                }
                ctx.NextState = 0x19;
                return true;
            }
            return true;
        }

        private static bool State10(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char != 0x75)
            {
                return false;
            }
            ctx.NextState = 11;
            return true;
        }

        private static bool State11(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char != 0x65)
            {
                return false;
            }
            ctx.Return = true;
            ctx.NextState = 1;
            return true;
        }

        private static bool State12(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char != 0x61)
            {
                return false;
            }
            ctx.NextState = 13;
            return true;
        }

        private static bool State13(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char != 0x6c)
            {
                return false;
            }
            ctx.NextState = 14;
            return true;
        }

        private static bool State14(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char != 0x73)
            {
                return false;
            }
            ctx.NextState = 15;
            return true;
        }

        private static bool State15(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char != 0x65)
            {
                return false;
            }
            ctx.Return = true;
            ctx.NextState = 1;
            return true;
        }

        private static bool State16(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char != 0x75)
            {
                return false;
            }
            ctx.NextState = 0x11;
            return true;
        }

        private static bool State17(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char != 0x6c)
            {
                return false;
            }
            ctx.NextState = 0x12;
            return true;
        }

        private static bool State18(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char != 0x6c)
            {
                return false;
            }
            ctx.Return = true;
            ctx.NextState = 1;
            return true;
        }

        private static bool State19(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                switch (ctx.L.input_char)
                {
                    case 0x22:
                        ctx.L.UngetChar();
                        ctx.Return = true;
                        ctx.NextState = 20;
                        return true;

                    case 0x5c:
                        ctx.StateStack = 0x13;
                        ctx.NextState = 0x15;
                        return true;
                }
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
            }
            return true;
        }

        private static bool State2(FsmContext ctx)
        {
            ctx.L.GetChar();
            if ((ctx.L.input_char >= 0x31) && (ctx.L.input_char <= 0x39))
            {
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 3;
                return true;
            }
            if (ctx.L.input_char != 0x30)
            {
                return false;
            }
            ctx.L.string_buffer.Append((char) ctx.L.input_char);
            ctx.NextState = 4;
            return true;
        }

        private static bool State20(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char != 0x22)
            {
                return false;
            }
            ctx.Return = true;
            ctx.NextState = 1;
            return true;
        }

        private static bool State21(FsmContext ctx)
        {
            ctx.L.GetChar();
            int num = ctx.L.input_char;
            switch (num)
            {
                case 0x72:
                case 0x74:
                    break;

                case 0x75:
                    ctx.NextState = 0x16;
                    return true;

                default:
                    if ((((num != 0x22) && (num != 0x27)) && ((num != 0x2f) && (num != 0x5c))) && (((num != 0x62) && (num != 0x66)) && (num != 110)))
                    {
                        return false;
                    }
                    break;
            }
            ctx.L.string_buffer.Append(ProcessEscChar(ctx.L.input_char));
            ctx.NextState = ctx.StateStack;
            return true;
        }

        private static bool State22(FsmContext ctx)
        {
            int num = 0;
            int num2 = 0x1000;
            ctx.L.unichar = 0;
            while (ctx.L.GetChar())
            {
                if ((((ctx.L.input_char < 0x30) || (ctx.L.input_char > 0x39)) && ((ctx.L.input_char < 0x41) || (ctx.L.input_char > 70))) && ((ctx.L.input_char < 0x61) || (ctx.L.input_char > 0x66)))
                {
                    return false;
                }
                ctx.L.unichar += HexValue(ctx.L.input_char) * num2;
                num++;
                num2 /= 0x10;
                if (num == 4)
                {
                    ctx.L.string_buffer.Append(Convert.ToChar(ctx.L.unichar));
                    ctx.NextState = ctx.StateStack;
                    return true;
                }
            }
            return true;
        }

        private static bool State23(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                switch (ctx.L.input_char)
                {
                    case 0x27:
                        ctx.L.UngetChar();
                        ctx.Return = true;
                        ctx.NextState = 0x18;
                        return true;

                    case 0x5c:
                        ctx.StateStack = 0x17;
                        ctx.NextState = 0x15;
                        return true;
                }
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
            }
            return true;
        }

        private static bool State24(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char != 0x27)
            {
                return false;
            }
            ctx.L.input_char = 0x22;
            ctx.Return = true;
            ctx.NextState = 1;
            return true;
        }

        private static bool State25(FsmContext ctx)
        {
            ctx.L.GetChar();
            switch (ctx.L.input_char)
            {
                case 0x2a:
                    ctx.NextState = 0x1b;
                    return true;

                case 0x2f:
                    ctx.NextState = 0x1a;
                    return true;
            }
            return false;
        }

        private static bool State26(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if (ctx.L.input_char == 10)
                {
                    ctx.NextState = 1;
                    return true;
                }
            }
            return true;
        }

        private static bool State27(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if (ctx.L.input_char == 0x2a)
                {
                    ctx.NextState = 0x1c;
                    return true;
                }
            }
            return true;
        }

        private static bool State28(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if (ctx.L.input_char != 0x2a)
                {
                    if (ctx.L.input_char == 0x2f)
                    {
                        ctx.NextState = 1;
                        return true;
                    }
                    ctx.NextState = 0x1b;
                    return true;
                }
            }
            return true;
        }

        private static bool State3(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if ((ctx.L.input_char >= 0x30) && (ctx.L.input_char <= 0x39))
                {
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    continue;
                }
                if ((ctx.L.input_char == 0x20) || ((ctx.L.input_char >= 9) && (ctx.L.input_char <= 13)))
                {
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;
                }
                int num = ctx.L.input_char;
                switch (num)
                {
                    case 0x2c:
                        break;

                    case 0x2e:
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        ctx.NextState = 5;
                        return true;

                    case 0x45:
                        goto Label_0118;

                    default:
                        if (num != 0x5d)
                        {
                            if (num == 0x65)
                            {
                                goto Label_0118;
                            }
                            if (num != 0x7d)
                            {
                                return false;
                            }
                        }
                        break;
                }
                ctx.L.UngetChar();
                ctx.Return = true;
                ctx.NextState = 1;
                return true;
            Label_0118:
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 7;
                return true;
            }
            return true;
        }

        private static bool State4(FsmContext ctx)
        {
            ctx.L.GetChar();
            if ((ctx.L.input_char == 0x20) || ((ctx.L.input_char >= 9) && (ctx.L.input_char <= 13)))
            {
                ctx.Return = true;
                ctx.NextState = 1;
                return true;
            }
            int num = ctx.L.input_char;
            switch (num)
            {
                case 0x2c:
                    break;

                case 0x2e:
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    ctx.NextState = 5;
                    return true;

                case 0x45:
                    goto Label_00D9;

                default:
                    if (num != 0x5d)
                    {
                        if (num == 0x65)
                        {
                            goto Label_00D9;
                        }
                        if (num != 0x7d)
                        {
                            return false;
                        }
                    }
                    break;
            }
            ctx.L.UngetChar();
            ctx.Return = true;
            ctx.NextState = 1;
            return true;
        Label_00D9:
            ctx.L.string_buffer.Append((char) ctx.L.input_char);
            ctx.NextState = 7;
            return true;
        }

        private static bool State5(FsmContext ctx)
        {
            ctx.L.GetChar();
            if ((ctx.L.input_char >= 0x30) && (ctx.L.input_char <= 0x39))
            {
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 6;
                return true;
            }
            return false;
        }

        private static bool State6(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if ((ctx.L.input_char >= 0x30) && (ctx.L.input_char <= 0x39))
                {
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    continue;
                }
                if ((ctx.L.input_char == 0x20) || ((ctx.L.input_char >= 9) && (ctx.L.input_char <= 13)))
                {
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;
                }
                int num = ctx.L.input_char;
                if (num != 0x2c)
                {
                    if (num == 0x45)
                    {
                        goto Label_00E5;
                    }
                    if (num != 0x5d)
                    {
                        if (num == 0x65)
                        {
                            goto Label_00E5;
                        }
                        if (num != 0x7d)
                        {
                            return false;
                        }
                    }
                }
                ctx.L.UngetChar();
                ctx.Return = true;
                ctx.NextState = 1;
                return true;
            Label_00E5:
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 7;
                return true;
            }
            return true;
        }

        private static bool State7(FsmContext ctx)
        {
            ctx.L.GetChar();
            if ((ctx.L.input_char >= 0x30) && (ctx.L.input_char <= 0x39))
            {
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 8;
                return true;
            }
            int num = ctx.L.input_char;
            if ((num != 0x2b) && (num != 0x2d))
            {
                return false;
            }
            ctx.L.string_buffer.Append((char) ctx.L.input_char);
            ctx.NextState = 8;
            return true;
        }

        private static bool State8(FsmContext ctx)
        {
            while (ctx.L.GetChar())
            {
                if ((ctx.L.input_char >= 0x30) && (ctx.L.input_char <= 0x39))
                {
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                }
                else
                {
                    if ((ctx.L.input_char == 0x20) || ((ctx.L.input_char >= 9) && (ctx.L.input_char <= 13)))
                    {
                        ctx.Return = true;
                        ctx.NextState = 1;
                        return true;
                    }
                    int num = ctx.L.input_char;
                    if (((num != 0x2c) && (num != 0x5d)) && (num != 0x7d))
                    {
                        return false;
                    }
                    ctx.L.UngetChar();
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;
                }
            }
            return true;
        }

        private static bool State9(FsmContext ctx)
        {
            ctx.L.GetChar();
            if (ctx.L.input_char != 0x72)
            {
                return false;
            }
            ctx.NextState = 10;
            return true;
        }

        private void UngetChar()
        {
            this.input_buffer = this.input_char;
        }

        public bool AllowComments
        {
            get => 
                this.allow_comments;
            set => 
                (this.allow_comments = value);
        }

        public bool AllowSingleQuotedStrings
        {
            get => 
                this.allow_single_quoted_strings;
            set => 
                (this.allow_single_quoted_strings = value);
        }

        public bool EndOfInput =>
            this.end_of_input;

        public int Token =>
            this.token;

        public string StringValue =>
            this.string_value;

        private delegate bool StateHandler(FsmContext ctx);
    }
}

