using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static ArenaManager <instance>k__BackingField;
    [SerializeField]
    private TextAsset csvData;
    [SerializeField]
    private int arenasCount = 1;
    [SerializeField]
    private int arenaStages = 1;
    private List<int[,]> arenaInfos;
    private int currentArena;
    private int currentStage;

    private void Awake()
    {
        instance = this;
    }

    private int GetCurrentArena()
    {
        for (int i = 0; i < this.arenasCount; i++)
        {
            if (DataLoader.playerData.arenaRating < this.arenaInfos[i][this.arenaStages - 1, 0])
            {
                return i;
            }
        }
        return (this.arenaInfos.Count - 1);
    }

    public int GetCurrentArenaMaxScore() => 
        this.arenaInfos[this.currentArena][this.arenaStages - 1, 1];

    private int GetCurrentArenaStage()
    {
        for (int i = 0; i < this.arenaStages; i++)
        {
            if (DataLoader.playerData.arenaRating < this.arenaInfos[this.currentArena][i, 0])
            {
                return i;
            }
        }
        return this.arenaInfos[this.currentArena][this.arenaStages - 1, 0];
    }

    public int GetCurrentStageScore() => 
        this.arenaInfos[this.currentArena][this.currentStage, 1];

    private void InitializeArenaInfos()
    {
        int[,] array = new int[this.arenasCount * this.arenaStages, 2];
        CsvLoader.SplitText<int>(this.csvData, ',', ref array);
        int[,] item = new int[this.arenaStages, 2];
        item[0, 0] = array[0, 0];
        item[0, 1] = array[0, 1];
        this.arenaInfos = new List<int[,]>();
        for (int i = 1; i < array.GetLength(0); i++)
        {
            if ((i % 4) == 0)
            {
                this.arenaInfos.Add(item);
                item = new int[this.arenaStages, 2];
                item[0, 0] = array[i, 0];
                item[0, 1] = array[i, 1];
            }
            else
            {
                int num2 = i % 4;
                item[num2, 0] = array[i, 0];
                item[num2, 1] = array[i, 1];
            }
        }
        this.arenaInfos.Add(item);
    }

    private void Start()
    {
        this.InitializeArenaInfos();
        this.UpdateAll();
    }

    public void UpdateAll()
    {
        this.currentArena = this.GetCurrentArena();
        this.currentStage = this.GetCurrentArenaStage();
    }

    public static ArenaManager instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }
}

