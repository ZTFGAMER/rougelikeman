using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class UIPresentController : MonoBehaviour
{
    [SerializeField]
    private List<PresentInfo> presents;
    [CompilerGenerated]
    private static Func<PresentInfo, int> <>f__am$cache0;

    public PresentInfo ChoosePresent()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = c => c.weight;
        }
        int max = Enumerable.Sum<PresentInfo>(this.presents, <>f__am$cache0);
        int num2 = UnityEngine.Random.Range(0, max);
        int num3 = 0;
        foreach (PresentInfo info in this.presents)
        {
            for (int i = num3; i < (info.weight + num3); i++)
            {
                if (i >= num2)
                {
                    return info;
                }
            }
            num3 += info.weight;
        }
        Debug.LogWarning("Something Wrong With Present Weights");
        return this.presents.First<PresentInfo>();
    }

    public void TryToShowPresent(int money)
    {
        if (GameManager.instance.inGameTime > 30)
        {
            DataLoader.gui.ChangeAnimationState("Present");
            for (int i = 0; i < this.presents.Count; i++)
            {
                PresentInfo info = this.presents[i];
                info.present.gameObject.SetActive(false);
            }
            this.ChoosePresent().present.SetContent(money);
        }
        else
        {
            DataLoader.gui.ChangeAnimationState("GameOver");
        }
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct PresentInfo
    {
        public UIPresent present;
        public int weight;
    }
}

