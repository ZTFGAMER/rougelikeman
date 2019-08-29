using System;
using System.Collections.Generic;

public class GameOverEvent
{
    private readonly List<Action<int>> _callbacks = new List<Action<int>>();

    public void Publish(int value)
    {
        foreach (Action<int> action in this._callbacks)
        {
            action(value);
        }
    }

    public void Subscribe(Action<int> callback)
    {
        this._callbacks.Add(callback);
    }

    public void UnSubscribe(Action<int> callback)
    {
        this._callbacks.Remove(callback);
    }
}

