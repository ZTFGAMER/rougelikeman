using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

  public enum PlayerType
  {
    Player ,
    Enemy 
  };

  public int m_HP;
  public int m_Cost;
  public bool m_IsAlive;
  public Text m_CostText;

	// Use this for initialization
	void Start () {
    m_IsAlive = true;
  }

  public void InitType(PlayerType type) {
    switch (type)
    {
      case PlayerType.Player:
        m_HP = Constant.PLAYER_HP;
        m_Cost = Constant.PLAYER_ENERGY;
        break;
      case PlayerType.Enemy:
        m_HP = Constant.ENEMY_HP;
        m_Cost = 0;
        break;
      default:
        break;
    }
  }

  public void GetHurt(int damage) {
    if (m_HP <= damage)
    {
      m_IsAlive = false;
      m_HP = 0;
    }
    else
    {
      m_HP -= damage;
    }
  }

  public void EndTurn()
  {
    m_Cost = Constant.PLAYER_ENERGY;
  }

	// Update is called once per frame
	void Update () {

    m_CostText.text = m_Cost.ToString();
  }
}
