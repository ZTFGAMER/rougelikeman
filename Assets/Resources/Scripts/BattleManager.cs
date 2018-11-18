using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

  public List<CardArea> CardAreaList;
  public Player player;
  public Player enemy;

  public bool bDrawCard = true;
  public bool bDropCard = false;
  public int iDrawCardCount = 0;
  public bool bShuffle = false;
  public bool bIsPlayerTurn = true;
  public bool bSelectHandCard = false;
  public Card cSelectCard;

  public int Level = 0;

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

    InitCardAreaList("PlayerHandArea");
    InitCardAreaList("EnemyHandArea");
    InitCardAreaList("PlayerBattleArea");
    InitCardAreaList("EnemyBattleArea");
    InitCardAreaList("PlayerDeckArea");
    InitCardAreaList("EnemyDeckArea");
    InitCardAreaList("PlayerDropArea");
    InitCardAreaList("EnemyDropArea");

    InitPlayer(player, "圣骑士", 35, 3, true);
    InitPlayer(enemy, "野蛮人", 30, 3, false);

    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "士兵","近卫兵", 1, 3, 3);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "士兵", "近卫兵", 1, 3, 3);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "士兵", "近卫兵", 1, 3, 3);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "弓箭手", "黄忠(骑马)", 1, 1, 5);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "弓箭手", "黄忠(骑马)", 1, 1, 5);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "弓箭手", "黄忠(骑马)", 1, 1, 5);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "盾手", "曹仁", 1, 5, 1);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "盾手", "曹仁", 1, 5, 1);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "盾手", "曹仁", 1, 5, 1);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "冲击手", "徐晃", 2, 3, 3, Card.CardType.Character, Card.HurtEffect.Puncture);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "指挥官", "夏侯敦(骑马)", 2, 4, 2);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "冲锋", "近卫兵", 1, 0, 0, Card.CardType.Magic);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "爆发", "近卫兵", 0, 0, 0, Card.CardType.Magic);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "坚守", "近卫兵", 1, 0, 0, Card.CardType.Magic);
    InitCard(FindCardAreaListByName( "PlayerDeckArea"), "巨盾", "近卫兵", 2, 0, 0, Card.CardType.Magic);

    InitCard(FindCardAreaListByName( "EnemyDeckArea"), "蛮族勇士", "黄盖", 1, 2, 2);
    InitCard(FindCardAreaListByName( "EnemyDeckArea"), "蛮族勇士", "黄盖", 1, 2, 2);
    InitCard(FindCardAreaListByName( "EnemyDeckArea"), "蛮族勇士", "黄盖", 1, 2, 2);
    InitCard(FindCardAreaListByName("EnemyDeckArea"), "蛮族刺客", "甘宁", 1, 1, 3, Card.CardType.Character, Card.HurtEffect.Backstab);
    InitCard(FindCardAreaListByName("EnemyDeckArea"), "蛮族刺客", "甘宁", 1, 1 ,3, Card.CardType.Character, Card.HurtEffect.Backstab);
    InitCard(FindCardAreaListByName( "EnemyDeckArea"), "蛮族巫师", "谋士", 1, 1, 2, Card.CardType.Character, Card.HurtEffect.Puncture);
    
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
      UpdateCardAreaListCount(cardarea);
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
  void InitCard(CardArea area, string name,string animname, int cost, int hp = 0,int atk = 0, Card.CardType cardtype = Card.CardType.Character, Card.HurtEffect hurteffect = Card.HurtEffect.Normal)
  {
    GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HandCard");
    Card card = Instantiate(toInstantiate, this.transform.Find("Recycle")).GetComponent<Card>();
    card.m_CardName =name;
    card.m_HP = hp;
    card.m_ATK = atk;
    card.m_Cost = cost;
    card.m_CardType = cardtype;
    card.m_HurtEffect = hurteffect;
    card.InitAnimation(animname);
    card.battleManager = this;
    card.PrepareForBattle();
    area.m_AreaList.Add(card);

  }

  void InitBattleCube(CardArea area, int row = 1, int column = 1)
  {
    for (int i = 0; i < row * column; i++)
    {
      GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/BattleCube");
      BattleCube cube = Instantiate(toInstantiate, area.transform).GetComponent<BattleCube>();
      cube.m_Row = i / column;
      cube.m_Column = i % column;
      cube.battlemanager = this;
    }
  }
  /// <summary>
  /// 清场，将战斗元素准备好
  /// </summary>
  void InitBattleGround()
  {
    player.PrepareForBattle();
    enemy.PrepareForBattle();
    InitBattleCube(FindCardAreaListByName("PlayerBattleArea"),3,3);
    InitBattleCube(FindCardAreaListByName("EnemyBattleArea"),3,3);
  }

	// Update is called once per frame
	void Update () {
    if (bDrawCard)
    {
      UpdateAICard();
      DrawCard(FindCardAreaListByName("PlayerDropArea"), FindCardAreaListByName( "PlayerDeckArea"), FindCardAreaListByName( "PlayerHandArea"));
    }
    if (bDropCard)
    {
      DropCard(FindCardAreaListByName("PlayerDropArea"), FindCardAreaListByName("PlayerDeckArea"), FindCardAreaListByName("PlayerHandArea"));
      UpdateBattle();
      
    }
    foreach (CardArea area in CardAreaList)
    {
      UpdateCardAreaListCount(area);
    }
    foreach (CardArea area in CardAreaList)
    {
      UpdateCardAnimation(area);
    }
  }

  void UpdateCardAnimation(CardArea area)
  {
    if (area.m_CardAreaName.Contains("PlayerHand"))
    {
      foreach (Card card in area.m_AreaList)
      {
        if(!card.m_IsBattleDead)
          card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunBack);
      }
    }
    //if (area.m_CardAreaName.Contains("EnemyHand"))
    //{
    //  foreach (Card card in area.m_AreaList)
    //  {
    //    if (!card.m_IsBattleDead)
    //      card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunToward);
    //  }
    //}
    if (area.m_CardAreaName.Contains("PlayerBattle"))
    {
      foreach (Card card in area.m_AreaList)
      {
        if (!card.m_IsBattleDead)
          card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.AttackBack);
      }
    }
    //if (area.m_CardAreaName.Contains("EnemyBattle"))
    //{
    //  foreach (Card card in area.m_AreaList)
    //  {
    //    if (!card.m_IsBattleDead)
    //      card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.AttackToward);
    //  }
    //}
  }

  void UpdateAICard()
  {
    for(int i = 0;i < Level; i++)
    {
      BattleCube cube = FindCardAreaListByName("EnemyBattleArea").transform.GetChild((Random.Range(0,9))).GetComponent<BattleCube>();
      if (cube.transform.childCount == 0)
      {
        GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HandCard");
        Card card = Instantiate(toInstantiate, cube.transform).GetComponent<Card>();
        card.gameObject.transform.position = cube.gameObject.transform.position;
        card.InitByClone(FindCardAreaListByName("EnemyDeckArea").m_AreaList[Random.Range(0, FindCardAreaListByName("EnemyDeckArea").m_AreaList.Count)],this);
        card.m_IsInBattleGround = true;
        card.m_BattleRow = cube.m_Row;
        card.m_BattleColumn = cube.m_Column;
        FindCardAreaListByName("EnemyBattleArea").m_AreaList.Add(cSelectCard);
      }
    }
  }

  void UpdateBattle()
  {

    enemy.m_CurrentHP -= BattleCalculation(FindCardAreaListByName("PlayerBattleArea"), FindCardAreaListByName("EnemyBattleArea"), 3, 3, 0);
    player.m_CurrentHP -= BattleCalculation(FindCardAreaListByName("EnemyBattleArea"), FindCardAreaListByName("PlayerBattleArea"), 3, 3, 0);
    enemy.m_CurrentHP -= BattleCalculation(FindCardAreaListByName("PlayerBattleArea"), FindCardAreaListByName("EnemyBattleArea"), 3, 3, 1);
    player.m_CurrentHP -= BattleCalculation(FindCardAreaListByName("EnemyBattleArea"), FindCardAreaListByName("PlayerBattleArea"), 3, 3, 1);
    enemy.m_CurrentHP -= BattleCalculation(FindCardAreaListByName("PlayerBattleArea"), FindCardAreaListByName("EnemyBattleArea"), 3, 3, 2);
    player.m_CurrentHP -= BattleCalculation(FindCardAreaListByName("EnemyBattleArea"), FindCardAreaListByName("PlayerBattleArea"), 3, 3, 2);
    BattleCalculationDead(FindCardAreaListByName("EnemyBattleArea"),3,3);
    BattleCalculationDead(FindCardAreaListByName("PlayerBattleArea"), 3, 3);

    if (enemy.m_CurrentHP <= 0)
    {
      NextStage();
    }

    if (player.m_CurrentHP <= 0)
    {
      Dead();
    }
    bDrawCard = true;
  }

  public void Dead()
  {
    this.transform.Find("BattleEnd").gameObject.SetActive(true);
  }

  void NextStage()
  {
    Shuffle(FindCardAreaListByName("PlayerHandArea"), FindCardAreaListByName("PlayerDeckArea"));
    Shuffle(FindCardAreaListByName("PlayerDropArea"), FindCardAreaListByName("PlayerDeckArea"));
    enemy.m_CurrentHP = enemy.m_HP;
    for (int i = 0; i < 3 * 3; i++)
    {
      BattleCube cube1 = FindCardAreaListByName("PlayerBattleArea").transform.GetChild(i).GetComponent<BattleCube>();
      BattleCube cube2 = FindCardAreaListByName("EnemyBattleArea").transform.GetChild(i).GetComponent<BattleCube>();
      if (cube1 != null && cube1.transform.childCount > 0)
      {
        FindCardAreaListByName("PlayerBattleArea").m_AreaList.Remove(cube1.transform.GetChild(0).gameObject.GetComponent<Card>());
        Destroy(cube1.transform.GetChild(0).gameObject);
      }
      if (cube2 != null && cube2.transform.childCount > 0)
      {
        FindCardAreaListByName("EnemyBattleArea").m_AreaList.Remove(cube2.transform.GetChild(0).gameObject.GetComponent<Card>());
        Destroy(cube2.transform.GetChild(0).gameObject);
      }
      
    }

    Level++;
    enemy.m_PlayerName = "野蛮人 Lv" + Level;
    this.transform.Find("Text_Stage").GetComponent<Text>().text = "Stage 1-" + Level;
    bDrawCard = true;
  }
  int BattleCalculation(CardArea atkarea,CardArea defarea, int row,int column,int currentcolumn)
  {
    int atk = 0;
    Card.HurtEffect hurteffect = Card.HurtEffect.Normal;
    for (int i = 0; i < row * column; i++)
    {
      BattleCube cube = atkarea.transform.GetChild(i).GetComponent<BattleCube>();
      if (cube != null && cube.m_Column == currentcolumn && cube.transform.childCount > 0)
      {
        atk += cube.transform.GetChild(0).GetComponent<Card>().m_CurrentATK;
        if (cube.transform.GetChild(0).GetComponent<Card>().m_HurtEffect > hurteffect)
        {
          hurteffect = cube.transform.GetChild(0).GetComponent<Card>().m_HurtEffect;
        }
      }
    }
    if (hurteffect == Card.HurtEffect.Backstab)
    {
      for (int i = row * column - 1; i >= 0; i--)
      {
        BattleCube cube = defarea.transform.GetChild(i).GetComponent<BattleCube>();
        if (cube != null && cube.m_Column == currentcolumn && cube.transform.childCount > 0)
        {
          atk = cube.transform.GetChild(0).GetComponent<Card>().GetHurt(atk);
        }
      }
    }
    else
    {
      for (int i = 0; i < row * column; i++)
      {
        BattleCube cube = defarea.transform.GetChild(i).GetComponent<BattleCube>();
        if (cube != null && cube.m_Column == currentcolumn && cube.transform.childCount > 0)
        {
          if (hurteffect == Card.HurtEffect.Normal)
          {
            atk = cube.transform.GetChild(0).GetComponent<Card>().GetHurt(atk);
          }
          if (hurteffect == Card.HurtEffect.Puncture)
          {
            atk = cube.transform.GetChild(0).GetComponent<Card>().GetHurtByPuncture(atk);
          }
          if (hurteffect == Card.HurtEffect.Penetrate)
          {
            cube.transform.GetChild(0).GetComponent<Card>().GetHurt(atk);
          }
        }
      }
    }
    return atk;
  }

  void BattleCalculationDead(CardArea area,int row,int column)
  {
    for (int i = 0; i < row*column; i++)
    {
      BattleCube cube = area.transform.GetChild(i).GetComponent<BattleCube>();
      if (cube.transform.childCount > 0)
      {
        if (cube.transform.GetChild(0).GetComponent<Card>().m_IsBattleDead)
        {
          //if (cube.transform.GetChild(0).GetComponent<Card>().m_DeadAnimTime < UGUISpriteAnimation.FRAMEBASE / UGUISpriteAnimation.FPS)
          //{
          //  cube.transform.GetChild(0).GetComponent<Card>().m_DeadAnimTime += Time.deltaTime;
          //}
          //else
          //{ 
            area.m_AreaList.Remove(cube.transform.GetChild(0).gameObject.GetComponent<Card>());
            Destroy(cube.transform.GetChild(0).gameObject);
          //}
        }
      }
    }
  }
