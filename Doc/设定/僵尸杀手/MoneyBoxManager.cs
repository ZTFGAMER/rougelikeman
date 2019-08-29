using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MoneyBoxManager : MonoBehaviour
{
    public static MoneyBoxManager instance;
    [HideInInspector]
    private MoneyBoxSpawn[] places;
    [SerializeField]
    private GameObject prefabMoneyBox;
    [NonSerialized]
    public bool moneyBoxPicked;
    private int randomPlace;
    [NonSerialized]
    public string currentHelpText = string.Empty;

    private void Awake()
    {
        instance = this;
    }

    private bool CanSpawnBox()
    {
        DateTime date;
        if (GameManager.instance.isTutorialNow)
        {
            return false;
        }
        if (!TimeManager.gotDateTime)
        {
            DataLoader.gui.secretAnimator.SetBool("IsOpened", false);
            return false;
        }
        if (PlayerPrefs.HasKey(StaticConstants.MoneyBoxKey))
        {
            DateTime time2 = new DateTime(Convert.ToInt64(PlayerPrefs.GetString(StaticConstants.MoneyBoxKey)), DateTimeKind.Utc);
            date = time2.Date;
        }
        else
        {
            if (StaticConstants.NeedInternetConnection)
            {
                return StaticConstants.IsConnectedToInternet();
            }
            return true;
        }
        if (DateTime.Compare(TimeManager.CurrentDateTime.Date, date) == 0)
        {
            UnityEngine.Debug.Log("You already got MoneyBox today");
            return false;
        }
        return true;
    }

    public void EndGame()
    {
        MoneyBox[] boxArray = UnityEngine.Object.FindObjectsOfType<MoneyBox>();
        for (int i = 0; i < boxArray.Length; i++)
        {
            UnityEngine.Object.Destroy(boxArray[i].gameObject);
        }
    }

    private void Start()
    {
    }

    public void StartGame()
    {
        this.places = UnityEngine.Object.FindObjectsOfType<MoneyBoxSpawn>();
        bool flag = false;
        foreach (MoneyBoxSpawn spawn in this.places)
        {
            if ((spawn.openAtLevel <= DataLoader.Instance.GetCurrentPlayerLevel()) && (spawn.worldNumber == GameManager.instance.currentWorldNumber))
            {
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            UnityEngine.Debug.LogError("Compatible places for spawn MoneyBox Not Found!");
        }
        else
        {
            do
            {
                this.randomPlace = UnityEngine.Random.Range(0, this.places.Length);
            }
            while ((this.places[this.randomPlace].openAtLevel > DataLoader.Instance.GetCurrentPlayerLevel()) || (this.places[this.randomPlace].worldNumber != GameManager.instance.currentWorldNumber));
            UnityEngine.Object.Instantiate<GameObject>(this.prefabMoneyBox, this.places[this.randomPlace].transform.position, new Quaternion(), TransformParentManager.Instance.moneyBox).GetComponent<MoneyBox>().isBigCoin = true;
        }
    }

    public void TrySpawnBox()
    {
    }

    public void UpdateSecret()
    {
        if (!this.CanSpawnBox())
        {
            DataLoader.gui.newSecret.SetActive(false);
            this.moneyBoxPicked = true;
            this.currentHelpText = LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.You_already_got_moneybox_today);
        }
        else
        {
            this.currentHelpText = LanguageManager.instance.GetLocalizedText(this.places[this.randomPlace].helpText);
            DataLoader.gui.newSecret.SetActive(true);
        }
    }

    [DebuggerHidden]
    public IEnumerator WaitForGui() => 
        new <WaitForGui>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <WaitForGui>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal MoneyBoxManager $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                case 1:
                    if (!DataLoader.initialized)
                    {
                        this.$current = new WaitForSeconds(0.5f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    this.$this.TrySpawnBox();
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }
}

