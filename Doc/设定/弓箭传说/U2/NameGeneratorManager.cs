namespace U2
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class NameGeneratorManager
    {
        public static string EquipKey = "Equipkey";
        private static volatile NameGeneratorManager _Instance;
        private Dictionary<string, NameGenerator> _NameGeneratorMap = new Dictionary<string, NameGenerator>();
        private object _Lock = new object();

        private NameGeneratorManager()
        {
        }

        public string Generator(string prefix)
        {
            object obj2 = this._Lock;
            lock (obj2)
            {
                if (this._NameGeneratorMap.TryGetValue(prefix, out NameGenerator generator))
                {
                    return generator.Generate();
                }
                return prefix;
            }
        }

        public NameGenerator GetNameGenerator(string prefix)
        {
            object obj2 = this._Lock;
            lock (obj2)
            {
                NameGenerator generator = null;
                this._NameGeneratorMap.TryGetValue(prefix, out generator);
                return generator;
            }
        }

        public void RegisterNameGenerator(string prefix)
        {
            object obj2 = this._Lock;
            lock (obj2)
            {
                if (!this._NameGeneratorMap.ContainsKey(prefix))
                {
                    this._NameGeneratorMap.Add(prefix, new NameGenerator(prefix));
                }
            }
        }

        public void UnregisterNameGenerator(string prefix)
        {
            object obj2 = this._Lock;
            lock (obj2)
            {
                if (this._NameGeneratorMap.ContainsKey(prefix))
                {
                    this._NameGeneratorMap.Remove(prefix);
                }
            }
        }

        public static NameGeneratorManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new NameGeneratorManager();
                }
                return _Instance;
            }
        }
    }
}

