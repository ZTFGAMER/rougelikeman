using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCube : MonoBehaviour {

  public int m_Row;
  public int m_Column;
  public bool m_IsPlayer = false;
  public BattleManager battlemanager;
  // Use this for initialization
  void Start () {
		
	}

  // Update is called once per frame
  void Update () {
		
	}

  public void PushCard()
  {
    battlemanager.SelectBattleCube(this);
  }
}
