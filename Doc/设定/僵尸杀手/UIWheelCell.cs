using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UIWheelCell : MonoBehaviour
{
    public RectTransform rectTransform;
    [SerializeField]
    private Text boosterCountText;
    [SerializeField]
    private Text coinsCountText;
    [SerializeField]
    private Image boosterImage;
    [SerializeField]
    private Image coinImage;
    private int rewardAmount;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private WheelCellType <type>k__BackingField;
    private SaveData.BoostersData.BoosterType boosterType;

    public void SaveReward()
    {
        switch (this.type)
        {
            case WheelCellType.Booster:
                DataLoader.Instance.BuyBoosters(this.boosterType, this.rewardAmount);
                break;

            case WheelCellType.Coin:
                DataLoader.Instance.RefreshMoney((double) this.rewardAmount, false);
                break;
        }
        DataLoader.Instance.SaveAllData();
    }

    public void SetBoosters(int amount, SaveData.BoostersData.BoosterType _type)
    {
        this.rewardAmount = amount;
        this.boosterImage.gameObject.SetActive(true);
        this.coinImage.gameObject.SetActive(false);
        this.boosterCountText.text = this.rewardAmount.ToString();
        this.boosterType = _type;
        this.type = WheelCellType.Booster;
        if (_type != SaveData.BoostersData.BoosterType.NewSurvivor)
        {
            if (_type == SaveData.BoostersData.BoosterType.KillAll)
            {
                this.boosterImage.set_sprite(DataLoader.gui.multiplyImages.activeBoosters[1]);
            }
        }
        else
        {
            this.boosterImage.set_sprite(DataLoader.gui.multiplyImages.activeBoosters[0]);
        }
    }

    public void SetMoney(int amount, bool doubleReward = false)
    {
        if (amount == 0)
        {
            amount = !doubleReward ? 100 : 200;
        }
        this.rewardAmount = amount;
        this.coinImage.gameObject.SetActive(true);
        this.boosterImage.gameObject.SetActive(false);
        this.coinsCountText.text = this.rewardAmount.ToString();
        this.type = WheelCellType.Coin;
    }

    public WheelCellType type { get; private set; }
}

