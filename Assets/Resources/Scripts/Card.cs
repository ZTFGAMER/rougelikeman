using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

  public enum CardType
  {
    Character,
    Magic,
    Trap,
  };
  public enum CardSpace
  {
    PlayerDeck,
    PlayerHand,
    PlayerDrop,
    PlayerGround,
    EnemyGround,
    EnemyPre,
  };
  public enum HurtEffect
  {
    Normal = 1,//常规
    Backstab = 2,//从最后的敌人开始计算伤害
    Puncture = 4,//无视护盾造成伤害
    Penetrate = 8,//贯穿，对所有排上的敌人造成伤害
  };

  public string m_CardName;
  public int m_HP = 0;
  public int m_Cost;
  public int m_ATK = 0;
  public int m_Armor;
  public CardType m_CardType;
  public CardSpace m_CardSpace;
  public HurtEffect m_HurtEffect;

  public bool m_IsSelected;
  public bool m_IsInBattleGround;
  public int m_BattleRow;
  public int m_BattleColumn;
  public bool m_IsBattleDead;
  public float m_DeadAnimTime = 0f;

  public int m_CurrentHP;
  public int m_CurrentCost;
  public int m_CurrentATK;
  public int m_CurrentArmor;
  public int m_CurrentOrder;
  public int m_CurrentHurt = 0;
  public int m_CurrentHurtA = 0;
  public Text m_TextHP;
  public Text m_TextATK;
  public Text m_TextCost;
  public Text m_TextCardName;
  public GameObject m_HPLine;
  public GameObject m_ATKLine;
  public GameObject m_ObjectCardSelect;
  public bool m_IsEnemy;

  public UGUISpriteAnimation animationConfig;

  public BattleManager battleManager;

  // Use this for initialization
  void Start() {

  }
  public void InitByClone(Card clonecard)
  {
    this.battleManager = clonecard.battleManager;
    m_IsEnemy = clonecard.m_IsEnemy;
    ChangeHPAndATKLine();
    m_CardName = clonecard.m_CardName;
    m_HurtEffect = clonecard.m_HurtEffect;
    ChangeHP(clonecard.m_HP);
    ChangeATK(clonecard.m_ATK);
    m_Cost = clonecard.m_Cost;
    m_CurrentHurt = 0;
    m_CurrentHurtA = 0;
    m_CardType = clonecard.m_CardType;
    InitAnimation(clonecard.animationConfig.m_SpiteName);
    PrepareForBattle();
  }
  public void ChangeHPAndATKLine()
  {
    if (m_IsEnemy)
    {
      GameObject tochange = m_HPLine;
      m_HPLine = m_ATKLine;
      m_ATKLine = tochange;
    }
  }
  public void ChangeHP(int delta)
  {
    m_HP = Mathf.Max(0,m_HP + delta);
    if (delta != 0)
    {
      ChangeCardLine(m_HPLine, m_HP, delta,"defence_icon_01");
    }
  }
  public void ChangeATK(int delta)
  {
    m_ATK = Mathf.Max(0, m_ATK + delta);
    if (delta != 0)
    {
      switch (m_HurtEffect)
      {
        case HurtEffect.Normal:
          ChangeCardLine(m_ATKLine, m_ATK, delta, "attack_icon_01");
          break;
        case HurtEffect.Backstab:
          ChangeCardLine(m_ATKLine, m_ATK, delta, "attack_icon_02");
          break;
        case HurtEffect.Penetrate:
          ChangeCardLine(m_ATKLine, m_ATK, delta, "attack_icon_03");
          break;
        default:
          ChangeCardLine(m_ATKLine, m_ATK, delta, "attack_icon_01");
          break;
      }
    }
  }
  void ChangeCardLine(GameObject line, int total, int delta,string image)
  {
    if (delta == 0)
    {
      return;
    }
    else if (delta > 0)
    {
      for (int i = 0; i < delta; i++)
      {
        GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HPLine");
        GameObject hpline = Instantiate(toInstantiate, line.transform);
        hpline.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + image);
      }
    }
    else
    {
      for (int i = 0; i > delta; i--)
      {
        Destroy(line.transform.GetChild(-i).gameObject);
      }
    }
    UpdateCardLinePosition(line);
  }
  void UpdateCardLinePosition(GameObject line)
  {
    int enemy = 1;
    if (m_IsEnemy)
    {
      enemy = -1;
    }
    for (int i = 0; i < line.transform.childCount; i++)
    {
      if (line.transform.childCount < 4)
      {
        line.transform.GetChild(i).position = new Vector3(line.transform.position.x, line.transform.position.y - 55 * enemy + enemy * (line.transform.childCount - 1 - i) * 40f);
      }
      else
      {
        line.transform.GetChild(i).position = new Vector3(line.transform.position.x, line.transform.position.y + 55 * enemy - enemy * i * 110f / (line.transform.childCount - 1));
      }
    }
  }
  public void InitAnimation(string animname)
  {
    this.animationConfig = this.transform.Find("HandCardAnim").GetComponent<UGUISpriteAnimation>();
    this.animationConfig.m_SpiteName = animname;
    this.animationConfig.InitFrame(animname);
  }
  public void PrepareForBattle()
  {
    m_CurrentHP = m_HP;
    m_CurrentCost = m_Cost;
    m_CurrentATK = m_ATK;
    m_IsSelected = false;
    m_IsInBattleGround = false;
    m_IsBattleDead = false;
  }
  public void ReadyToBattle()
  {
    if (!m_IsInBattleGround)
      battleManager.SelectHandCard(this);
  }
  public void SetPlace(int row, int column)
  {
    m_BattleColumn = column;
    m_BattleRow = row;
  }
  public bool GetPlace(int row, int column)
  {
    return row == m_BattleRow && column == m_BattleColumn;
  }
  public int GetHurtPre(int hurt)
  {
    if (hurt >= m_CurrentHP - m_CurrentHurt)
    {
      hurt -= m_CurrentHP - m_CurrentHurt;
      m_CurrentHurt = m_CurrentHP;
      return hurt;
    }
    else
    {
      m_CurrentHurt += hurt;
      return 0;
    }
  }
  public void GetHurtAPra(int hurt)
  {
    if (hurt <= m_CurrentATK)
    {
      m_CurrentHurtA = m_CurrentATK - hurt;
    }
    else
    {
      m_CurrentHurtA = 0;
    }
  }
  public int GetHurt(int hurt)
  {
    if (hurt >= m_CurrentHP)
    {
      m_IsBattleDead = true;
      hurt -= m_CurrentHP;
      m_CurrentHP = 0;
      return hurt;
    }
    else
    {
      m_CurrentHP -= hurt;
      return 0;
    }
  }
  public int GetHurtByPuncture(int hurt)
  {
    if (hurt >= m_CurrentHP)
    {
      m_IsBattleDead = true;
      hurt -= m_CurrentHP;
      m_CurrentHP = 0;
      return hurt;
    }
    else
    {
      m_CurrentHP -= hurt;
      return 0;
    }
  }
  public void Tick() {
    ShowCard();
    if (m_IsInBattleGround)
    { 
      UpdateCardLine(m_HPLine, m_HP, m_CurrentHP,m_CurrentHurt,Color.green, Color.gray,Color.yellow);
      UpdateCardLine(m_ATKLine, m_ATK, m_CurrentATK, m_CurrentHurtA, Color.red,Color.gray, Color.yellow);
    }
  }
  void UpdateCardLine(GameObject line, int total, int delta,int hurt,Color totalc,Color deltac,Color hurtc)
  {
    if (total > 0)
    {
      for (int i = 0; i < total; i++)
      {
        if (delta < total && i < total - delta)
        {
          line.transform.GetChild(i).GetComponent<Image>().color = deltac;
        }
        else if (hurt > 0 && i < total - delta + hurt)
        {
          line.transform.GetChild(i).GetComponent<Image>().color = hurtc;
        }
        else
        {
          line.transform.GetChild(i).GetComponent<Image>().color = totalc;
        }
      }
    }
  }
  void ShowCard()
  {
    m_TextCost.text = m_CurrentCost.ToString();
    m_TextCardName.text = m_CardName;
    if (this.gameObject.transform.Find("HandCardAnim/HandCard_Cost"))
    {
      this.gameObject.transform.Find("HandCardAnim/HandCard_Cost").gameObject.SetActive(!m_IsInBattleGround);
    }
    if (this.gameObject.transform.Find("HandCardAnim/HandCard_Name"))
    {
      this.gameObject.transform.Find("HandCardAnim/HandCard_Name").gameObject.SetActive(!m_IsInBattleGround);
    }
    if (this.gameObject.transform.Find("HandCardAnim/HandCard_ATK"))
    {
      this.gameObject.transform.Find("HandCardAnim/HandCard_ATK").gameObject.SetActive(!m_IsInBattleGround);
    }
    if (this.gameObject.transform.Find("HandCardAnim/HandCard_HP"))
    {
      this.gameObject.transform.Find("HandCardAnim/HandCard_HP").gameObject.SetActive(!m_IsInBattleGround);
    }
    if (m_HPLine != null)
    {
      m_HPLine.SetActive(m_IsInBattleGround);
    }
    if (m_ATKLine != null)
    {
      m_ATKLine.SetActive(m_IsInBattleGround);
    }
    if (m_CardType == CardType.Character)
    {
      m_TextATK.text = m_ATK.ToString();
      m_TextHP.text = m_HP.ToString();
    }
    else
    {
      m_TextATK.transform.parent.gameObject.SetActive(false);
      m_TextHP.transform.parent.gameObject.SetActive(false);
    }
    if (m_ObjectCardSelect != null)
    {
      m_ObjectCardSelect.SetActive(m_IsSelected);
    }
  }
}
