using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverChallengeCtrl : MonoBehaviour
{
    public GameObject child;
    public Text Text_Content;

    public void SetContent(string value)
    {
        this.Text_Content.text = value;
    }

    public void Show(bool value)
    {
        this.child.SetActive(value);
    }
}

