using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UnlockStageLevelCtrl : MonoBehaviour
{
    public GameObject stageparent;
    private GameObject stageitem;

    public void Init(int stage)
    {
        if (this.stageitem != null)
        {
            Object.Destroy(this.stageitem);
        }
        object[] args = new object[] { stage };
        this.stageitem = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/MainUI/Stage/Stage{0:D2}", args)));
        this.stageitem.SetParentNormal(this.stageparent);
        Image[] componentsInChildren = this.stageitem.GetComponentsInChildren<Image>();
        int index = 0;
        int length = componentsInChildren.Length;
        while (index < length)
        {
            componentsInChildren[index].set_material((Material) null);
            index++;
        }
    }
}

