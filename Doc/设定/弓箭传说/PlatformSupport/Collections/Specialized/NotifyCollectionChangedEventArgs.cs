namespace PlatformSupport.Collections.Specialized
{
    using System;
    using System.Collections;

    public class NotifyCollectionChangedEventArgs : EventArgs
    {
        private NotifyCollectionChangedAction _action;
        private IList _newItems;
        private IList _oldItems;
        private int _newStartingIndex;
        private int _oldStartingIndex;

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Reset)
            {
                throw new ArgumentException("action");
            }
            this.InitializeAdd(action, null, -1);
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)) && (action != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException("action");
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItems != null)
                {
                    throw new ArgumentException("action");
                }
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                if (changedItems == null)
                {
                    throw new ArgumentNullException("changedItems");
                }
                this.InitializeAddOrRemove(action, changedItems, -1);
            }
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)) && (action != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException("action");
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItem != null)
                {
                    throw new ArgumentException("action");
                }
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                object[] changedItems = new object[] { changedItem };
                this.InitializeAddOrRemove(action, changedItems, -1);
            }
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException("action");
            }
            if (newItems == null)
            {
                throw new ArgumentNullException("newItems");
            }
            if (oldItems == null)
            {
                throw new ArgumentNullException("oldItems");
            }
            this.InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)) && (action != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException("action");
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItems != null)
                {
                    throw new ArgumentException("action");
                }
                if (startingIndex != -1)
                {
                    throw new ArgumentException("action");
                }
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                if (changedItems == null)
                {
                    throw new ArgumentNullException("changedItems");
                }
                if (startingIndex < -1)
                {
                    throw new ArgumentException("startingIndex");
                }
                this.InitializeAddOrRemove(action, changedItems, startingIndex);
            }
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)) && (action != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException("action");
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItem != null)
                {
                    throw new ArgumentException("action");
                }
                if (index != -1)
                {
                    throw new ArgumentException("action");
                }
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                object[] changedItems = new object[] { changedItem };
                this.InitializeAddOrRemove(action, changedItems, index);
            }
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException("action");
            }
            object[] newItems = new object[] { newItem };
            object[] oldItems = new object[] { oldItem };
            this.InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException("action");
            }
            if (newItems == null)
            {
                throw new ArgumentNullException("newItems");
            }
            if (oldItems == null)
            {
                throw new ArgumentNullException("oldItems");
            }
            this.InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex);
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Move)
            {
                throw new ArgumentException("action");
            }
            if (index < 0)
            {
                throw new ArgumentException("index");
            }
            this.InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Move)
            {
                throw new ArgumentException("action");
            }
            if (index < 0)
            {
                throw new ArgumentException("index");
            }
            object[] newItems = new object[] { changedItem };
            this.InitializeMoveOrReplace(action, newItems, newItems, index, oldIndex);
        }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException("action");
            }
            object[] newItems = new object[] { newItem };
            object[] oldItems = new object[] { oldItem };
            this.InitializeMoveOrReplace(action, newItems, oldItems, index, index);
        }

        internal NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int newIndex, int oldIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            this._action = action;
            this._newItems = (newItems != null) ? new ReadOnlyList(newItems) : null;
            this._oldItems = (oldItems != null) ? new ReadOnlyList(oldItems) : null;
            this._newStartingIndex = newIndex;
            this._oldStartingIndex = oldIndex;
        }

        private void InitializeAdd(NotifyCollectionChangedAction action, IList newItems, int newStartingIndex)
        {
            this._action = action;
            this._newItems = (newItems != null) ? new ReadOnlyList(newItems) : null;
            this._newStartingIndex = newStartingIndex;
        }

        private void InitializeAddOrRemove(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
        {
            if (action == NotifyCollectionChangedAction.Add)
            {
                this.InitializeAdd(action, changedItems, startingIndex);
            }
            else if (action == NotifyCollectionChangedAction.Remove)
            {
                this.InitializeRemove(action, changedItems, startingIndex);
            }
        }

        private void InitializeMoveOrReplace(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, int oldStartingIndex)
        {
            this.InitializeAdd(action, newItems, startingIndex);
            this.InitializeRemove(action, oldItems, oldStartingIndex);
        }

        private void InitializeRemove(NotifyCollectionChangedAction action, IList oldItems, int oldStartingIndex)
        {
            this._action = action;
            this._oldItems = (oldItems != null) ? new ReadOnlyList(oldItems) : null;
            this._oldStartingIndex = oldStartingIndex;
        }

        public NotifyCollectionChangedAction Action =>
            this._action;

        public IList NewItems =>
            this._newItems;

        public IList OldItems =>
            this._oldItems;

        public int NewStartingIndex =>
            this._newStartingIndex;

        public int OldStartingIndex =>
            this._oldStartingIndex;
    }
}

