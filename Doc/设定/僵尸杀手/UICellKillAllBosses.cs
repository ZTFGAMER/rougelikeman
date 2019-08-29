using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICellKillAllBosses : MonoBehaviour
{
    public Text textKill;
    public Text textReward;
    public Text textWorldName;
    public Text progressText;
    public Button btnClaim;
    public Image progresImage;
    public GameObject objDone;
    public OverlayParticle claimFx;
    private WavesManager.Bosses bosses;
    private int worldIndex;

    public int GetKilledBossesCount()
    {
        int num = 0;
        for (int i = 0; i < this.bosses.bosses.Length; i++)
        {
            foreach (KilledBosses bosses in DataLoader.playerData.killedBosses)
            {
                if ((bosses.name == this.bosses.bosses[i].prefabBoss.myNameIs) && (bosses.rewardedStage > 0))
                {
                    num++;
                    break;
                }
            }
        }
        return num;
    }

    public void GetReward()
    {
        PlayerPrefs.SetInt(StaticConstants.allBossesRewardedkey + this.worldIndex, 1);
        this.claimFx.Play();
        this.UpdateContent();
        DataLoader.gui.wantedList.UpdateCountText();
        SoundManager.Instance.PlayClickSound();
        DataLoader.Instance.RefreshMoney((double) this.bosses.allBossesReward, true);
        Debug.Log("AllBossesReward: " + GameManager.instance.worldNames[this.worldIndex]);
        Dictionary<string, string> eventParameters = new Dictionary<string, string> {
            { 
                "World",
                GameManager.instance.worldNames[this.worldIndex]
            }
        };
        AnalyticsManager.instance.LogEvent("ClaimAllBossesReward", eventParameters);
    }

    public bool IsReady() => 
        (!PlayerPrefs.HasKey(StaticConstants.allBossesRewardedkey + this.worldIndex) && (this.GetKilledBossesCount() >= this.bosses.bosses.Length));

    public void SetContent(WavesManager.Bosses bosses, int index)
    {
        this.bosses = bosses;
        this.worldIndex = index;
        this.textReward.text = $"{bosses.allBossesReward:N0}";
        this.UpdateContent();
    }

    public void UpdateContent()
    {
        Debug.Log("UpdateContent " + this.textWorldName.text);
        this.textKill.text = LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Kill_all_bosses);
        this.textKill.set_font(LanguageManager.instance.currentLanguage.font);
        this.textWorldName.text = LanguageManager.instance.GetLocalizedText(GameManager.instance.worldNames[this.worldIndex]);
        this.textWorldName.set_font(LanguageManager.instance.currentLanguage.font);
        int killedBossesCount = this.GetKilledBossesCount();
        this.progressText.text = killedBossesCount + "/" + this.bosses.bosses.Length;
        this.progresImage.fillAmount = ((float) killedBossesCount) / ((float) this.bosses.bosses.Length);
        if (this.progresImage.fillAmount >= 1f)
        {
            this.btnClaim.gameObject.SetActive(!PlayerPrefs.HasKey(StaticConstants.allBossesRewardedkey + this.worldIndex));
            this.objDone.SetActive(!this.btnClaim.gameObject.activeInHierarchy);
            this.textReward.gameObject.SetActive(this.btnClaim.gameObject.activeInHierarchy);
            this.progresImage.transform.parent.gameObject.SetActive(this.btnClaim.gameObject.activeInHierarchy);
        }
        else
        {
            this.progresImage.transform.parent.gameObject.SetActive(true);
            this.objDone.SetActive(false);
            this.btnClaim.gameObject.SetActive(false);
            this.textReward.gameObject.SetActive(true);
        }
        this.btnClaim.interactable = !PlayerPrefs.HasKey(StaticConstants.allBossesRewardedkey + this.worldIndex);
    }
}

