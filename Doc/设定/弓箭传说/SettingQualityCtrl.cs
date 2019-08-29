using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingQualityCtrl : MonoBehaviour
{
    private static Dictionary<int, string> mQualityHigh;
    private static Dictionary<int, string> mQualityLow;
    private bool bFlagship;
    public ButtonCtrl Button_Quality;
    public Text Text_QualityContent;
    public Text Text_Quality;

    static SettingQualityCtrl()
    {
        Dictionary<int, string> dictionary = new Dictionary<int, string> {
            { 
                1,
                "设置_画面质量_低"
            },
            { 
                2,
                "设置_画面质量_中"
            },
            { 
                3,
                "设置_画面质量_高"
            }
        };
        mQualityHigh = dictionary;
        dictionary = new Dictionary<int, string> {
            { 
                1,
                "设置_画面质量_低"
            },
            { 
                2,
                "设置_画面质量_中"
            }
        };
        mQualityLow = dictionary;
    }

    private void Awake()
    {
        this.bFlagship = PlatformHelper.GetFlagShip();
        this.Button_Quality.onClick = new Action(this.OnClickButton);
        this.UpdateShow();
    }

    private string GetQualityString(int qualityid)
    {
        if (this.bFlagship)
        {
            return mQualityHigh[qualityid];
        }
        return mQualityLow[qualityid];
    }

    private void OnClickButton()
    {
        if (this.bFlagship)
        {
            if (GameLogic.QualityID == 3)
            {
                GameLogic.QualityID = 1;
            }
            else
            {
                GameLogic.QualityID++;
            }
        }
        else
        {
            GameLogic.QualityID = 3 - GameLogic.QualityID;
        }
        this.UpdateShow();
    }

    public void UpdateLanguage()
    {
        this.UpdateShow();
    }

    private void UpdateShow()
    {
        this.Text_QualityContent.text = GameLogic.Hold.Language.GetLanguageByTID("设置_画面质量", Array.Empty<object>());
        this.Text_Quality.text = GameLogic.Hold.Language.GetLanguageByTID(this.GetQualityString(GameLogic.QualityID), Array.Empty<object>());
    }
}

