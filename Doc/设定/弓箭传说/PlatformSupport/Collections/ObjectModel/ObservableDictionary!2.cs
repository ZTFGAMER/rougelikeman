namespace PlatformSupport.Collections.ObjectModel
{
    using PlatformSupport.Collections.Specialized;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        private const string CountString = "Count";
        private const string IndexerName = "Item[]";
        private const string KeysName = "Keys";
        private const string ValuesName = "Values";
        private IDictionary<TKey, TValue> _Dictionary;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableDictionary()
        {
            this._Dictionary = new Dictionary<TKey, TValue>();
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this._Dictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        public ObservableDictionary(IEqualityComparer<TKey> comparer)
        {
            this._Dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        public ObservableDictionary(int capacity)
        {
            this._Dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            this._Dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            this._Dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Insert(item.Key, item.Value, true);
        }

        public void Add(TKey key, TValue value)
        {
            this.Insert(key, value, true);
        }

        public void AddRange(IDictionary<TKey, TValue> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Count > 0)
            {
                if (this.Dictionary.Count > 0)
                {
                    if (items.Keys.Any<TKey>(k => base.Dictionary.ContainsKey(k)))
                    {
                        throw new ArgumentException("An item with the same key has already been added.");
                    }
                    foreach (KeyValuePair<TKey, TValue> pair in items)
                    {
                        this.Dictionary.Add(pair);
                    }
                }
                else
                {
                    this._Dictionary = new Dictionary<TKey, TValue>(items);
                }
                this.OnCollectionChanged(NotifyCollectionChangedAction.Add, items.ToArray<KeyValuePair<TKey, TValue>>());
            }
        }

        public void Clear()
        {
            if (this.Dictionary.Count > 0)
            {
                this.Dictionary.Clear();
                this.OnCollectionChanged();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => 
            this.Dictionary.Contains(item);

        public bool ContainsKey(TKey key) => 
            this.Dictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.Dictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => 
            this.Dictionary.GetEnumerator();

        private void Insert(TKey key, TValue value, bool add)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (this.Dictionary.TryGetValue(key, out TValue local))
            {
                if (add)
                {
                    throw new ArgumentException("An item with the same key has already been added.");
                }
                if (!object.Equals(local, value))
                {
                    this.Dictionary[key] = value;
                    this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, local));
                }
            }
            else
            {
                this.Dictionary[key] = value;
                this.OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
            }
        }

        private void OnCollectionChanged()
        {
            this.OnPropertyChanged();
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem)
        {
            this.OnPropertyChanged();
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem));
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems)
        {
            this.OnPropertyChanged();
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItems));
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
        {
            this.OnPropertyChanged();
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
            }
        }

        private void OnPropertyChanged()
        {
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");
            this.OnPropertyChanged("Keys");
            this.OnPropertyChanged("Values");
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            this.Dictionary.TryGetValue(key, out _);
            bool flag = this.Dictionary.Remove(key);
            if (flag)
            {
                this.OnCollectionChanged();
            }
            return flag;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) => 
            this.Remove(item.Key);

        IEnumerator IEnumerable.GetEnumerator() => 
            this.Dictionary.GetEnumerator();

        public bool TryGetValue(TKey key, out TValue value) => 
            this.Dictionary.TryGetValue(key, out value);

        protected IDictionary<TKey, TValue> Dictionary =>
            this._Dictionary;

        public ICollection<TKey> Keys =>
            this.Dictionary.Keys;

        public ICollection<TValue> Values =>
            this.Dictionary.Values;

        public TValue this[TKey key]
        {
            get => 
                this.Dictionary[key];
            set => 
                this.Insert(key, value, false);
        }

        public int Count =>
            this.Dictionary.Count;

        public bool IsReadOnly =>
            this.Dictionary.IsReadOnly;
    }
}

