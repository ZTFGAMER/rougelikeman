using Dxx.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Test_GridInfinity : MonoBehaviour
{
    public InfinityScrollGroup infinity;
    public GameObject copyItem;
    public int initItemCount;
    public int initDisplayCount;
    public InputField inputItemIndex;

    public void OnClickDisplayCountMinus()
    {
    }

    public void OnClickDisplayCountPlus()
    {
    }

    public void OnClickItemCountMinus()
    {
        this.infinity.SetItemCount(--this.initItemCount, true);
    }

    public void OnClickItemCountPlus()
    {
        this.infinity.SetItemCount(++this.initItemCount, true);
    }

    public void OnClickRefresh()
    {
        this.infinity.RefreshAll();
    }

    public void OnClickScrollTo()
    {
        int result = 0;
        if (!string.IsNullOrEmpty(this.inputItemIndex.text) && int.TryParse(this.inputItemIndex.text, out result))
        {
            this.infinity.ScrollToItem(result);
        }
    }

    private void Start()
    {
        this.infinity.RegUpdateCallback<Button>(new Action<int, Button>(this.UpdateChildCallbak));
        this.infinity.Init(this.initDisplayCount, this.initItemCount, this.copyItem);
    }

    private void UpdateChildCallbak(int index, Button obj)
    {
        obj.transform.Find("Text").GetComponent<Text>().text = $"{index:D4}";
        object[] args = new object[] { index };
        Debug.LogFormat("UpdateChildCallbak: {0}", args);
    }
}