/// <summary>
/// 更新区域显示
/// </summary>
/// <param name="area"></param>
void UpdateCardAreaListCount(CardArea area)
  {
    if (area.transform.Find("Text_Count"))
    {
      if (area.transform.Find("Text_Count").GetComponent<Text>())
      {
        area.m_TextCount = area.transform.Find("Text_Count").GetComponent<Text>();
        area.m_TextCount.text = area.m_AreaList.Count.ToString();
      }
    }
  }


  /// <summary>
  /// 洗牌
  /// </summary>
  void Shuffle(CardArea areaout,CardArea areain)
  {
    int countNum = areaout.m_AreaList.Count;
    while (countNum > areain.m_AreaList.Count)
    {
      int index = Random.Range(0, areaout.m_AreaList.Count);
      if (!areain.m_AreaList.Contains(areaout.m_AreaList[index]))
      {
        ChangeCardArea(areain, areaout.m_AreaList[index]);
        areain.m_AreaList.Add(areaout.m_AreaList[index]);
        areaout.m_AreaList.Remove(areaout.m_AreaList[index]);
      }
    }
    UpdateCardAreaListCount(areaout);
    UpdateCardAreaListCount(areain);
  }

  void DrawCard(CardArea areadrop, CardArea areadeck, CardArea areahand)
  {
    while (iDrawCardCount < player.m_DrawCardCount)
    {
      if (areadeck.m_AreaList.Count == 0)
      {
        Shuffle(areadrop, areadeck);
      }
      if (areadeck.m_AreaList.Count >= 1)
      {
        ChangeCardArea(areahand, areadeck.m_AreaList[0]);
        areahand.m_AreaList.Add(areadeck.m_AreaList[0]);
        areadeck.m_AreaList.Remove(areadeck.m_AreaList[0]);
        iDrawCardCount++;
      }
      else
      {
        return;
      }
    }
    UpdateCardAreaListCount(areadeck);
    bDrawCard = false;
    iDrawCardCount = 0;
  }

  void DropCard(CardArea areadrop, CardArea areadeck, CardArea areahand)
  {
    for(int i = areahand.m_AreaList.Count-1; i >=0; i--)
    {
      ChangeCardArea(areadrop, areahand.m_AreaList[i]);
      areadrop.m_AreaList.Add(areahand.m_AreaList[i]);
      areahand.m_AreaList.Remove(areahand.m_AreaList[i]);
    }
    UpdateCardAreaListCount(areadrop);
    bDropCard = false;
  }


  CardArea FindCardAreaListByName(string name)
  {
    foreach (CardArea area in CardAreaList)
    {
      if (area.m_CardAreaName == name)
        return area;
    }
    return null;
  }

  void ChangeCardArea( CardArea areanew, Card card)
  {
    if (!areanew.m_CardAreaName.Contains("Hand"))
    {
      card.transform.SetParent(this.transform.Find("Recycle"));
    }
    else
    {
      card.transform.SetParent(areanew.transform);
    }
  }




  public void SelectBattleCube(BattleCube cube)
  {
    foreach(Card card in FindCardAreaListByName("PlayerBattleArea").m_AreaList)
    {
      if (card.m_BattleColumn == cube.m_Column && card.m_BattleRow == cube.m_Row)
      {
        return;
      }
    }
    if (cSelectCard != null && player.m_CurrentCost >= cSelectCard.m_Cost)
    {
      player.m_CurrentCost -= cSelectCard.m_Cost;
      cSelectCard.m_IsSelected = false;
      cSelectCard.m_IsInBattleGround = true;
      cSelectCard.gameObject.transform.SetParent(cube.gameObject.transform);
      cSelectCard.gameObject.transform.position = cube.gameObject.transform.position;
      cSelectCard.m_BattleRow = cube.m_Row;
      cSelectCard.m_BattleColumn = cube.m_Column;

      CheckSpecial(cSelectCard,FindCardAreaListByName("PlayerBattleArea"));

      FindCardAreaListByName("PlayerBattleArea").m_AreaList.Add(cSelectCard);
      GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HandCard");
      Card card = Instantiate(toInstantiate, this.transform.Find("Recycle")).GetComponent<Card>();
      card.InitByClone(cSelectCard,this);
      FindCardAreaListByName("PlayerDropArea").m_AreaList.Add(card);
      FindCardAreaListByName("PlayerHandArea").m_AreaList.Remove(cSelectCard);
      cSelectCard = null;
    }
    bSelectHandCard = false;
  }

  void CheckSpecial(Card card,CardArea area)
  {
    if (card.m_CardName == "指挥官")
    {
      card.m_CurrentHP += area.m_AreaList.Count;
    }
    if (card.m_CardName == "盾手")
    {
      card.m_Armor = 2;
    }
    if (card.m_CardName == "蛮族勇士")
    {
      card.m_Armor = 2;
    }
    foreach (Card forcard in area.m_AreaList)
    {
      if (card.m_CardName == "冲锋" && forcard.m_CardType == Card.CardType.Character && card.m_BattleColumn == forcard.m_BattleColumn)
      {
        forcard.m_CurrentATK += 2;
      }
      if (card.m_CardName == "爆发" && forcard.m_CardType == Card.CardType.Character && card.m_BattleColumn == forcard.m_BattleColumn)
      {
        forcard.m_CurrentATK *= 3;
        forcard.m_CurrentHP = 1;
      }
      if (card.m_CardName == "坚守" && forcard.m_CardType == Card.CardType.Character && card.m_BattleColumn == forcard.m_BattleColumn)
      {
        forcard.m_CurrentArmor *= 2;
      }
      if (card.m_CardName == "巨盾" && forcard.m_CardType == Card.CardType.Character && forcard.m_BattleRow == 0)
      {
        forcard.m_CurrentArmor += 4;
      }
    }
  }

  public void SelectHandCard(Card card)
  {
    foreach (Card forcard in FindCardAreaListByName("PlayerHandArea").m_AreaList)
    {
      if (forcard != card)
      {
        forcard.m_IsSelected = false;
      }
    }
    card.m_IsSelected = true;
    cSelectCard = card;
    bSelectHandCard = true;
  }

  public void OnDropButtonClick()
  {
    player.m_CurrentCost = player.m_Cost;
    bDropCard = true;
  }
}
