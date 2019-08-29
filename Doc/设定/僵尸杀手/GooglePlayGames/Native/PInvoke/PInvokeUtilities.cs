namespace GooglePlayGames.Native.PInvoke
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using UnityEngine;

    internal static class PInvokeUtilities
    {
        private static readonly DateTime UnixEpoch = DateTime.SpecifyKind(new DateTime(0x7b2, 1, 1), DateTimeKind.Utc);

        internal static UIntPtr ArrayToSizeT<T>(T[] array)
        {
            if (array == null)
            {
                return UIntPtr.Zero;
            }
            return new UIntPtr((ulong) array.Length);
        }

        internal static HandleRef CheckNonNull(HandleRef reference)
        {
            if (IsNull(reference))
            {
                throw new InvalidOperationException();
            }
            return reference;
        }

        internal static DateTime FromMillisSinceUnixEpoch(long millisSinceEpoch) => 
            UnixEpoch.Add(TimeSpan.FromMilliseconds((double) millisSinceEpoch));

        internal static bool IsNull(IntPtr pointer) => 
            pointer.Equals(IntPtr.Zero);

        internal static bool IsNull(HandleRef reference) => 
            IsNull(HandleRef.ToIntPtr(reference));

        internal static T[] OutParamsToArray<T>(OutMethod<T> outMethod)
        {
            UIntPtr ptr = outMethod(null, UIntPtr.Zero);
            if (ptr.Equals(UIntPtr.Zero))
            {
                return new T[0];
            }
            T[] localArray = new T[ptr.ToUInt64()];
            outMethod(localArray, ptr);
            return localArray;
        }

        internal static string OutParamsToString(OutStringMethod outStringMethod)
        {
            UIntPtr ptr = outStringMethod(null, UIntPtr.Zero);
            if (ptr.Equals(UIntPtr.Zero))
            {
                return null;
            }
            try
            {
                byte[] buffer = new byte[ptr.ToUInt32()];
                outStringMethod(buffer, ptr);
                return Encoding.UTF8.GetString(buffer, 0, ((int) ptr.ToUInt32()) - 1);
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogError("Exception creating string from char array: " + exception);
                return string.Empty;
            }
        }

        [DebuggerHidden]
        internal static IEnumerable<T> ToEnumerable<T>(UIntPtr size, Func<UIntPtr, T> getElement) => 
            new <ToEnumerable>c__Iterator0<T> { 
                size = size,
                getElement = getElement,
                $PC = -2
            };

        internal static IEnumerator<T> ToEnumerator<T>(UIntPtr size, Func<UIntPtr, T> getElement) => 
            ToEnumerable<T>(size, getElement).GetEnumerator();

        internal static long ToMilliseconds(TimeSpan span)
        {
            double totalMilliseconds = span.TotalMilliseconds;
            if (totalMilliseconds > 9.2233720368547758E+18)
            {
                return 0x7fffffffffffffffL;
            }
            if (totalMilliseconds < -9.2233720368547758E+18)
            {
                return -9223372036854775808L;
            }
            return Convert.ToInt64(totalMilliseconds);
        }

        [CompilerGenerated]
        private sealed class <ToEnumerable>c__Iterator0<T> : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T>
        {
            internal ulong <i>__1;
            internal UIntPtr size;
            internal Func<UIntPtr, T> getElement;
            internal T $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<i>__1 = 0L;
                        break;

                    case 1:
                        this.<i>__1 += (ulong) 1L;
                        break;

                    default:
                        goto Label_008A;
                }
                if (this.<i>__1 < this.size.ToUInt64())
                {
                    this.$current = this.getElement(new UIntPtr(this.<i>__1));
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$PC = -1;
            Label_008A:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new PInvokeUtilities.<ToEnumerable>c__Iterator0<T> { 
                    size = this.size,
                    getElement = this.getElement
                };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();

            T IEnumerator<T>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        internal delegate UIntPtr OutMethod<T>([In, Out] T[] out_bytes, UIntPtr out_size);

        internal delegate UIntPtr OutStringMethod([In, Out] byte[] out_bytes, UIntPtr out_size);
    }
}

