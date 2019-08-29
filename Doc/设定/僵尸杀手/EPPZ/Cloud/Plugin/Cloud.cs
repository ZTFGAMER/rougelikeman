namespace EPPZ.Cloud.Plugin
{
    using EPPZ.Cloud;
    using System;
    using UnityEngine;

    public class Cloud
    {
        protected ICloud cloudObject;

        public virtual bool BoolForKey(string key) => 
            false;

        public virtual void CloudDidChange(string message)
        {
        }

        public virtual float FloatForKey(string key) => 
            0f;

        public virtual void InitializeWithGameObjectName(string gameObjectName)
        {
        }

        public virtual int IntForKey(string key) => 
            0;

        protected void Log(string message)
        {
            EPPZ.Cloud.Cloud.Log(message);
        }

        public static EPPZ.Cloud.Plugin.Cloud NativePluginInstance(ICloud cloudObject)
        {
            EPPZ.Cloud.Plugin.Cloud cloud = !Application.isEditor ? ((EPPZ.Cloud.Plugin.Cloud) new Cloud_Android()) : ((EPPZ.Cloud.Plugin.Cloud) new Cloud_Editor());
            cloud.cloudObject = cloudObject;
            return cloud;
        }

        public virtual void SetBoolForKey(bool value, string key)
        {
        }

        public virtual void SetFloatForKey(float value, string key)
        {
        }

        public virtual void SetIntForKey(int value, string key)
        {
        }

        public virtual void SetStringForKey(string value, string key)
        {
        }

        public virtual string StringForKey(string key) => 
            string.Empty;

        public virtual void Synchronize()
        {
        }
    }
}

