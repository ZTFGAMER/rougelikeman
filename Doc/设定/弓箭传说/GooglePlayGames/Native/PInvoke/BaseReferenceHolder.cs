namespace GooglePlayGames.Native.PInvoke
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal abstract class BaseReferenceHolder : IDisposable
    {
        private static Dictionary<HandleRef, BaseReferenceHolder> _refs = new Dictionary<HandleRef, BaseReferenceHolder>();
        private HandleRef mSelfPointer;

        public BaseReferenceHolder(IntPtr pointer)
        {
            this.mSelfPointer = PInvokeUtilities.CheckNonNull(new HandleRef(this, pointer));
        }

        internal IntPtr AsPointer() => 
            this.SelfPtr().Handle;

        protected abstract void CallDispose(HandleRef selfPointer);
        public void Dispose()
        {
            this.Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool fromFinalizer)
        {
            if ((fromFinalizer || !_refs.ContainsKey(this.mSelfPointer)) && !PInvokeUtilities.IsNull(this.mSelfPointer))
            {
                this.CallDispose(this.mSelfPointer);
                this.mSelfPointer = new HandleRef(this, IntPtr.Zero);
            }
        }

        ~BaseReferenceHolder()
        {
            this.Dispose(true);
        }

        internal void ForgetMe()
        {
            if (_refs.ContainsKey(this.SelfPtr()))
            {
                _refs.Remove(this.SelfPtr());
                this.Dispose(false);
            }
        }

        protected bool IsDisposed() => 
            PInvokeUtilities.IsNull(this.mSelfPointer);

        internal void ReferToMe()
        {
            _refs[this.SelfPtr()] = this;
        }

        protected HandleRef SelfPtr()
        {
            if (this.IsDisposed())
            {
                throw new InvalidOperationException("Attempted to use object after it was cleaned up");
            }
            return this.mSelfPointer;
        }
    }
}

