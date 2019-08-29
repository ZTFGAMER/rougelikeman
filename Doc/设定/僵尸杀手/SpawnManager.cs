using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    [HideInInspector]
    public List<SurvivorSpawn> survivorSpawns;
    [SerializeField]
    private CRSB[] countRecumbentSurvivorsByLevel;
    [SerializeField]
    private float dropDelay = 20f;
    [SerializeField]
    public NewSurvivor[] survivors;
    [SerializeField]
    private int[] chances;
    [SerializeField]
    private GameObject prefabParashute;
    [SerializeField]
    private Transform cameraTarget;
    [SerializeField]
    private float parashuteStartHeight = 15f;
    private float minDistance = 20f;

    private void Awake()
    {
        instance = this;
    }

    private void DropCage()
    {
        this.OneMoreDrop();
        base.Invoke("DropCage", this.dropDelay);
    }

    public NewSurvivor GetNewSurvivor()
    {
        NewSurvivor survivor = null;
        do
        {
            survivor = this.survivors[this.GetNumRandSurv()];
        }
        while (!DataLoader.playerData.IsHeroOpened(survivor.heroType));
        return survivor;
    }

    private int GetNumRandSurv()
    {
        int max = 0;
        foreach (int num2 in this.chances)
        {
            max += num2;
        }
        int num4 = UnityEngine.Random.Range(0, max);
        int num5 = 0;
        for (int i = 0; i < this.chances.Length; i++)
        {
            num5 += this.chances[i];
            if (num4 < num5)
            {
                return i;
            }
        }
        return 0;
    }

    public SaveData.HeroData.HeroType GetRandomSurvivorType()
    {
        SaveData.HeroData.HeroType heroType;
        do
        {
            heroType = this.survivors[this.GetNumRandSurv()].heroType;
        }
        while (!DataLoader.playerData.IsHeroOpened(heroType));
        return heroType;
    }

    public void OneMoreDrop()
    {
        List<SurvivorSpawn> list = new List<SurvivorSpawn>();
        foreach (SurvivorSpawn spawn in this.survivorSpawns)
        {
            if (spawn.isReady() && (Vector3.Distance(spawn.transform.position, this.cameraTarget.position) > this.minDistance))
            {
                list.Add(spawn);
            }
        }
        if (list.Count <= 0)
        {
            Debug.LogError("Compatible places for spawn NewSurvivor Not Found!");
        }
        else
        {
            list[UnityEngine.Random.Range(0, list.Count)].Spawn();
            GameManager.instance.newSurvivorsLeft++;
        }
    }

    public void OneMoreParashute()
    {
        UnityEngine.Object.Instantiate<GameObject>(this.prefabParashute, new Vector3(this.cameraTarget.position.x, this.parashuteStartHeight, this.cameraTarget.position.z), new Quaternion());
    }

    public void Reset()
    {
        this.survivorSpawns.Clear();
    }

    public void StartGame()
    {
        if (this.dropDelay > 0f)
        {
            base.Invoke("DropCage", this.dropDelay);
        }
        for (int i = 0; i < this.countRecumbentSurvivorsByLevel[GameManager.instance.currentWorldNumber - 1].values[DataLoader.Instance.GetCurrentPlayerLevel()]; i++)
        {
            this.OneMoreDrop();
        }
    }

    public void StopIt()
    {
        base.CancelInvoke("DropCage");
    }

    public void TutorialDrop()
    {
        foreach (SurvivorSpawn spawn in this.survivorSpawns)
        {
            if (spawn.worldNumber < 1)
            {
                spawn.Spawn();
                break;
            }
        }
        GameManager.instance.newSurvivorsLeft++;
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    private struct CRSB
    {
        public int[] values;
    }
}

