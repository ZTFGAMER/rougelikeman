using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoneyCoinManager : MonoBehaviour
{
    public static MoneyCoinManager instance;
    private MoneyCoinSpawn[] places;
    [SerializeField]
    private CCBL[] countCoinsByLevel;
    [SerializeField]
    private GameObject prefabMoneyCoin;
    [SerializeField]
    public int coinMultyplier = 30;
    [HideInInspector]
    public double coinMoney;

    private void Awake()
    {
        instance = this;
    }

    public void EndGame()
    {
        foreach (MoneyCoin coin in UnityEngine.Object.FindObjectsOfType<MoneyCoin>())
        {
            UnityEngine.Object.Destroy(coin.gameObject);
        }
    }

    public void StartGame()
    {
        this.places = UnityEngine.Object.FindObjectsOfType<MoneyCoinSpawn>();
        List<MoneyCoinSpawn> list = new List<MoneyCoinSpawn>();
        foreach (MoneyCoinSpawn spawn in this.places)
        {
            if ((spawn.openAtLevel <= DataLoader.Instance.GetCurrentPlayerLevel()) && (spawn.worldNumber == GameManager.instance.currentWorldNumber))
            {
                list.Add(spawn);
            }
        }
        if (list.Count <= 0)
        {
            Debug.LogError("Compatible places for spawn MoneyCoins Not Found!");
        }
        else
        {
            for (int i = 0; (i < this.countCoinsByLevel[GameManager.instance.currentWorldNumber - 1].values[DataLoader.Instance.GetCurrentPlayerLevel()]) && (list.Count > 0); i++)
            {
                int index = UnityEngine.Random.Range(0, list.Count);
                Quaternion rotation = new Quaternion();
                UnityEngine.Object.Instantiate<GameObject>(this.prefabMoneyCoin, list[index].transform.position, rotation, TransformParentManager.Instance.moneyBox);
                list.RemoveAt(index);
            }
            this.UpdateCoinMoney();
        }
    }

    public void UpdateCoinMoney()
    {
        this.coinMoney = Mathf.Round((Mathf.Pow(1.35f, (float) (DataLoader.Instance.GetCurrentPlayerLevel() - 1)) * this.coinMultyplier) * DataLoader.Instance.dailyMultiplier);
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    private struct CCBL
    {
        public int[] values;
    }
}

