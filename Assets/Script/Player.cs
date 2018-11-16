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

  public string m_PlayerName;
  public int m_HP;
  public int m_Cost;
  public int m_CurrentHP;
  public int m_CurrentCost;
  public bool m_IsAlive;
  public bool m_IsPlayer;
  public List<Card> m_BattleCardList;
  public AnimationManager m_AnimationManager;
  public Text m_NameText;
  public Text m_HPText;
  public Text m_CostText;

  // Use this for initialization
  void Start () {
  }

  public void PrepareForBattle() {
    m_CurrentHP = m_HP;
    m_CurrentCost = m_Cost;
    m_IsAlive = true;
    m_NameText.text = m_PlayerName;
    m_HPText.text = m_HP.ToString();
    if (m_IsPlayer)
      m_CostText.text = m_Cost.ToString();

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
    
    UpdateData();
  }

  void UpdateData()
  {
    m_NameText.text = m_PlayerName;
    m_HPText.text = m_CurrentHP.ToString();
    if (m_IsPlayer)
      m_CostText.text = m_CurrentCost.ToString()+"/" +m_Cost.ToString();
  }
}
