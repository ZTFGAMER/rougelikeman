using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CharSortCtrl : MonoBehaviour
{
    public ButtonCtrl Button_Sort;
    public Text Text_Sort;
    private const int sorttypecount = 2;
    private static string[] sortstrings = new string[] { "EquipUI_Sort_Quality", "EquipUI_Sort_Position" };
    public Action<List<LocalSave.EquipOne>> OnButtonClick;
    private Func<List<LocalSave.EquipOne>>[] sorts = new Func<List<LocalSave.EquipOne>>[2];
    private EquipType mEquipType = EquipType.eAll;
    [CompilerGenerated]
    private static Comparison<LocalSave.EquipOne> <>f__am$cache0;
    [CompilerGenerated]
    private static Comparison<LocalSave.EquipOne> <>f__am$cache1;

    private void Awake()
    {
        this.sorts[0] = delegate {
            List<LocalSave.EquipOne> props = LocalSave.Instance.GetProps(this.mEquipType, false);
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (LocalSave.EquipOne a, LocalSave.EquipOne b) {
                    if (a.PropType < b.PropType)
                    {
                        return -1;
                    }
                    if (a.PropType <= b.PropType)
                    {
                        if (a.data.Quality > b.data.Quality)
                        {
                            return -1;
                        }
                        if (a.data.Quality < b.data.Quality)
                        {
                            return 1;
                        }
                        if (a.data.Id < b.data.Id)
                        {
                            return -1;
                        }
                    }
                    return 1;
                };
            }
            props.Sort(<>f__am$cache0);
            return props;
        };
        this.sorts[1] = delegate {
            List<LocalSave.EquipOne> props = LocalSave.Instance.GetProps(this.mEquipType, false);
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate (LocalSave.EquipOne a, LocalSave.EquipOne b) {
                    if (a.PropType < b.PropType)
                    {
                        return -1;
                    }
                    if (a.PropType <= b.PropType)
                    {
                        if (a.data.Position < b.data.Position)
                        {
                            return -1;
                        }
                        if (a.data.Position > b.data.Position)
                        {
                            return 1;
                        }
                        if (a.data.Quality > b.data.Quality)
                        {
                            return -1;
                        }
                        if (a.data.Quality < b.data.Quality)
                        {
                            return 1;
                        }
                        if (a.data.Id < b.data.Id)
                        {
                            return -1;
                        }
                    }
                    return 1;
                };
            }
            props.Sort(<>f__am$cache1);
            return props;
        };
        this.Button_Sort.onClick = delegate {
            if (this.OnButtonClick != null)
            {
                this.mSortType++;
                this.mSortType = this.mSortType % 2;
                this.OnLanguageChange();
                this.OnButtonClick(this.sorts[this.mSortType]());
            }
        };
    }

    public List<LocalSave.EquipOne> GetList(EquipType type)
    {
        this.mEquipType = type;
        return this.sorts[this.mSortType]();
    }

    public void OnLanguageChange()
    {
        this.Text_Sort.text = GameLogic.Hold.Language.GetLanguageByTID(sortstrings[this.mSortType], Array.Empty<object>());
    }

    private int mSortType
    {
        get => 
            PlayerPrefsEncrypt.GetInt("charui_sort_local", 0);
        set => 
            PlayerPrefsEncrypt.SetInt("charui_sort_local", value);
    }
}

