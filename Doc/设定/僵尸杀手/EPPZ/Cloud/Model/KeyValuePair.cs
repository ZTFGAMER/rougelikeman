namespace EPPZ.Cloud.Model
{
    using EPPZ.Cloud;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Serializable]
    public class KeyValuePair
    {
        public string key;
        public Type type;
        private Dictionary<Type, System.Type> actionTypesForTypes;
        private Dictionary<int, object> onChangeActions;
        private Dictionary<Type, Action<object>> invokersForTypes;

        public KeyValuePair()
        {
            Dictionary<Type, System.Type> dictionary = new Dictionary<Type, System.Type> {
                { 
                    Type.String,
                    typeof(Action<string>)
                },
                { 
                    Type.Float,
                    typeof(Action<float>)
                },
                { 
                    Type.Int,
                    typeof(Action<int>)
                },
                { 
                    Type.Bool,
                    typeof(Action<bool>)
                }
            };
            this.actionTypesForTypes = dictionary;
            this.onChangeActions = new Dictionary<int, object>();
            Dictionary<Type, Action<object>> dictionary2 = new Dictionary<Type, Action<object>> {
                { 
                    Type.String,
                    new Action<object>(this.<KeyValuePair>m__0)
                },
                { 
                    Type.Float,
                    new Action<object>(this.<KeyValuePair>m__1)
                },
                { 
                    Type.Int,
                    new Action<object>(this.<KeyValuePair>m__2)
                },
                { 
                    Type.Bool,
                    new Action<object>(this.<KeyValuePair>m__3)
                }
            };
            this.invokersForTypes = dictionary2;
        }

        [CompilerGenerated]
        private void <KeyValuePair>m__0(object eachAction)
        {
            ((Action<string>) eachAction)(this.stringValue);
        }

        [CompilerGenerated]
        private void <KeyValuePair>m__1(object eachAction)
        {
            ((Action<float>) eachAction)(this.floatValue);
        }

        [CompilerGenerated]
        private void <KeyValuePair>m__2(object eachAction)
        {
            ((Action<int>) eachAction)(this.intValue);
        }

        [CompilerGenerated]
        private void <KeyValuePair>m__3(object eachAction)
        {
            ((Action<bool>) eachAction)(this.boolValue);
        }

        public void AddOnChangeAction(object action, int priority)
        {
            if (action.GetType() != this.actionType)
            {
                Debug.LogWarning(string.Concat(new object[] { "eppz! Cloud: Cannot add on change action for key `", this.key, "` with type `", this.type, "`. Types mismatched." }));
            }
            else
            {
                this.onChangeActions.Add(priority, action);
            }
        }

        public void InvokeOnValueChangedAction()
        {
            Debug.Log("InvokeOnValueChangedAction()");
            Debug.Log("onChangeActions.Count: " + this.onChangeActions.Count);
            if (this.onChangeActions.Count != 0)
            {
                List<int> list = this.onChangeActions.Keys.ToList<int>();
                list.Sort();
                foreach (int num in list)
                {
                    this.invoker(this.onChangeActions[num]);
                }
            }
        }

        public void RemoveOnChangeAction(object action)
        {
            <RemoveOnChangeAction>c__AnonStorey0 storey = new <RemoveOnChangeAction>c__AnonStorey0 {
                action = action
            };
            foreach (KeyValuePair<int, object> pair in Enumerable.Where<KeyValuePair<int, object>>(this.onChangeActions, new Func<KeyValuePair<int, object>, bool>(storey.<>m__0)).ToList<KeyValuePair<int, object>>())
            {
                this.onChangeActions.Remove(pair.Key);
            }
        }

        public void RemoveOnChangeActions()
        {
            this.onChangeActions.Clear();
        }

        private System.Type actionType =>
            this.actionTypesForTypes[this.type];

        private Action<object> invoker =>
            this.invokersForTypes[this.type];

        public string stringValue =>
            EPPZ.Cloud.Cloud.StringForKey(this.key);

        public float floatValue =>
            EPPZ.Cloud.Cloud.FloatForKey(this.key);

        public int intValue =>
            EPPZ.Cloud.Cloud.IntForKey(this.key);

        public bool boolValue =>
            EPPZ.Cloud.Cloud.BoolForKey(this.key);

        [CompilerGenerated]
        private sealed class <RemoveOnChangeAction>c__AnonStorey0
        {
            internal object action;

            internal bool <>m__0(KeyValuePair<int, object> keyValuePair) => 
                (keyValuePair.Value == this.action);
        }

        public enum Type
        {
            String,
            Float,
            Int,
            Bool
        }
    }
}

