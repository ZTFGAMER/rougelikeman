using System;
using UnityEngine;

public class UIPause : MonoBehaviour
{
    public void GameOver()
    {
        GameManager.instance.GameOver();
    }
}

