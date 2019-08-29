using System;

[Serializable]
public abstract class LocalSaveBase
{
    protected LocalSaveBase()
    {
    }

    protected abstract void OnRefresh();
    public void Refresh()
    {
        this.OnRefresh();
    }
}

