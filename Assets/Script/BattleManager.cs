using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

  public List<Card> PlayerHandCardList = new List<Card>();
  public List<Card> PlayerBattleCardList = new List<Card>();
  public List<Card> PlayerDeckCardList = new List<Card>();
  public List<Card> PlayerDropCardList = new List<Card>();
  public List<Card> EnemyBattleCardList = new List<Card>();
  public List<Card> EnemyPreCardList = new List<Card>();
  public Player player;
  public Player enemy;
  public GameObject PlayerHandArea;
  public GameObject pfb;

  public GameLogic gameLogic;

  // Use this for initialization
  void Start () {

    InitBattleData();

    InitBattleGround();
  }
  /// <summary>
  /// 临时数据初始化，后期会通过读表获取
  /// </summary>
  void InitBattleData()
  {
    player.m_PlayerName = "圣骑士";
    player.m_HP = 35;
    player.m_Cost = 3;
    player.m_BattleCardList = new List<Card>();
    player.m_IsPlayer = true;

    Card card = this.gameObject.AddComponent<Card>();
    card.m_CardName = "圣教军";
    card.m_HP = 3;
    card.m_ATK = 3;
    card.m_Cost = 1;
    card.m_CardType = Card.CardType.Character;
    card.m_HurtEffect = Card.HurtEffect.Normal;
    player.m_BattleCardList.Add(card);

    Card card2 = this.gameObject.AddComponent<Card>();
    card2.m_CardName = "弓箭手";
    card2.m_HP = 1;
    card2.m_ATK = 5;
    card2.m_Cost = 1;
    card2.m_CardType = Card.CardType.Character;
    card2.m_HurtEffect = Card.HurtEffect.Normal;
    player.m_BattleCardList.Add(card2);

    enemy.m_PlayerName = "野蛮人";
    enemy.m_HP = 30;
    enemy.m_Cost = 3;
    enemy.m_BattleCardList = new List<Card>();
    enemy.m_IsPlayer = false;

    Card card3 = this.gameObject.AddComponent<Card>();
    card3.m_CardName = "蛮族勇士";
    card3.m_HP = 3;
    card3.m_ATK = 3;
    card3.m_Cost = 1;
    card3.m_CardType = Card.CardType.Character;
    card3.m_HurtEffect = Card.HurtEffect.Normal;
    enemy.m_BattleCardList.Add(card3);

    Card card4 = this.gameObject.AddComponent<Card>();
    card4.m_CardName = "蛮力一击";
    card4.m_ATK = 8;
    card4.m_Cost = 2;
    card4.m_CardType = Card.CardType.Magic;
    card4.m_HurtEffect = Card.HurtEffect.Normal;
    enemy.m_BattleCardList.Add(card4);
    player.m_BattleCardList.Add(card4);
  }

  /// <summary>
  /// 清场，将战斗元素准备好
  /// </summary>
  void InitBattleGround()
  {
    player.PrepareForBattle();
    enemy.PrepareForBattle();

    foreach (Card card in player.m_BattleCardList)
    {
      //pfb = Resources.Load("Resource/HandCardArea") as GameObject;
      
      GameObject prefabInstance = Instantiate(pfb) as GameObject;
      prefabInstance.transform.parent = PlayerHandArea.transform;
      prefabInstance.GetComponents<Card>()[0].InitByClone(card);
      prefabInstance.GetComponents<Card>()[0].PrepareForBattle();
      PlayerHandCardList.Add(prefabInstance.GetComponents<Card>()[0]);
    }
  }

	// Update is called once per frame
	void Update () {
    UpdateHandCard();
  }

  void UpdateHandCard()
  {
  }

  public bool EnterBattleGround(Card card,int row,int column)
  {
    foreach (Card ongroundcard in PlayerBattleCardList)
    {
      if (ongroundcard.GetPlace(row, column))
      {
        return false;
      }
    }
    card.SetPlace(row, column);
    PlayerBattleCardList.Add(card);
    PlayerHandCardList.Remove(card);
    return true;
  }

  public static void OnButtonClick()
  {

  }
}
