using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject firstMoveUI;
    [SerializeField]
    private GameObject MoveUI;
    [SerializeField]
    private GameObject KillZombiesUI;
    [SerializeField]
    private GameObject FindSurvUI;
    [SerializeField]
    private NewSurvivorPointer survivorPointer;
    [SerializeField]
    private float tutorialWavePower = 0.4f;
    [SerializeField]
    private float tutorialSecondWavePower = 0.05f;
    [SerializeField]
    private int pauseTime = 3;
    [SerializeField]
    private Button buttonOK;
    private Text buttonOkText;
    private Transform cameraTarget;
    private float movedDistance;
    private Vector3 prevSurvPos;
    private int tutorialStage = -1;
    private bool haterSpawned;
    private float defaultPointerDistance;
    private bool tutorPrepared;

    public void Continue()
    {
        this.buttonOK.gameObject.SetActive(false);
        switch (this.tutorialStage)
        {
            case 1:
                this.KillZombiesUI.SetActive(false);
                break;

            case 2:
                this.FindSurvUI.SetActive(false);
                break;
        }
        Time.timeScale = 1f;
    }

    [DebuggerHidden]
    private IEnumerator Pause() => 
        new <Pause>c__Iterator1 { $this = this };

    private void Start()
    {
        base.StartCoroutine(this.WaitForScene());
    }

    public void StartTutorial()
    {
        UnityEngine.Debug.LogWarning("DON'T CHANGE DEFAULT POSITION OF CameraTarget ON MAIN SCENE");
        this.cameraTarget = UnityEngine.Object.FindObjectOfType<CameraTarget>().transform;
        this.firstMoveUI.SetActive(true);
        this.defaultPointerDistance = this.survivorPointer.showDistance;
        DataLoader.Instance.ResetLocalInfo();
        this.buttonOkText = this.buttonOK.GetComponentInChildren<Text>();
        this.tutorPrepared = true;
    }

    private void Update()
    {
        if (this.tutorPrepared)
        {
            switch ((this.tutorialStage + 1))
            {
                case 0:
                    if (this.cameraTarget.position != Vector3.zero)
                    {
                        this.tutorialStage++;
                        this.prevSurvPos = this.cameraTarget.position;
                    }
                    break;

                case 1:
                    if (this.movedDistance <= 7f)
                    {
                        if (this.movedDistance > 0.35f)
                        {
                            this.firstMoveUI.SetActive(false);
                            if (!this.haterSpawned)
                            {
                                SpawnManager.instance.TutorialDrop();
                                this.haterSpawned = true;
                            }
                        }
                        this.movedDistance += Vector3.Distance(this.prevSurvPos, this.cameraTarget.position);
                        this.prevSurvPos = this.cameraTarget.position;
                        break;
                    }
                    this.tutorialStage++;
                    base.StartCoroutine(this.Pause());
                    WavesManager.instance.SpawnTutorialWave(this.tutorialWavePower, 1);
                    return;

                case 2:
                    if (GameManager.instance.zombies.Count <= 0)
                    {
                        this.tutorialStage++;
                        base.StartCoroutine(this.Pause());
                    }
                    break;

                case 3:
                {
                    List<Vector3> list = new List<Vector3>();
                    foreach (SurvivorSpawn spawn in SpawnManager.instance.survivorSpawns)
                    {
                        if (spawn.newSurvivor != null)
                        {
                            list.Add(spawn.transform.position);
                        }
                    }
                    if ((list.Count <= 0) && (GameManager.instance.zombies.Count <= 0))
                    {
                        this.tutorialStage++;
                        return;
                    }
                    this.survivorPointer.showDistance = 10000f;
                    break;
                }
                case 4:
                    DataLoader.Instance.SetPlayerLevel(2);
                    DataLoader.Instance.RefreshMoney(200.0, true);
                    this.survivorPointer.showDistance = this.defaultPointerDistance;
                    this.tutorialStage++;
                    this.FindSurvUI.SetActive(false);
                    GameManager.instance.GameOver();
                    UnityEngine.Object.Destroy(base.gameObject);
                    break;
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator WaitForScene() => 
        new <WaitForScene>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <Pause>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int <pauseTimeRemaining>__0;
        internal float <i>__1;
        internal Tutorial $this;
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
                    this.$current = new WaitForSecondsRealtime(1.5f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_02F8;

                case 1:
                    switch (this.$this.tutorialStage)
                    {
                        case 1:
                            this.$this.KillZombiesUI.SetActive(true);
                            goto Label_009C;

                        case 2:
                            this.$this.FindSurvUI.SetActive(true);
                            goto Label_009C;
                    }
                    break;

                case 2:
                    goto Label_0282;

                case 3:
                    this.<i>__1 = this.$this.pauseTime;
                    goto Label_0232;

                case 4:
                    this.<i>__1 -= 0.08f;
                    goto Label_0232;

                default:
                    goto Label_02F6;
            }
        Label_009C:
            SoundManager.Instance.PlayStepsSound(false);
            Time.timeScale = 0f;
            this.<pauseTimeRemaining>__0 = this.$this.pauseTime;
            this.$this.buttonOK.gameObject.SetActive(true);
            this.$this.buttonOK.interactable = false;
            this.$this.buttonOkText.set_color(this.$this.buttonOkText.get_color() - new Color(0f, 0f, 0f, 0.5f));
            this.$this.buttonOkText.text = this.<pauseTimeRemaining>__0.ToString();
            this.$current = new WaitForSecondsRealtime(0.2f);
            if (!this.$disposing)
            {
                this.$PC = 2;
            }
            goto Label_02F8;
        Label_0232:
            if (this.<i>__1 > (this.$this.pauseTime - 1))
            {
                this.$this.buttonOkText.transform.localScale = Vector3.Lerp(this.$this.buttonOkText.transform.localScale, Vector3.zero, 0.05f);
                this.$current = new WaitForSecondsRealtime(0.02f);
                if (!this.$disposing)
                {
                    this.$PC = 4;
                }
                goto Label_02F8;
            }
            this.<pauseTimeRemaining>__0--;
            this.$this.buttonOkText.get_rectTransform().localScale = new Vector3(1f, 1f, 1f);
        Label_0282:
            if (this.<pauseTimeRemaining>__0 > 0)
            {
                this.$this.buttonOkText.text = this.<pauseTimeRemaining>__0.ToString();
                this.$current = new WaitForSecondsRealtime(0.2f);
                if (!this.$disposing)
                {
                    this.$PC = 3;
                }
                goto Label_02F8;
            }
            this.$this.buttonOkText.set_color(this.$this.buttonOkText.get_color() + new Color(0f, 0f, 0f, 0.5f));
            this.$this.buttonOkText.text = LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Ok);
            this.$this.buttonOK.interactable = true;
            this.$PC = -1;
        Label_02F6:
            return false;
        Label_02F8:
            return true;
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

    [CompilerGenerated]
    private sealed class <WaitForScene>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Tutorial $this;
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
                    if (!GameManager.instance.operation.isDone)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    this.$this.StartTutorial();
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

