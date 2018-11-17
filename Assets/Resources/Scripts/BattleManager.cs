using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

  public List<CardArea> CardAreaList;
  public Player player;
  public Player enemy;

  // Use this for initialization
  void Start() {

    InitBattleData();

    InitBattleGround();
  }
  /// <summary>
  /// 临时数据初始化，后期会通过读表获取
  /// </summary>
  void InitBattleData()
  {
    InitCardAreaList("HandCardArea");
    InitCardAreaList("PlayerBattleArea");
    InitCardAreaList("EnemyBattleArea");
    InitCardAreaList("PlayerDeckArea");
    InitCardAreaList("EnemyDeckArea");
    InitCardAreaList("PlayerDropArea");
    InitCardAreaList("EnemyDropArea");

    InitPlayer(player, "圣骑士", 35, 3, true);
    InitPlayer(enemy, "野蛮人", 30, 3, false);

    InitCard(CardAreaList[3], "士兵", 1, 3, 3);
    InitCard(CardAreaList[3], "士兵", 1, 3, 3);
    InitCard(CardAreaList[3], "士兵", 1, 3, 3);
    InitCard(CardAreaList[3], "弓箭手", 1, 1, 5);
    InitCard(CardAreaList[3], "弓箭手", 1, 1, 5);
    InitCard(CardAreaList[3], "弓箭手", 1, 1, 5);
    InitCard(CardAreaList[3], "盾手", 1, 5, 1);
    InitCard(CardAreaList[3], "盾手", 1, 5, 1);
    InitCard(CardAreaList[3], "盾手", 1, 5, 1);
    InitCard(CardAreaList[3], "冲击手", 2, 3, 3, Card.CardType.Character, Card.HurtEffect.Penetrate);
    InitCard(CardAreaList[3], "指挥官", 2, 4, 2);
    InitCard(CardAreaList[3], "冲锋", 1, 0, 0, Card.CardType.Magic);
    InitCard(CardAreaList[3], "爆发", 0, 0, 0, Card.CardType.Magic);
    InitCard(CardAreaList[3], "坚守", 1, 0, 0, Card.CardType.Magic);
    InitCard(CardAreaList[3], "巨盾", 2, 0, 0, Card.CardType.Magic);

    InitCard(CardAreaList[4], "蛮族勇士", 1, 2, 2);
    InitCard(CardAreaList[4], "蛮族勇士", 1, 2, 2);
    InitCard(CardAreaList[4], "蛮族勇士", 1, 2, 2);
    InitCard(CardAreaList[4], "蛮族巫师", 1, 1, 5, Card.CardType.Character, Card.HurtEffect.Backstab);
    InitCard(CardAreaList[4], "奉献", 1, 0, 0, Card.CardType.Magic);
    InitCard(CardAreaList[4], "牺牲", 1, 0, 0, Card.CardType.Magic);
    InitCard(CardAreaList[4], "牺牲", 1, 0, 0, Card.CardType.Magic);
    InitCard(CardAreaList[4], "狂暴", 3, 0, 0, Card.CardType.Magic);
  }

  /// <summary>
  /// 初始化几个不同的放卡位置
  /// </summary>
  /// <param name="name"></param>
  void InitCardAreaList(string name)
  {
    if (this.transform.Find(name))
    {
      if (!this.transform.Find(name).GetComponent<CardArea>())
      {
        this.transform.Find(name).gameObject.AddComponent<CardArea>();
      }
      CardArea cardarea = this.transform.Find(name).GetComponent<CardArea>();
      cardarea.m_CardAreaName = name;
      if (cardarea.transform.Find("Text_Count"))
      {
        if (cardarea.transform.Find("Text_Count").GetComponent<Text>())
        {
          cardarea.m_TextCount = cardarea.transform.Find("Text_Count").GetComponent<Text>();
        }
      }
      cardarea.m_AreaList = new List<Card>();
      CardAreaList.Add(cardarea);
    }
  }

  /// <summary>
  /// 初始化角色
  /// </summary>
  void InitPlayer(Player p,string name,int hp,int cost,bool isplayer)
  {
    p.m_PlayerName = name;
    p.m_HP = hp;
    p.m_Cost = cost;
    p.m_BattleCardList = new List<Card>();
    p.m_IsPlayer = isplayer;
  }

  /// <summary>
  /// 初始化卡牌
  /// </summary>
  void InitCard(CardArea area, string name, int cost, int hp = 0,int atk = 0, Card.CardType cardtype = Card.CardType.Character, Card.HurtEffect hurteffect = Card.HurtEffect.Normal)
  {
    Card card = area.gameObject.AddComponent<Card>();
    card.m_CardName =name;
    card.m_HP = hp;
    card.m_ATK = atk;
    card.m_Cost = cost;
    card.m_CardType = cardtype;
    card.m_HurtEffect = hurteffect;
    area.m_AreaList.Add(card);
  }

  /// <summary>
  /// 清场，将战斗元素准备好
  /// </summary>
  void InitBattleGround()
  {
    player.PrepareForBattle();
    enemy.PrepareForBattle();
    
  }

	// Update is called once per frame
	void Update () {
    UpdateHandCard();
    ShowDeckCard();
  }

  /// <summary>
  /// 显示卡组的剩余卡数
  /// </summary>
  void ShowDeckCard()
  {
    foreach (CardArea area in CardAreaList)
    {
      if (area.m_TextCount != null)
      {
        area.m_TextCount.text = area.m_AreaList.Count.ToString();
      }
    }
  }

  void UpdateHandCard()
  {
  }

  public static void OnButtonClick()
  {

  }
}
