using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipCombineParent : MonoBehaviour
{
    public GameObject child;
    public RectTransform arrow;
    public List<GameObject> mCombineBG;
    public GameObject copychoose;
    public Action<EquipCombineChooseOne> OnCombineDown;
    private int mCount;
    private int width = 120;
    private LocalUnityObjctPool mPool;
    private List<EquipCombineChooseOne> mChooses = new List<EquipCombineChooseOne>();
    public List<int> mChooseIndexs = new List<int>();
    private List<float> mPos3;
    private List<float> mPos2;
    private int equipid;

    public EquipCombineParent()
    {
        List<float> list = new List<float> { 
            -150f,
            30f,
            150f
        };
        this.mPos3 = list;
        list = new List<float> { 
            -100f,
            80f
        };
        this.mPos2 = list;
    }

    private void Awake()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<EquipCombineChooseOne>(this.copychoose);
    }

    public bool can_choose(EquipCombineOne one) => 
        (this.equipid == one.mData.EquipID);

    public EquipCombineChooseOne ChooseOne(EquipCombineOne one)
    {
        int index = 0;
        int count = this.mChooseIndexs.Count;
        while (index < count)
        {
            if (this.mChooseIndexs[index] < 0)
            {
                this.modify_chooseindex(index, one.mIndex);
                this.mChooses[index].Set_Choose_Equip(one);
                return this.mChooses[index];
            }
            index++;
        }
        return null;
    }

    private void debug_indexs()
    {
    }

    public void down_one(int index)
    {
        int num = 0;
        int count = this.mChooseIndexs.Count;
        while (num < count)
        {
            if (this.mChooseIndexs[num] == index)
            {
                this.modify_chooseindex(num, -1);
                if (this.mChooses[index].mEquipChoose != null)
                {
                    this.mChooses[index].mEquipChoose.PlayAni(true);
                }
                this.mChooses[index].Clear();
            }
            num++;
        }
    }

    public int FindEmpty()
    {
        int num = 0;
        int count = this.mChooseIndexs.Count;
        while (num < count)
        {
            if (this.mChooseIndexs[num] < 0)
            {
                return num;
            }
            num++;
        }
        return -1;
    }

    public int get_choose_index(EquipCombineOne one)
    {
        int num = 0;
        int count = this.mChooseIndexs.Count;
        while (num < count)
        {
            if (this.mChooseIndexs[num] == one.mIndex)
            {
                return num;
            }
            num++;
        }
        return -1;
    }

    public EquipCombineChooseOne GetChoose(int index)
    {
        if ((index >= 0) && (index < this.mChooses.Count))
        {
            return this.mChooses[index];
        }
        return null;
    }

    public int GetIndex(int index)
    {
        this.debug_indexs();
        if ((index >= 0) && (index < this.mChooseIndexs.Count))
        {
            return this.mChooseIndexs[index];
        }
        return -1;
    }

    public Vector3 GetPosition(int index) => 
        this.mCombineBG[index].transform.position;

    public float GetScale(int index) => 
        this.mCombineBG[index].transform.localScale.x;

    public void Init(int count, EquipCombineOne equip)
    {
        this.mCount = count;
        this.mChooses.Clear();
        this.mPool.Collect<EquipCombineChooseOne>();
        List<float> list = this.mPos3;
        for (int i = 0; i < count; i++)
        {
            EquipCombineChooseOne item = this.mPool.DeQueue<EquipCombineChooseOne>();
            item.gameObject.SetParentNormal(this.mCombineBG[i]);
            (item.transform as RectTransform).SetAsFirstSibling();
            item.Clear();
            if (i == 0)
            {
                item.Init(i, equip.mData);
                item.Set_Choose_Equip(equip);
            }
            else
            {
                item.Init(i, equip.mData);
                item.ShowMask(true);
            }
            item.OnButtonClick = this.OnCombineDown;
            this.mChooses.Add(item);
        }
    }

    public void init_data(int count, EquipCombineOne equip)
    {
        this.equipid = equip.mData.EquipID;
        this.mCount = count;
        this.mChooseIndexs.Clear();
        this.mChooseIndexs.Add(equip.mIndex);
        for (int i = 0; i < (count - 1); i++)
        {
            this.mChooseIndexs.Add(-1);
        }
    }

    public bool Is_Full() => 
        (this.FindEmpty() < 0);

    private void modify_chooseindex(int index, int value)
    {
        this.mChooseIndexs[index] = value;
        this.debug_indexs();
    }

    public void Show(bool value)
    {
        if (!value)
        {
            this.equipid = 0;
            this.mPool.Collect<EquipCombineChooseOne>();
            int index = 0;
            int count = this.mChooseIndexs.Count;
            while (index < count)
            {
                this.modify_chooseindex(index, -1);
                index++;
            }
        }
    }
}

