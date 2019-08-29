using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleLevelWaveCtrl : MonoBehaviour
{
    public GameObject child;
    public Text Text_Wave;
    private BattleLevelWaveData mData;
    private float starttime;

    public void Deinit()
    {
    }

    private void set_time(int time)
    {
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("battle_level_wave", Array.Empty<object>());
        string str2 = GameLogic.Hold.Language.GetLanguageByTID("battle_level_nexttime", Array.Empty<object>());
        object[] args = new object[] { languageByTID, this.mData.currentwave, this.mData.maxwave, str2, GameLogic.Hold.Language.GetSecond(time) };
        this.Text_Wave.text = Utils.FormatString("{0}:{1}/{2}     {3}:{4}", args);
        if (time < 0)
        {
            this.mData.currentwave++;
            if (this.mData.is_last_wave())
            {
                this.mData.showui = false;
                this.SetActive(false);
            }
            else
            {
                this.SetInfo(this.mData);
            }
        }
    }

    public void SetActive(bool value)
    {
        this.child.SetActive(value);
    }

    public void SetInfo(BattleLevelWaveData data)
    {
        this.mData = data;
        this.starttime = Updater.AliveTime;
        this.SetActive(data.showui);
        if (data.showui)
        {
            this.Update();
        }
    }

    private void Update()
    {
        if (this.mData.showui)
        {
            float num = Updater.AliveTime - this.starttime;
            float num2 = this.mData.lasttime - num;
            this.set_time(MathDxx.CeilToInt(num2));
        }
    }
}

