namespace PureMVC.Core
{
    using PureMVC.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Model : IModel, IDisposable
    {
        protected string m_multitonKey;
        protected IDictionary<string, IProxy> m_proxyMap;
        protected static volatile IModel m_instance;
        protected static readonly IDictionary<string, IModel> m_instanceMap = new Dictionary<string, IModel>();
        public const string DEFAULT_KEY = "PureMVC";
        protected const string MULTITON_MSG = "Model instance for this Multiton key already constructed!";

        public Model() : this("PureMVC")
        {
        }

        public Model(string key)
        {
            this.m_multitonKey = key;
            this.m_proxyMap = new Dictionary<string, IProxy>();
            if (m_instanceMap.ContainsKey(key))
            {
                throw new Exception("Model instance for this Multiton key already constructed!");
            }
            m_instanceMap[key] = this;
            this.InitializeModel();
        }

        public void Dispose()
        {
            RemoveModel(this.m_multitonKey);
            this.m_proxyMap.Clear();
        }

        public static IModel GetInstance(string key)
        {
            if (!m_instanceMap.TryGetValue(key, out IModel model))
            {
                model = new Model(key);
                m_instanceMap[key] = model;
            }
            return model;
        }

        public virtual bool HasProxy(string proxyName) => 
            this.m_proxyMap.ContainsKey(proxyName);

        protected virtual void InitializeModel()
        {
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            proxy.InitializeNotifier(this.m_multitonKey);
            this.m_proxyMap[proxy.ProxyName] = proxy;
            proxy.OnRegister();
        }

        public static void RemoveModel(string key)
        {
            if (m_instanceMap.TryGetValue(key, out IModel model))
            {
                m_instanceMap.Remove(key);
                model.Dispose();
            }
        }

        public virtual IProxy RemoveProxy(string proxyName)
        {
            IProxy proxy = null;
            if (this.m_proxyMap.ContainsKey(proxyName))
            {
                proxy = this.RetrieveProxy(proxyName);
                this.m_proxyMap.Remove(proxyName);
            }
            if (proxy != null)
            {
                proxy.OnRemove();
            }
            return proxy;
        }

        public virtual IProxy RetrieveProxy(string proxyName)
        {
            if (!this.m_proxyMap.ContainsKey(proxyName))
            {
                return null;
            }
            return this.m_proxyMap[proxyName];
        }

        public IEnumerable<string> ListProxyNames =>
            this.m_proxyMap.Keys;

        public static IModel Instance =>
            GetInstance("PureMVC");
    }
}

