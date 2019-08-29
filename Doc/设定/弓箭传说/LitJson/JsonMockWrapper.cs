namespace LitJson
{
    using System;
    using System.Collections;

    public class JsonMockWrapper : IJsonWrapper, IList, IOrderedDictionary, ICollection, IEnumerable, IDictionary
    {
        public bool GetBoolean() => 
            false;

        public double GetDouble() => 
            0.0;

        public int GetInt() => 
            0;

        public JsonType GetJsonType() => 
            JsonType.None;

        public long GetLong() => 
            0L;

        public string GetString() => 
            string.Empty;

        IDictionaryEnumerator IOrderedDictionary.GetEnumerator() => 
            null;

        void IOrderedDictionary.Insert(int i, object k, object v)
        {
        }

        void IOrderedDictionary.RemoveAt(int i)
        {
        }

        public void SetBoolean(bool val)
        {
        }

        public void SetDouble(double val)
        {
        }

        public void SetInt(int val)
        {
        }

        public void SetJsonType(JsonType type)
        {
        }

        public void SetLong(long val)
        {
        }

        public void SetString(string val)
        {
        }

        void ICollection.CopyTo(Array array, int index)
        {
        }

        void IDictionary.Add(object k, object v)
        {
        }

        void IDictionary.Clear()
        {
        }

        bool IDictionary.Contains(object key) => 
            false;

        IDictionaryEnumerator IDictionary.GetEnumerator() => 
            null;

        void IDictionary.Remove(object key)
        {
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            null;

        int IList.Add(object value) => 
            0;

        void IList.Clear()
        {
        }

        bool IList.Contains(object value) => 
            false;

        int IList.IndexOf(object value) => 
            -1;

        void IList.Insert(int i, object v)
        {
        }

        void IList.Remove(object value)
        {
        }

        void IList.RemoveAt(int index)
        {
        }

        public string ToJson() => 
            string.Empty;

        public void ToJson(JsonWriter writer)
        {
        }

        bool IList.IsFixedSize =>
            true;

        bool IList.IsReadOnly =>
            true;

        object IList.this[int index]
        {
            get => 
                null;
            set
            {
            }
        }

        int ICollection.Count =>
            0;

        bool ICollection.IsSynchronized =>
            false;

        object ICollection.SyncRoot =>
            null;

        bool IDictionary.IsFixedSize =>
            true;

        bool IDictionary.IsReadOnly =>
            true;

        ICollection IDictionary.Keys =>
            null;

        ICollection IDictionary.Values =>
            null;

        object IDictionary.this[object key]
        {
            get => 
                null;
            set
            {
            }
        }

        object IOrderedDictionary.this[int idx]
        {
            get => 
                null;
            set
            {
            }
        }

        public bool IsArray =>
            false;

        public bool IsBoolean =>
            false;

        public bool IsDouble =>
            false;

        public bool IsInt =>
            false;

        public bool IsLong =>
            false;

        public bool IsObject =>
            false;

        public bool IsString =>
            false;
    }
}

