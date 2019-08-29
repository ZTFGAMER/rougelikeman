using Dxx.Util;
using System;
using UnityEngine;

public class ChooseSkillButtonCtrl : MonoBehaviour
{
    private ChooseSkillOneCtrl[] list = new ChooseSkillOneCtrl[3];

    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            object[] args = new object[] { "fg/child/columnctrl/", i };
            this.list[i] = base.transform.Find(Utils.GetString(args)).GetComponent<ChooseSkillOneCtrl>();
        }
    }

    public void OnClick()
    {
        for (int i = 0; i < 3; i++)
        {
            this.list[i].OnClick();
        }
    }

    private void Update()
    {
    }
}

