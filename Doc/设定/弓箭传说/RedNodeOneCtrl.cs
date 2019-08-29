using System;
using UnityEngine;
using UnityEngine.UI;

public class RedNodeOneCtrl : MonoBehaviour
{
    public RectTransform child;
    public Text text_count;
    public Image image;
    public Image image_icon;
    public Animator ani;
    public int count;
    private RedNodeType mType;

    private void SetAniEnable(bool value)
    {
        this.ani.enabled = value;
        if (!value)
        {
            this.child.anchoredPosition = Vector2.zero;
        }
    }

    public void SetText(string value)
    {
        if (this.CanSetText && (this.text_count != null))
        {
            this.text_count.text = value;
        }
    }

    public void SetType(RedNodeType type)
    {
        if (this.mType != type)
        {
            this.mType = type;
            if (type == RedNodeType.eRedCount)
            {
                this.image.enabled = true;
                this.image_icon.enabled = false;
                this.text_count.enabled = true;
                this.SetAniEnable(true);
                this.image.set_sprite(SpriteManager.GetUICommon("UICommon_RedNode"));
            }
            else if (type == RedNodeType.eRedEmpty)
            {
                this.image.enabled = true;
                this.image_icon.enabled = false;
                this.text_count.enabled = false;
                this.SetAniEnable(true);
                this.image.set_sprite(SpriteManager.GetUICommon("UICommon_RedNode"));
            }
            else if (type == RedNodeType.eRedWear)
            {
                this.image.enabled = true;
                this.image_icon.enabled = true;
                this.text_count.enabled = false;
                this.SetAniEnable(true);
                this.image.set_sprite(SpriteManager.GetUICommon("UICommon_RedNode"));
                this.image_icon.set_sprite(SpriteManager.GetUICommon("UICommon_RedNode_Wear"));
            }
            else if (type == RedNodeType.eRedNew)
            {
                this.image.enabled = true;
                this.image_icon.enabled = false;
                this.text_count.enabled = true;
                this.SetAniEnable(true);
                this.image.set_sprite(SpriteManager.GetUICommon("UICommon_RedNode"));
                this.text_count.text = GameLogic.Hold.Language.GetLanguageByTID("red_new", Array.Empty<object>());
            }
            else if (type == RedNodeType.eRedUp)
            {
                this.image.enabled = true;
                this.image_icon.enabled = true;
                this.text_count.enabled = false;
                this.SetAniEnable(true);
                this.image.set_sprite(SpriteManager.GetUICommon("UICommon_RedNode"));
                this.image_icon.set_sprite(SpriteManager.GetUICommon("UICommon_RedNode_Up"));
            }
            else if (type != RedNodeType.eGreenCount)
            {
                if (type == RedNodeType.eGreenEmpty)
                {
                    this.image.enabled = true;
                    this.image_icon.enabled = false;
                    this.text_count.enabled = false;
                    this.SetAniEnable(false);
                    this.image.set_sprite(SpriteManager.GetUICommon("UICommon_GreenNode"));
                }
                else if (type == RedNodeType.eGreenUp)
                {
                    this.image.enabled = true;
                    this.image_icon.enabled = true;
                    this.text_count.enabled = false;
                    this.SetAniEnable(false);
                    this.image.set_sprite(SpriteManager.GetUICommon("UICommon_GreenNode"));
                    this.image_icon.set_sprite(SpriteManager.GetUICommon("UICommon_RedNode_Up"));
                }
                else if (type == RedNodeType.eWarning)
                {
                    this.image.enabled = true;
                    this.image_icon.enabled = true;
                    this.text_count.enabled = false;
                    this.SetAniEnable(true);
                    this.image.set_sprite(SpriteManager.GetUICommon("UICommon_RedNode"));
                    this.image_icon.set_sprite(SpriteManager.GetUICommon("UICommon_RedNode_Combine"));
                }
            }
            else
            {
                this.image.enabled = true;
                this.image_icon.enabled = false;
                this.text_count.enabled = true;
                this.SetAniEnable(false);
                this.image.set_sprite(SpriteManager.GetUICommon("UICommon_GreenNode"));
            }
        }
    }

    public int Value
    {
        set
        {
            this.count = value;
            if (value > 0)
            {
                this.SetText(value.ToString());
            }
            else
            {
                this.SetText(string.Empty);
            }
        }
    }

    private bool CanSetText =>
        ((this.mType == RedNodeType.eGreenCount) || (this.mType == RedNodeType.eRedCount));
}

