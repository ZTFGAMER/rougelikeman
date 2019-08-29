using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOver_NoNetCtrl : MonoBehaviour
{
    public GameObject child;
    public Image Image_NoNet;
    public Text Text_NoNet;

    private void Awake()
    {
        TweenSettingsExtensions.SetUpdate<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(ShortcutExtensions46.DOFade(this.Image_NoNet, 0f, 1f), -1, 1), true);
    }

    public void OnLanguageUpdate()
    {
        this.Text_NoNet.text = GameLogic.Hold.Language.GetLanguageByTID("GameOver_NoNet", Array.Empty<object>());
    }

    public void SetShow(bool value)
    {
        if (this.child != null)
        {
            this.child.SetActive(value);
        }
    }
}

