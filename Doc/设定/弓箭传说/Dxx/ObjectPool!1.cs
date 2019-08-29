namespace Dxx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;

    public class ObjectPool<T> where T: new()
    {
        private readonly Stack<T> m_Stack;
        private readonly UnityAction<T> m_ActionOnGet;
        private readonly UnityAction<T> m_ActionOnRelease;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <countAll>k__BackingField;

        public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
        {
            this.m_Stack = new Stack<T>();
            this.m_ActionOnGet = actionOnGet;
            this.m_ActionOnRelease = actionOnRelease;
        }

        public T Get()
        {
            T local;
            if (this.m_Stack.Count == 0)
            {
                local = Activator.CreateInstance<T>();
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

