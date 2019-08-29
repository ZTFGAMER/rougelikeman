using System;
using System.Collections.Generic;

public class NewRoundEvent
{
    private readonly List<Action> _callbacks = new List<Action>();

    public void Publish()
    {
        foreach (Action action in this._callbacks)
        {
            action();
        }
    }

    public void Subscribe(Action callback)
    {
        this._callbacks.Add(callback);
    }

    public void UnSubscribe(Action callback)
    {
        this._callbacks.Remove(callback);
    }
}

