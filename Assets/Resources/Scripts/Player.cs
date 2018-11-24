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
  public int m_CurrentHurt;
  public int m_DrawCardCount = 5;
  public bool m_IsAlive;
  public bool m_IsPlayer;
  public List<Card> m_BattleCardList;
  public AnimationManager m_AnimationManager;
  public Text m_TextName;
  public Text m_TextHP;
  public Text m_TextCost;
  private GameObject m_CurrentHPLine;
  private GameObject m_CurrentHurtLine;

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
    m_CurrentHPLine = m_TextHP.transform.parent.Find("Current_HP").gameObject;
    m_CurrentHurtLine = m_TextHP.transform.parent.Find("Current_Hurt").gameObject;
  }

  public void InitAnimation(string animname)
  {
    this.animationConfig = this.gameObject.GetComponent<UGUISpriteAnimation>();
    this.animationConfig.m_SpiteName = animname;
    this.animationConfig.InitFrame(animname);
  }
  public void EndTurn()
  {
    m_Cost = Constant.PLAYER_ENERGY;
  }
	// Update is called once per frame
	public void Tick () {
    DrawHPLine();
    UpdateData();
  }
  public void SetCurrentHurt(int hurt)
  {
    m_CurrentHurt = Mathf.Min(m_CurrentHurt + hurt,m_CurrentHP);
  }
  public void SetCurrentHP(int hp)
  {
    m_CurrentHP = Mathf.Min(m_CurrentHP + hp, m_HP);
  }  
  void DrawHPLine()
  {
    if (m_HP > 0)
    { 
      m_CurrentHurtLine.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Min((float)m_CurrentHurt,(float)m_CurrentHP) / (float)m_HP * 200f, 40f);
      m_CurrentHPLine.GetComponent<RectTransform>().sizeDelta = new Vector2((float)m_CurrentHP / (float)m_HP * 200f, 40f);
      m_CurrentHPLine.transform.position = new Vector3(m_CurrentHPLine.transform.parent.position.x - (1f - (float)m_CurrentHP / (float)m_HP) * 100f, m_CurrentHPLine.transform.parent.position.y);
    }
    if (m_CurrentHP > 0)
      m_CurrentHurtLine.transform.position = new Vector3(m_CurrentHPLine.transform.position.x + (1f - (float)m_CurrentHurt / (float)m_CurrentHP) * ((float)m_CurrentHP / (float)m_HP) * 100f, m_CurrentHPLine.transform.position.y);
  }

  void UpdateData()
  {
    m_TextName.text = m_PlayerName;
    m_TextHP.text = m_CurrentHP.ToString() + "/" + m_HP.ToString();
    if (m_IsPlayer)
      m_TextCost.text = m_CurrentCost.ToString()+"/" +m_Cost.ToString();
  }
}
