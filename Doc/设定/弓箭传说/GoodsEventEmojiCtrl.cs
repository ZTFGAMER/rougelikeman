using System;
using UnityEngine;

public class GoodsEventEmojiCtrl : MonoBehaviour
{
    private const string EventEmojiNear = "EventEmojiNear";
    private TextMesh text;
    private Animation ani;

    private void Awake()
    {
        this.text = base.transform.Find("child/Text").GetComponent<TextMesh>();
        this.ani = base.transform.Find("child").GetComponent<Animation>();
        MeshRenderer component = this.text.GetComponent<MeshRenderer>();
        component.sortingLayerName = "SkillEffect";
        component.sortingOrder = 0x3e7;
    }

    public void Far()
    {
        this.text.text = "?";
        this.ani["EventEmojiNear"].time = this.ani["EventEmojiNear"].clip.length;
        this.ani["EventEmojiNear"].speed = -1f;
        this.ani.Play("EventEmojiNear");
    }

    public void Near()
    {
        this.text.text = "!";
        this.ani["EventEmojiNear"].time = 0f;
        this.ani["EventEmojiNear"].speed = 1f;
        this.ani.Play("EventEmojiNear");
    }

    private void OnDisable()
    {
        if (this.ani != null)
        {
            this.ani.enabled = false;
        }
    }

    private void OnEnable()
    {
        this.text.text = "?";
        if (this.ani != null)
        {
            this.ani.enabled = true;
        }
    }
}

