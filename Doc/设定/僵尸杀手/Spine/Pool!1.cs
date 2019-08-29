namespace Spine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Pool<T> where T: class, new()
    {
        public readonly int max;
        private readonly Stack<T> freeObjects;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Peak>k__BackingField;

        public Pool(int initialCapacity = 0x10, int max = 0x7fffffff)
        {
            this.freeObjects = new Stack<T>(initialCapacity);
            this.max = max;
        }

        public void Clear()
        {
            this.freeObjects.Clear();
        }

        public void Free(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj", "obj cannot be null");
            }
            if (this.freeObjects.Count < this.max)
            {
                this.freeObjects.Push(obj);
                this.Peak = Math.Max(this.Peak, this.freeObjects.Count);
            }
            this.Reset(obj);
        }

        public T Obtain() => 
            ((this.freeObjects.Count != 0) ? this.freeObjects.Pop() : Activator.CreateInstance<T>());

        protected void Reset(T obj)
        {
            IPoolable<T> poolable = obj as IPoolable<T>;
            if (poolable != null)
            {
                poolable.Reset();
            }
        }

        public int Count =>
            this.freeObjects.Count;

        public int Peak { get; private set; }

        public interface IPoolable
        {
            void Reset();
        }
    }
}

