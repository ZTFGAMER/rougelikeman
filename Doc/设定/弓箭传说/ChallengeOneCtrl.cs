using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeOneCtrl : MonoBehaviour
{
    public Text Text_ID;
    public GameObject dots;
    private int mIndex;
    private Stage_Level_activity mData;

    public void Init(int index, Stage_Level_activity data, int allcount)
    {
        this.mIndex = index;
        this.mData = data;
        this.Text_ID.text = (this.mData.ID - 0x834).ToString();
        bool flag = index < (allcount - 1);
        if (flag != this.dots.activeSelf)
        {
            this.dots.SetActive(flag);
        }
    }
}

