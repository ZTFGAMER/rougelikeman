using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UIBossCell : MonoBehaviour
{
    public Text textName;
    public Text textKillCount;
    public Text textReward;
    public Image progressImage;
    public Button buttonClaim;
    public GameObject objDone;
    public GameObject objProgress;
    public OverlayParticle claimFx;
    public RawImage rawImage;
    private WavesManager.Boss boss;
    private KilledBosses savedBoss;
    private Camera bossCamera;
    private GameObject bossObj;
    private int index = -1;
    private UIWantedList uIWantedList;
    private int[] bossStages = new int[] { 1, 3, 5, 10 };

    public void CreateBossCam(int index)
    {
        float num = ((2f * DataLoader.gui.wantedList.bossCameraPrefab.orthographicSize) * DataLoader.gui.survivorUpgradePanel.heroCamPrefab.aspect) + 1f;
        Camera camera = UnityEngine.Object.Instantiate<Camera>(DataLoader.gui.wantedList.bossCameraPrefab, new Vector3(10000f + (index * (num * 2f)), 0f, 0f), Quaternion.identity, TransformParentManager.Instance.bossList);
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.boss.prefabBoss.gameObject, camera.transform.GetChild(0));
        foreach (Component component in obj2.gameObject.GetComponents<Component>())
        {
            if (!(component is Transform))
            {
                UnityEngine.Object.Destroy(component);
            }
        }
        RenderTexture texture = new RenderTexture(DataLoader.gui.wantedList.prefabRenderTexture);
        this.bossCamera = camera;
        this.bossObj = obj2;
        this.rawImage.set_texture(texture);
        obj2.transform.Rotate(new Vector3(0f, 180f, 0f));
        camera.targetTexture = texture;
        camera.Render();
        camera.enabled = false;
    }

    public void GetReward()
    {
        this.claimFx.Play();
        DataLoader.Instance.RefreshMoney((double) this.GetRewardCount(), false);
        this.savedBoss.rewardedStage++;
        this.savedBoss.count = 0;
        Debug.Log(string.Concat(new object[] { "GetReward - ", this.boss.prefabBoss.myNameIs, "| uiWantedList is ", this.uIWantedList }));
        this.UpdateCell(this.uIWantedList);
        if (this.uIWantedList != null)
        {
            this.uIWantedList.UpdateKillAllCells();
        }
        DataLoader.gui.wantedList.UpdateCountText();
        DataLoader.Instance.SaveAllData();
        SoundManager.Instance.PlayClickSound();
        Dictionary<string, string> eventParameters = new Dictionary<string, string> {
            { 
                "BossName",
                this.boss.prefabBoss.myNameIs
            }
        };
        AnalyticsManager.instance.LogEvent("ClaimBossReward", eventParameters);
    }

    private int GetRewardCount()
    {
        if (this.savedBoss == null)
        {
            return this.boss.reward;
        }
        switch (this.savedBoss.rewardedStage)
        {
            case 0:
                return this.boss.reward;

            case 1:
                return (int) (this.boss.reward * 0.5f);

            case 2:
                return this.boss.reward;

            case 3:
                return (this.boss.reward * 2);
        }
        return 0;
    }

    private bool IsPrevBossKilled(int _index)
    {
        for (int i = 0; i < WavesManager.instance.bossesByWorld.Length; i++)
        {
            for (int j = 0; j < WavesManager.instance.bossesByWorld[i].bosses.Length; j++)
            {
                if (_index == 0)
                {
                    for (int k = 0; k < DataLoader.playerData.killedBosses.Count; k++)
                    {
                        if (DataLoader.playerData.killedBosses[k].name == WavesManager.instance.bossesByWorld[i].bosses[j].prefabBoss.myNameIs)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                _index--;
            }
        }
        return false;
    }

    public bool IsReady()
    {
        if (this.savedBoss == null)
        {
            return false;
        }
        if (this.savedBoss.rewardedStage >= this.bossStages.Length)
        {
            return false;
        }
        return (this.savedBoss.count >= this.bossStages[this.savedBoss.rewardedStage]);
    }

    public void SetContent(WavesManager.Boss boss, int _index)
    {
        this.boss = boss;
        this.index = _index;
        this.CreateBossCam((_index + TransformParentManager.Instance.upgradePanelCams.childCount) + 10);
        this.UpdateCell(null);
    }

    public void SetShader(bool dark)
    {
        Shader shader;
        Color color;
        SkinnedMeshRenderer componentInChildren = this.bossObj.GetComponentInChildren<SkinnedMeshRenderer>();
        MeshRenderer[] componentsInChildren = this.bossObj.GetComponentsInChildren<MeshRenderer>();
        if (dark)
        {
            shader = Shader.Find("Legacy Shaders/VertexLit");
            color = new Color(0.6862745f, 0.6862745f, 0.6862745f);
        }
        else
        {
            shader = Shader.Find("Outlined/Silhouetted Unlit");
            color = new Color(1f, 1f, 1f);
        }
        componentInChildren.material = new Material(componentInChildren.material);
        componentInChildren.material.shader = shader;
        componentInChildren.material.color = color;
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            componentsInChildren[i].material = new Material(componentsInChildren[i].material);
            componentsInChildren[i].material.shader = shader;
            componentsInChildren[i].material.color = color;
        }
        this.bossCamera.Render();
    }

    public void UpdateCell(UIWantedList uIWantedList = null)
    {
        this.uIWantedList = uIWantedList;
        this.textName.set_font(LanguageManager.instance.currentLanguage.font);
        this.savedBoss = DataLoader.playerData.killedBosses.Find(kb => kb.name == this.boss.prefabBoss.myNameIs);
        this.textReward.text = $"{this.GetRewardCount():N0}";
        if (this.savedBoss != null)
        {
            this.textKillCount.text = (this.bossStages[this.savedBoss.rewardedStage] - (this.bossStages[this.savedBoss.rewardedStage] - this.savedBoss.count)) + "/" + this.bossStages[this.savedBoss.rewardedStage];
            this.progressImage.fillAmount = ((float) (this.bossStages[this.savedBoss.rewardedStage] - (this.bossStages[this.savedBoss.rewardedStage] - this.savedBoss.count))) / ((float) this.bossStages[this.savedBoss.rewardedStage]);
            this.buttonClaim.gameObject.SetActive(this.IsReady());
            this.objDone.SetActive(this.savedBoss.rewardedStage >= this.bossStages.Length);
            this.textReward.gameObject.SetActive(!this.objDone.activeInHierarchy);
            this.objProgress.SetActive(this.textReward.gameObject.activeInHierarchy);
            this.SetShader(false);
            this.textName.text = LanguageManager.instance.GetLocalizedText(this.boss.prefabBoss.myNameIs);
        }
        else
        {
            this.progressImage.fillAmount = 0f;
            this.textKillCount.text = "0/" + this.bossStages[0];
            this.buttonClaim.gameObject.SetActive(false);
            this.objDone.SetActive(false);
            this.objProgress.SetActive(true);
            this.textReward.gameObject.SetActive(true);
            this.SetShader(!this.IsPrevBossKilled(this.index - 1));
            if ((this.index == 0) || this.IsPrevBossKilled(this.index - 1))
            {
                this.textName.text = LanguageManager.instance.GetLocalizedText(this.boss.prefabBoss.myNameIs);
                this.SetShader(false);
            }
            else if (GameManager.instance.IsWorldOpen(2) && (this.index == WavesManager.instance.bossesByWorld[0].bosses.Length))
            {
                this.textName.text = LanguageManager.instance.GetLocalizedText(this.boss.prefabBoss.myNameIs);
                this.SetShader(false);
            }
            else
            {
                this.textName.text = "????";
                this.SetShader(true);
            }
        }
    }
}

