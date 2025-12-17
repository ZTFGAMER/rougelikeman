using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardArea : MonoBehaviour {

  public List<Card> m_AreaList;
  public string m_CardAreaName;
  public Text m_TextCount;
  public BattleManager battlemanager;

	// Use this for initialization
	void Start () {
	}
  public void InitCard(bool isenemy,string name, string animname, int cost, int hp = 0, int atk = 0, Card.CardType cardtype = Card.CardType.Character, Card.HurtEffect hurteffect = Card.HurtEffect.Normal)
  {
    GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HandCard");
    Card card = Instantiate(toInstantiate, battlemanager.transform.Find("Recycle")).GetComponent<Card>();
    card.battleManager = battlemanager;
    card.m_IsEnemy = isenemy;
    card.ChangeHPAndATKLine();
    card.m_CardName = name;
    card.m_HurtEffect = hurteffect;
    card.ChangeHP(hp);
    card.ChangeATK(atk);
    card.m_Cost = cost;
    card.m_CardType = cardtype;
    card.InitAnimation(animname);
    card.PrepareForBattle();
    this.m_AreaList.Add(card);

  }

  /// <summary>
  /// 使用CardData初始化卡牌
  /// Initialize card using CardData
  /// </summary>
  public void InitCardFromData(CardData cardData)
  {
    GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HandCard");
    Card card = Instantiate(toInstantiate, battlemanager.transform.Find("Recycle")).GetComponent<Card>();
    card.battleManager = battlemanager;
    card.m_IsEnemy = cardData.isEnemy;
    card.ChangeHPAndATKLine();
    card.m_CardName = cardData.cardName;
    card.m_HurtEffect = cardData.hurtEffect;
    card.ChangeHP(cardData.hp);
    card.ChangeATK(cardData.attack);
    card.m_Cost = cardData.cost;
    card.m_CardType = cardData.cardType;
    card.InitAnimation(cardData.animationName);
    card.PrepareForBattle();
    this.m_AreaList.Add(card);
  }
  // Update is called once per frame
  void Update () {
		
	}
}
