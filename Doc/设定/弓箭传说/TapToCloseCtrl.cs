using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TapToCloseCtrl : MonoBehaviour
{
    public GameObject child;
    public ButtonCtrl Button_Close;
    public Text Text_Content;
    public Action OnClose;

    private void Awake()
    {
        if (this.Button_Close != null)
        {
            this.Button_Close.onClick = delegate {
                if (this.OnClose != null)
                {
                    this.OnClose();
                }
            };
        }
        this.Play();
    }

    private void Play()
    {
        ShortcutExtensions.DOKill(this.Text_Content, false);
        this.Text_Content.set_color(new Color(this.Text_Content.get_color().r, this.Text_Content.get_color().g, this.Text_Content.get_color().b, 0f));
        TweenSettingsExtensions.SetUpdate<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(ShortcutExtensions46.DOFade(this.Text_Content, 1f, 1f), -1, 1), 7), true);
        this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("TapToClose", Array.Empty<object>());
    }

    public void Show(bool value)
    {
        this.child.SetActive(value);
        if (value)
        {
            this.Play();
        }
    }
}

