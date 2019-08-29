namespace Dxx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UnityObjectPool<T> where T: Object
    {
        protected Stack<T> m_Stack;
        protected Func<T> m_actionCreate;
        protected Action<T> m_ActionOnGet;
        protected Action<T> m_ActionOnRelease;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <countAll>k__BackingField;
        private T origin;

        public UnityObjectPool(Func<T> actionCreate, Action<T> actionOnGet, Action<T> actionOnRelease)
        {
            this.m_Stack = new Stack<T>();
            this.m_actionCreate = actionCreate;
            this.m_ActionOnGet = actionOnGet;
            this.m_ActionOnRelease = actionOnRelease;
        }

        public T Get()
        {
            T local;
            if (this.m_Stack.Count == 0)
            {
                local = this.m_actionCreate();
                this.countAll++;
            }
            else
            {
                local = this.m_Stack.Pop();
            }
            if (this.m_ActionOnGet != null)
            {
                this.m_ActionOnGet(local);
            }
            return local;
        }

        public void Release(T element)
        {
            if ((this.m_Stack.Count > 0) && object.ReferenceEquals(this.m_Stack.Peek(), element))
            {
                Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
            }
            if (this.m_ActionOnRelease != null)
            {
                this.m_ActionOnRelease(element);
            }
            this.m_Stack.Push(element);
        }

        public int countAll { get; private set; }

        public int countActive =>
            (this.countAll - this.countInactive);

        public int countInactive =>
            this.m_Stack.Count;
    }
}

