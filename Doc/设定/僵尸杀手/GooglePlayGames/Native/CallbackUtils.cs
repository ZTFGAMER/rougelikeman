namespace GooglePlayGames.Native
{
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;

    internal static class CallbackUtils
    {
        [CompilerGenerated]
        private static void <ToOnGameThread`1>m__0<T>(T)
        {
        }

        [CompilerGenerated]
        private static void <ToOnGameThread`2>m__1<T1, T2>(T1, T2)
        {
        }

        [CompilerGenerated]
        private static void <ToOnGameThread`3>m__2<T1, T2, T3>(T1, T2, T3)
        {
        }

        internal static Action<T> ToOnGameThread<T>(Action<T> toConvert)
        {
            <ToOnGameThread>c__AnonStorey0<T> storey = new <ToOnGameThread>c__AnonStorey0<T> {
                toConvert = toConvert
            };
            if (storey.toConvert == null)
            {
                return new Action<T>(CallbackUtils.<ToOnGameThread`1>m__0<T>);
            }
            return new Action<T>(storey.<>m__0);
        }

        internal static Action<T1, T2> ToOnGameThread<T1, T2>(Action<T1, T2> toConvert)
        {
            <ToOnGameThread>c__AnonStorey2<T1, T2> storey = new <ToOnGameThread>c__AnonStorey2<T1, T2> {
                toConvert = toConvert
            };
            if (storey.toConvert == null)
            {
                return new Action<T1, T2>(CallbackUtils.<ToOnGameThread`2>m__1<T1, T2>);
            }
            return new Action<T1, T2>(storey.<>m__0);
        }

        internal static Action<T1, T2, T3> ToOnGameThread<T1, T2, T3>(Action<T1, T2, T3> toConvert)
        {
            <ToOnGameThread>c__AnonStorey4<T1, T2, T3> storey = new <ToOnGameThread>c__AnonStorey4<T1, T2, T3> {
                toConvert = toConvert
            };
            if (storey.toConvert == null)
            {
                return new Action<T1, T2, T3>(CallbackUtils.<ToOnGameThread`3>m__2<T1, T2, T3>);
            }
            return new Action<T1, T2, T3>(storey.<>m__0);
        }

        [CompilerGenerated]
        private sealed class <ToOnGameThread>c__AnonStorey0<T>
        {
            internal Action<T> toConvert;

            internal void <>m__0(T val)
            {
                <ToOnGameThread>c__AnonStorey1<T> storey = new <ToOnGameThread>c__AnonStorey1<T> {
                    <>f__ref$0 = (CallbackUtils.<ToOnGameThread>c__AnonStorey0<T>) this,
                    val = val
                };
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }

            private sealed class <ToOnGameThread>c__AnonStorey1
            {
                internal T val;
                internal CallbackUtils.<ToOnGameThread>c__AnonStorey0<T> <>f__ref$0;

                internal void <>m__0()
                {
                    this.<>f__ref$0.toConvert(this.val);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ToOnGameThread>c__AnonStorey2<T1, T2>
        {
            internal Action<T1, T2> toConvert;

            internal void <>m__0(T1 val1, T2 val2)
            {
                <ToOnGameThread>c__AnonStorey3<T1, T2> storey = new <ToOnGameThread>c__AnonStorey3<T1, T2> {
                    <>f__ref$2 = (CallbackUtils.<ToOnGameThread>c__AnonStorey2<T1, T2>) this,
                    val1 = val1,
                    val2 = val2
                };
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }

            private sealed class <ToOnGameThread>c__AnonStorey3
            {
                internal T1 val1;
                internal T2 val2;
                internal CallbackUtils.<ToOnGameThread>c__AnonStorey2<T1, T2> <>f__ref$2;

                internal void <>m__0()
                {
                    this.<>f__ref$2.toConvert(this.val1, this.val2);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ToOnGameThread>c__AnonStorey4<T1, T2, T3>
        {
            internal Action<T1, T2, T3> toConvert;

            internal void <>m__0(T1 val1, T2 val2, T3 val3)
            {
                <ToOnGameThread>c__AnonStorey5<T1, T2, T3> storey = new <ToOnGameThread>c__AnonStorey5<T1, T2, T3> {
                    <>f__ref$4 = (CallbackUtils.<ToOnGameThread>c__AnonStorey4<T1, T2, T3>) this,
                    val1 = val1,
                    val2 = val2,
                    val3 = val3
                };
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
            }

            private sealed class <ToOnGameThread>c__AnonStorey5
            {
                internal T1 val1;
                internal T2 val2;
                internal T3 val3;
                internal CallbackUtils.<ToOnGameThread>c__AnonStorey4<T1, T2, T3> <>f__ref$4;

                internal void <>m__0()
                {
                    this.<>f__ref$4.toConvert(this.val1, this.val2, this.val3);
                }
            }
        }
    }
}

