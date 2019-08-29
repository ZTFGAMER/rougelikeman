using System;
using UnityEngine;

[Serializable]
public class ZombiePrefabData
{
    public SaveData.ZombieData.ZombieType type;
    public GameObject[] zombiePrefabs;
    public int health;
    public int damage;
    [HideInInspector]
    public float power;

    public void SetPrefabData()
    {
        for (int i = 0; i < this.zombiePrefabs.Length; i++)
        {
            ZombieHuman component = this.zombiePrefabs[i].GetComponent<ZombieHuman>();
            component.zombieType = this.type;
            component.countHealth = this.health;
            component.damage = this.damage;
            component.power = this.power;
        }
    }
}

