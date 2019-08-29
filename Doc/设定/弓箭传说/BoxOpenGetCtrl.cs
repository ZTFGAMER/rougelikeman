using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenGetCtrl : MonoBehaviour
{
    public const float ItemWidth = 150f;
    public const float ItemHeight = 150f;
    public const int LineCount = 4;
    public GameObject child;
    public Text Text_Title;
    public Transform getparent;
    private GameObject _copyitem;
    private LocalUnityObjctPool mPool;

    private void Awake()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<BoxOpenGetOne>(this.copyitem);
    }

    public Sequence Init(List<Drop_DropModel.DropData> list)
    {
        <Init>c__AnonStorey0 storey = new <Init>c__AnonStorey0 {
            list = list,
            $this = this
        };
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("BoxOpen_GetTitle", Array.Empty<object>());
        this.Text_Title.transform.localScale = Vector3.one * 1.6f;
        Sequence sequence = DOTween.Sequence();
        TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.Text_Title.transform, 1f, 0.3f));
        this.mPool.Collect<BoxOpenGetOne>();
        storey.count = storey.list.Count;
        for (int i = 0; i < storey.count; i++)
        {
            <Init>c__AnonStorey1 storey2 = new <Init>c__AnonStorey1 {
                <>f__ref$0 = storey,
                index = i
            };
            TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey2, this.<>m__0));
            TweenSettingsExtensions.AppendInterval(sequence, 0.2f);
        }
        return sequence;
    }

    public void Show(bool value)
    {
        this.child.SetActive(value);
    }

    private GameObject copyitem
    {
        get
        {
            if (this._copyitem == null)
            {
                this._copyitem = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/BoxGetUI/BoxGetResult"));
                this._copyitem.SetParentNormal(this.getparent);
                this._copyitem.SetActive(false);
            }
            return this._copyitem;
        }
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey0
    {
        internal int count;
        internal List<Drop_DropModel.DropData> list;
        internal BoxOpenGetCtrl $this;
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey1
    {
        internal int index;
        internal BoxOpenGetCtrl.<Init>c__AnonStorey0 <>f__ref$0;

        internal void <>m__0()
        {
            BoxOpenGetOne one = this.<>f__ref$0.$this.mPool.DeQueue<BoxOpenGetOne>();
            RectTransform child = one.transform as RectTransform;
            child.SetParentNormal(this.<>f__ref$0.$this.getparent);
            if (this.<>f__ref$0.count <= 4)
            {
                float x = (this.index * 150f) - (75f * (this.<>f__ref$0.count - 1));
                child.anchoredPosition = new Vector2(x, 0f);
            }
            else
            {
                child.anchoredPosition = new Vector2(((this.index % 4) * 150f) - 225f, (this.index / 4) * -150f);
            }
            one.Init(this.<>f__ref$0.list[this.index]);
            one.mCanvasGroup.alpha = 0f;
            child.localScale = Vector3.zero;
            TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(DOTween.Sequence(), ShortcutExtensions46.DOFade(one.mCanvasGroup, 1f, 0.3f)), ShortcutExtensions.DOScale(child, 1f, 0.3f));
        }
    }
}

