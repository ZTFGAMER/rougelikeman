using System;

public class ButtonGoldCtrl : ButtonCtrl
{
    public GoldTextCtrl mGoldCtrl;
    private bool bRed = true;

    public void SetCanRed(bool value)
    {
        this.bRed = value;
    }

    public void SetCurrency(CurrencyType type)
    {
        if (this.mGoldCtrl != null)
        {
            this.mGoldCtrl.SetCurrencyType(type);
        }
    }

    public void SetCurrency(int type)
    {
        this.SetCurrency((CurrencyType) type);
    }

    public override void SetEnable(bool value)
    {
        base.SetEnable(value);
        if ((this.mGoldCtrl != null) && this.bRed)
        {
            this.mGoldCtrl.SetButtonEnable(value);
        }
    }

    public void SetGold(int gold)
    {
        if (this.mGoldCtrl != null)
        {
            this.mGoldCtrl.SetValue(gold);
            if (this.bRed)
            {
                this.mGoldCtrl.SetButtonEnable(base.bEnable);
            }
        }
    }
}

