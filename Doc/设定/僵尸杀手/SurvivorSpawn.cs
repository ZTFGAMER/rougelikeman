using System;
using UnityEngine;

public class SurvivorSpawn : PointOnMap
{
    [HideInInspector]
    public NewSurvivor newSurvivor;

    public bool isReady() => 
        (((this.newSurvivor == null) && (base.openAtLevel <= DataLoader.Instance.GetCurrentPlayerLevel())) && (base.worldNumber == GameManager.instance.currentWorldNumber));

    public void Spawn()
    {
        this.newSurvivor = UnityEngine.Object.Instantiate<NewSurvivor>(SpawnManager.instance.GetNewSurvivor(), base.transform.position, new Quaternion());
    }

    private void Start()
    {
        SpawnManager.instance.survivorSpawns.Add(this);
    }
}

