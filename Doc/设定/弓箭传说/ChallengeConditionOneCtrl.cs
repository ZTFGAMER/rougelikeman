using System;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeConditionOneCtrl : MonoBehaviour
{
    public Text Text_Condition;

    public void Init(string str)
    {
        this.Text_Condition.text = str;
    }
}

