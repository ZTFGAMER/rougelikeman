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
  public int m_DrawCardCount = 5;
  public bool m_IsAlive;
  public bool m_IsPlayer;
  public List<Card> m_BattleCardList;
  public AnimationManager m_AnimationManager;
  public Text m_TextName;
  public Text m_TextHP;
  public Text m_TextCost;

  public UGUISpriteAnimation animationConfig;

  // Use this for initialization
  void Start () {
  }

  public void PrepareForBattle() {
    m_CurrentHP = m_HP;
    m_CurrentCost = m_Cost;
    m_IsAlive = true;
    m_TextName.text = m_PlayerName;
    m_TextHP.text = m_HP.ToString();
    if (m_IsPlayer)
      m_TextCost.text = m_Cost.ToString();

  }

  public void InitAnimation(string animname)
  {
    this.animationConfig = this.gameObject.GetComponent<UGUISpriteAnimation>();
    this.animationConfig.m_SpiteName = animname;
    this.animationConfig.InitFrame(animname);
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
    m_TextName.text = m_PlayerName;
    m_TextHP.text = m_CurrentHP.ToString();
    if (m_IsPlayer)
      m_TextCost.text = m_CurrentCost.ToString()+"/" +m_Cost.ToString();
  }
}
