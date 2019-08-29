using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class PropOneBase : MonoBehaviour
{
    public ButtonCtrl mButton;
    public Image Image_BG;
    public Image Image_Icon;
    public Text Text_Value;
    public Text Text_Content;
    public Image Image_Type;
    public Image Image_QualityGold;
    private Text Text_Button;
    protected PropOneEquip.Transfer data;
    public Action<PropOneBase, object> OnClickEvent;

    private void Awake()
    {
        this.mButton.onClick = new Action(this.OnClickBase);
        this.Text_Button = this.mButton.GetComponent<Text>();
        this.OnAwake();
    }

    private void Init(PropOneEquip.Transfer data)
    {
        this.data = data;
        this.OnInit();
    }

    public void InitCurrency(int id, long count)
    {
        PropOneEquip.Transfer transfer = new PropOneEquip.Transfer {
            type = PropType.eCurrency
        };
        PropOneEquip.CurrencyData data = new PropOneEquip.CurrencyData {
            id = id,
            count = count
        };
        transfer.data = data;
        this.Init(transfer);
    }

    public void InitEquip(int id, int count)
    {
        PropOneEquip.Transfer transfer = new PropOneEquip.Transfer {
            type = PropType.eEquip
        };
        PropOneEquip.EquipData data = new PropOneEquip.EquipData {
            id = id,
            count = count
        };
        transfer.data = data;
        this.Init(transfer);
    }

    public void InitProp(Drop_DropModel.DropData data)
    {
        if (data.type == PropType.eCurrency)
        {
            this.InitCurrency(data.id, (long) data.count);
        }
        else if (data.type == PropType.eEquip)
        {
            this.InitEquip(data.id, data.count);
        }
    }

    protected virtual void OnAwake()
    {
    }

    private void OnClickBase()
    {
        if (this.OnClickEvent != null)
        {
            this.OnClickEvent(this, this.data);
        }
        this.OnClicked();
    }

    protected virtual void OnClicked()
    {
    }

    protected virtual void OnInit()
    {
    }

    public void SetButtonEnable(bool value)
    {
        this.mButton.enabled = value;
        if (this.Text_Button != null)
        {
            this.Text_Button.enabled = value;
        }
    }
}

