namespace TableTool
{
    using Dxx.Util;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using UnityEngine;

    public abstract class LocalModel<T, Key> where T: LocalBean, new()
    {
        private IList<T> _BeanList;
        private Dictionary<Key, T> _BeanMap;
        private List<Key> _BeanKeyList;

        public LocalModel()
        {
            this._BeanList = new List<T>();
            this._BeanMap = new Dictionary<Key, T>();
            this._BeanKeyList = new List<Key>();
            this.Initialise();
        }

        protected virtual void ArrangeBeans()
        {
            IEnumerator<T> enumerator = this._BeanList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                T current = enumerator.Current;
                Key beanKey = this.GetBeanKey(current);
                this._BeanMap[beanKey] = current;
                this._BeanKeyList.Add(beanKey);
            }
        }

        protected T CreateBean() => 
            Activator.CreateInstance<T>();

        public void DoNothing()
        {
        }

        public IList<T> GetAllBeans() => 
            this._BeanList;

        public T GetBeanById(Key key)
        {
            T local = null;
            if (this._BeanMap.TryGetValue(key, out local))
            {
                return local;
            }
            return null;
        }

        protected int GetBeanCount(byte[] raws) => 
            IPAddress.NetworkToHostOrder(BitConverter.ToInt32(new byte[] { raws[0], raws[1], raws[2], raws[3] }, 0));

        public Dictionary<Key, T> GetBeanDic() => 
            this._BeanMap;

        protected abstract Key GetBeanKey(T bean);
        public List<Key> GetBeanKeyList() => 
            this._BeanKeyList;

        protected void Initialise()
        {
            this.ReadFromFile();
            this.ArrangeBeans();
        }

        protected bool ReadFromFile()
        {
            bool flag = true;
            object[] args = new object[] { this.Filename };
            string name = Utils.FormatString("{0}.bytes", args);
            try
            {
                this._BeanList.Clear();
                byte[] raws = null;
                raws = FileUtils.GetFileBytes("data/excel", name);
                if (raws != null)
                {
                    if (raws.Length < 4)
                    {
                        flag = false;
                    }
                    else
                    {
                        int beanCount = this.GetBeanCount(raws);
                        int startPos = 4;
                        for (int i = 0; i < beanCount; i++)
                        {
                            T item = this.CreateBean();
                            startPos = item.readFromBytes(raws, startPos);
                            this._BeanList.Add(item);
                        }
                    }
                }
                else
                {
                    flag = false;
                }
            }
            catch
            {
                flag = false;
            }
            if (!flag)
            {
                PlayerPrefs.SetString(name, string.Empty);
                this._BeanList.Clear();
                try
                {
                    byte[] bytes = ResourceManager.Load<TextAsset>(this.FullPath).bytes;
                    if ((bytes == null) || (bytes.Length < 4))
                    {
                        return false;
                    }
                    int beanCount = this.GetBeanCount(bytes);
                    int startPos = 4;
                    for (int i = 0; i < beanCount; i++)
                    {
                        T item = this.CreateBean();
                        startPos = item.readFromBytes(bytes, startPos);
                        this._BeanList.Add(item);
                    }
                }
                catch (Exception)
                {
                    Debug.LogError(this.Filename + " load error");
                    return false;
                }
            }
            return true;
        }

        protected abstract string Filename { get; }

        protected string FullPath =>
            ("ExcelData/" + this.Filename);
    }
}

