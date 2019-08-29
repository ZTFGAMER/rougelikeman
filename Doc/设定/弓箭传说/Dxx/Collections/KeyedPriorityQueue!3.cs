namespace Dxx.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class KeyedPriorityQueue<K, V, P>
    {
        public CompareDelegate<K, V, P> priorityComparer;
        private Comparer<P> mComparer;
        private Dictionary<K, Node<K, V, P>> mDict;
        private List<Node<K, V, P>> mHeap;
        private int mCount;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event KeyedPriorityQueueHeadChangeDelegate<K, V, P> onHeadChanged;

        public KeyedPriorityQueue()
        {
            this.mComparer = Comparer<P>.Default;
            this.mDict = new Dictionary<K, Node<K, V, P>>();
            this.mHeap = new List<Node<K, V, P>>();
            this.mHeap.Add(null);
        }

        public void Clear()
        {
            this.mHeap.Clear();
            this.mDict.Clear();
            this.mHeap.Add(null);
            this.mCount = 0;
        }

        private int Compare(P p1, P p2)
        {
            if (this.priorityComparer != null)
            {
                return this.priorityComparer(p1, p2);
            }
            return this.mComparer.Compare(p1, p2);
        }

        public bool Contains(K key) => 
            this.mDict.ContainsKey(key);

        public V Dequeue()
        {
            if (this.mCount <= 0)
            {
                throw new InvalidOperationException("Empty Queue");
            }
            Node<K, V, P> node = this.mHeap[1];
            this.mHeap[1] = this.mHeap[this.mCount];
            this.mHeap[1].index = 1;
            this.mHeap[this.mCount] = node;
            this.mCount--;
            this.mDict.Remove(node.key);
            this.Heapify(1);
            V local = node.value;
            node.key = default(K);
            node.value = default(V);
            node.priority = default(P);
            return local;
        }

        public V Dequeue(out K key, out P priority)
        {
            if (this.mCount <= 0)
            {
                throw new InvalidOperationException("Empty Queue");
            }
            Node<K, V, P> node = this.mHeap[1];
            this.mHeap[1] = this.mHeap[this.mCount];
            this.mHeap[1].index = 1;
            this.mHeap[this.mCount] = node;
            this.mCount--;
            this.mDict.Remove(node.key);
            this.Heapify(1);
            key = node.key;
            priority = node.priority;
            V local = node.value;
            node.key = default(K);
            node.value = default(V);
            node.priority = default(P);
            return local;
        }

        public void Enqueue(K key, V value, P priority)
        {
            if (this.mDict.ContainsKey(key))
            {
                throw new ArgumentException("An element with the same key already exists in the KeyedPriorityQueue");
            }
            V oriHead = (this.mCount <= 0) ? default(V) : this.mHeap[1].value;
            int num = ++this.mCount;
            if (this.mCount == this.mHeap.Count)
            {
                this.mHeap.Add(new Node<K, V, P>());
            }
            Node<K, V, P> node = this.mHeap[this.mCount];
            node.key = key;
            node.value = value;
            node.priority = priority;
            for (int i = num >> 1; (i > 0) && (this.Compare(priority, this.mHeap[i].priority) < 0); i = num >> 1)
            {
                this.mHeap[num] = this.mHeap[i];
                this.mHeap[num].index = num;
                num = i;
            }
            this.mHeap[num] = node;
            this.mDict.Add(key, node);
            node.index = num;
            if ((num == 1) && (this.onHeadChanged != null))
            {
                this.onHeadChanged((KeyedPriorityQueue<K, V, P>) this, oriHead, this.mHeap[1].value);
            }
        }

        private void Heapify(int i)
        {
            int num = i << 1;
            int num2 = num + 1;
            int num3 = i;
            if ((num <= this.mCount) && (this.Compare(this.mHeap[num].priority, this.mHeap[num3].priority) < 0))
            {
                num3 = num;
            }
            if ((num2 <= this.mCount) && (this.Compare(this.mHeap[num2].priority, this.mHeap[num3].priority) < 0))
            {
                num3 = num2;
            }
            if (num3 != i)
            {
                Node<K, V, P> node = this.mHeap[num3];
                this.mHeap[num3] = this.mHeap[i];
                this.mHeap[num3].index = num3;
                this.mHeap[i] = node;
                node.index = i;
                this.Heapify(num3);
            }
        }

        private int HeapUp(int i)
        {
            Node<K, V, P> node = this.mHeap[i];
            P priority = node.priority;
            for (int j = i >> 1; (j > 0) && (this.Compare(priority, this.mHeap[j].priority) < 0); j = i >> 1)
            {
                this.mHeap[i] = this.mHeap[j];
                this.mHeap[i].index = i;
                i = j;
            }
            this.mHeap[i] = node;
            node.index = i;
            return i;
        }

        public V Peek()
        {
            if (this.mCount <= 0)
            {
                throw new InvalidOperationException("Empty Queue");
            }
            Node<K, V, P> node = this.mHeap[1];
            return node.value;
        }

        public V Peek(out K key, out P priority)
        {
            if (this.mCount <= 0)
            {
                throw new InvalidOperationException("Empty Queue");
            }
            Node<K, V, P> node = this.mHeap[1];
            key = node.key;
            priority = node.priority;
            return node.value;
        }

        public bool RemoveFromQueue(K key)
        {
            if (this.mCount <= 0)
            {
                return false;
            }
            if (!this.mDict.TryGetValue(key, out Node<K, V, P> node))
            {
                return false;
            }
            this.mDict.Remove(key);
            int index = node.index;
            this.mHeap[index] = this.mHeap[this.mCount];
            this.mHeap[this.mCount] = node;
            this.mHeap[index].index = index;
            this.mCount--;
            this.Heapify(index);
            this.HeapUp(index);
            if ((index == 1) && (this.onHeadChanged != null))
            {
                this.onHeadChanged((KeyedPriorityQueue<K, V, P>) this, node.value, (this.mCount <= 0) ? default(V) : this.mHeap[1].value);
            }
            node.key = default(K);
            node.value = default(V);
            node.priority = default(P);
            return true;
        }

        public bool TryGetItem(K key, out V value)
        {
            value = default(V);
            if (this.mCount <= 0)
            {
                return false;
            }
            return this.TryGetItem(key, out value, out _);
        }

        public bool TryGetItem(K key, out V value, out P priority)
        {
            if (this.mCount <= 0)
            {
                value = default(V);
                priority = default(P);
                return false;
            }
            if (this.mDict.TryGetValue(key, out Node<K, V, P> node))
            {
                value = node.value;
                priority = node.priority;
                return true;
            }
            value = default(V);
            priority = default(P);
            return false;
        }

        public int Count =>
            this.mCount;

        public delegate int CompareDelegate(P p1, P p2);

        public delegate void KeyedPriorityQueueHeadChangeDelegate(KeyedPriorityQueue<K, V, P> q, V oriHead, V newHead);

        private class Node
        {
            public K key;
            public V value;
            public P priority;
            public int index;
        }
    }
}

