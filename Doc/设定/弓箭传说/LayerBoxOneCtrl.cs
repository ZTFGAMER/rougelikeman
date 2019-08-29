using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class LayerBoxOneCtrl : MonoBehaviour
{
    public Image Image_BG;
    public Text Text_Stage;
    public Text Text_Count;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Box_ChapterBox <m_Data>k__BackingField;

    public void Init(Box_ChapterBox data)
    {
        this.m_Data = data;
        LocalSave.Instance.mStage.GetLayerBoxStageLayer(this.m_Data.Chapter, out int num, out int num2);
        object[] args = new object[] { num };
        this.Text_Stage.text = GameLogic.Hold.Language.GetLanguageByTID("ChapterIndex_x", args);
        this.Text_Count.text = num2.ToString();
    }

    public Box_ChapterBox m_Data { get; private set; }
}

