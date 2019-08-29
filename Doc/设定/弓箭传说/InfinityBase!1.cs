using Dxx.UI;
using System;
using UnityEngine;

public class InfinityBase<T> : MonoBehaviour where T: Component
{
    public InfinityScrollGroup infinity;
    public GameObject copyItem;
    public int initDisplayCount;
    public Action<int, T> updatecallback;

    public InfinityBase()
    {
        this.initDisplayCount = 40;
    }

    public void Init(int itemcount)
    {
        this.infinity.RegUpdateCallback<T>(new Action<int, T>(this.UpdateChildCallbak));
        this.infinity.Init(this.initDisplayCount, itemcount, this.copyItem);
    }

    public void Refresh()
    {
        this.infinity.RefreshAll();
    }

    public void SetItemCount(int itemcount)
    {
        this.infinity.SetItemCount(itemcount, true);
    }

    private void UpdateChildCallbak(int index, T data)
    {
        if (this.updatecallback != null)
        {
            this.updatecallback(index, data);
        }
    }
}

