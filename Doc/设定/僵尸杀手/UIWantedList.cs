using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIWantedList : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform killAllBossesPrefab;
    public RectTransform bossCellPrefab;
    public Text newCountText;
    public GameObject countObj;
    public RenderTexture prefabRenderTexture;
    private int count;
    public Camera bossCameraPrefab;
    private List<UIBossCell> bossCells = new List<UIBossCell>();
    private List<UICellKillAllBosses> killAllCells = new List<UICellKillAllBosses>();

    public void CreateCells()
    {
        WavesManager.Bosses[] bossesByWorld = WavesManager.instance.bossesByWorld;
        float num = 0f;
        float num2 = 15f;
        float y = this.killAllBossesPrefab.rect.y - num2;
        for (int i = 0; i < bossesByWorld.Length; i++)
        {
            RectTransform transform = UnityEngine.Object.Instantiate<RectTransform>(this.killAllBossesPrefab, this.scrollRect.get_content());
            transform.anchoredPosition = new Vector2(transform.anchoredPosition.x, y);
            y -= (transform.rect.height * transform.localScale.y) + num2;
            this.killAllCells.Add(transform.GetComponent<UICellKillAllBosses>());
            this.killAllCells.Last<UICellKillAllBosses>().SetContent(bossesByWorld[i], i);
            for (int j = 0; j < bossesByWorld[i].bosses.Length; j++)
            {
                RectTransform transform2 = UnityEngine.Object.Instantiate<RectTransform>(this.bossCellPrefab, this.scrollRect.get_content());
                transform2.anchoredPosition = new Vector2(transform2.anchoredPosition.x, y);
                y -= (transform2.rect.height * transform2.localScale.y) + num2;
                this.bossCells.Add(transform2.GetComponent<UIBossCell>());
                this.bossCells.Last<UIBossCell>().SetContent(bossesByWorld[i].bosses[j], j + (i * bossesByWorld[i].bosses.Length));
            }
            num += (this.killAllBossesPrefab.sizeDelta.y + num2) + ((this.bossCellPrefab.sizeDelta.y + num2) * bossesByWorld[i].bosses.Length);
        }
        this.scrollRect.get_content().sizeDelta = new Vector2(this.scrollRect.get_content().sizeDelta.x, num + num2);
        base.Invoke("UpdateCountText", Time.deltaTime);
    }

    public void OnEnable()
    {
        AnalyticsManager.instance.LogEvent("WantedListOpened", new Dictionary<string, string>());
        this.UpdateAll();
    }

    public void OpenPopup()
    {
        DataLoader.gui.popUpsPanel.OpenPopup();
        base.gameObject.SetActive(true);
        this.scrollRect.get_content().anchoredPosition = new Vector2(this.scrollRect.get_content().anchoredPosition.x, (float) (0x5e6 * (GameManager.instance.currentWorldNumber - 1)));
    }

    public void UpdateAll()
    {
        this.UpdateBossCells();
        this.UpdateKillAllCells();
        this.UpdateCountText();
    }

    public void UpdateBossCells()
    {
        this.count = 0;
        for (int i = 0; i < this.bossCells.Count; i++)
        {
            this.bossCells[i].UpdateCell(this);
        }
    }

    public void UpdateCountText()
    {
        this.count = 0;
        for (int i = 0; i < this.killAllCells.Count; i++)
        {
            if (this.killAllCells[i].IsReady())
            {
                this.count++;
            }
        }
        for (int j = 0; j < this.bossCells.Count; j++)
        {
            if (this.bossCells[j].IsReady())
            {
                this.count++;
            }
        }
        this.countObj.SetActive(this.count > 0);
        this.newCountText.text = this.count.ToString();
    }

    public void UpdateKillAllCells()
    {
        Debug.Log("UpdateKillAllCells");
        this.count = 0;
        for (int i = 0; i < this.killAllCells.Count; i++)
        {
            this.killAllCells[i].UpdateContent();
        }
    }
}

