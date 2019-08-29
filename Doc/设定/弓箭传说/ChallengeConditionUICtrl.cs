using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeConditionUICtrl : MonoBehaviour
{
    public GameObject child;
    public GameObject copyitem;
    public Text Text_ConditionContent;
    private LocalUnityObjctPool mPool;
    private List<Text> mList = new List<Text>();

    private void Awake()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<ChallengeConditionOneCtrl>(this.copyitem);
    }

    public void Init()
    {
        this.Text_ConditionContent.text = GameLogic.Hold.Language.GetLanguageByTID("Challenge_Condition", Array.Empty<object>());
        this.mPool.Collect<ChallengeConditionOneCtrl>();
        List<string> list = GameLogic.Hold.BattleData.Challenge_GetConditions();
        bool flag = list.Count > 0;
        this.child.SetActive(flag);
        if (flag)
        {
            int num = 0;
            int count = list.Count;
            while (num < count)
            {
                ChallengeConditionOneCtrl ctrl = this.mPool.DeQueue<ChallengeConditionOneCtrl>();
                ctrl.transform.SetParentNormal(this.child);
                ctrl.transform.localPosition = new Vector3(0f, (float) (num * -40), 0f);
                ctrl.Init(list[num]);
                num++;
            }
        }
    }
}

