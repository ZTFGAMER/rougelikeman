using System;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSkillColumnCtrl : MonoBehaviour
{
    private Text text;
    private int index;
    public const int HEIGHT = 180;
    private bool bStart;
    private bool bEnd;
    private ChooseSkillOneCtrl[] list = new ChooseSkillOneCtrl[3];

    private void Awake()
    {
        this.index = int.Parse(base.transform.parent.parent.parent.name);
        for (int i = 0; i < 3; i++)
        {
            this.list[i] = base.transform.Find(i.ToString()).GetComponent<ChooseSkillOneCtrl>();
        }
    }

    public void Init(int count, Text text)
    {
        this.text = text;
        for (int i = 0; i < 3; i++)
        {
            this.list[i].AddAction(this.index, count, new Action(this.RandomName));
        }
    }

    private void RandomName()
    {
        int randomSkill = GameLogic.Self.GetRandomSkill();
        this.text.text = GameLogic.Hold.Language.GetSkillName(randomSkill);
    }
}

