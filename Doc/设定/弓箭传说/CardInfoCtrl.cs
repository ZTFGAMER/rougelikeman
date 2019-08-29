using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoCtrl : MonoBehaviour
{
    public GameObject child;
    public RectTransform bgparent;
    public RectTransform arrowparent;
    public Text Text_Name;
    public Text Text_Info;
    public RectTransform left;
    public RectTransform right;
    public Animation ani;

    public void Init(CardOneCtrl ctrl)
    {
        this.child.transform.position = ctrl.transform.position;
        if (!ctrl.carddata.Unlock)
        {
            this.Text_Name.text = "?";
            this.Text_Info.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Lock", Array.Empty<object>());
        }
        else
        {
            object[] args = new object[] { ctrl.carddata.CardID };
            this.Text_Name.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("宝物名称{0}", args), Array.Empty<object>());
            string[] strArray = new string[ctrl.carddata.data.BaseAttributes.Length];
            bool flag = false;
            int index = 0;
            int length = strArray.Length;
            while (index < length)
            {
                strArray[index] = ctrl.carddata.GetNextAttribute(index);
                if (ctrl.carddata.data.BaseAttributes[index].Contains("Global_HarvestLevel"))
                {
                    flag = true;
                }
                index++;
            }
            if (flag)
            {
                object[] objArray2 = new object[] { ctrl.carddata.CardID };
                this.Text_Info.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("宝物描述{0}", objArray2), Array.Empty<object>());
            }
            else
            {
                object[] objArray3 = new object[] { ctrl.carddata.CardID };
                this.Text_Info.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("宝物描述{0}", objArray3), strArray);
            }
        }
        float num3 = 0f;
        if (this.left.position.x < 0f)
        {
            num3 = -this.left.position.x;
        }
        else if (this.right.position.x > GameLogic.DesignWidth)
        {
            num3 = GameLogic.DesignWidth - this.right.position.x;
        }
        this.bgparent.position = new Vector3(ctrl.transform.position.x + num3, ctrl.transform.position.y, 0f);
    }

    public void Show(bool value)
    {
        this.child.SetActive(value);
        if (value)
        {
            this.ani.Play("Card_InfoShow");
        }
    }
}

