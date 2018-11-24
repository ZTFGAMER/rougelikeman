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
  public bool bShowAttack = false;
  public int iDrawCardCount = 0;
  public bool bShuffle = false;
  public bool bIsPlayerTurn = true;
  public Card cSelectCard;
  public int Level = 1;
  
  void Start() {

    InitBattleData();

    InitBattleGround();
  }
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

    InitPlayer(player, "圣骑士", 35, 3, true, "曹操(骑马)");
    InitPlayer(enemy, "野蛮人", 30, 3, false, "陆逊");
    player.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunBack);
    enemy.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunToward);

    FindCardAreaListByName("PlayerDropArea").InitCard(false,"士兵","近卫兵", 1, 3, 3);
    FindCardAreaListByName("PlayerDropArea").InitCard(false, "士兵", "近卫兵", 1, 3, 3);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "士兵", "近卫兵", 1, 3, 3);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "弓箭手", "黄忠(骑马)", 1, 1, 4, Card.CardType.Character, Card.HurtEffect.Backstab);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "弓箭手", "黄忠(骑马)", 1, 1, 4, Card.CardType.Character, Card.HurtEffect.Backstab);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "弓箭手", "黄忠(骑马)", 1, 1, 4, Card.CardType.Character, Card.HurtEffect.Backstab);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "盾手", "曹仁", 1, 5, 1);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "盾手", "曹仁", 1, 5, 1);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "盾手", "曹仁", 1, 5, 1);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "冲击手", "徐晃", 2, 3, 3, Card.CardType.Character, Card.HurtEffect.Penetrate);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "指挥官", "夏侯敦(骑马)", 2, 2, 2);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "冲锋", "magiccross", 1, 0, 0, Card.CardType.Magic);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "爆发", "magiccolumn", 0, 0, 0, Card.CardType.Magic);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "坚守", "magicrow", 1, 0, 0, Card.CardType.Magic);
    FindCardAreaListByName( "PlayerDropArea").InitCard(false, "巨盾", "magicall", 2, 0, 0, Card.CardType.Magic);

    FindCardAreaListByName( "EnemyDeckArea").InitCard(true, "蛮族勇士", "黄盖", 1, 2, 2);
    FindCardAreaListByName( "EnemyDeckArea").InitCard(true, "蛮族勇士", "黄盖", 1, 2, 2);
    FindCardAreaListByName( "EnemyDeckArea").InitCard(true, "蛮族勇士", "黄盖", 1, 2, 2);
    FindCardAreaListByName("EnemyDeckArea").InitCard(true, "蛮族刺客", "甘宁", 1, 1, 3, Card.CardType.Character, Card.HurtEffect.Backstab);
    FindCardAreaListByName("EnemyDeckArea").InitCard(true, "蛮族刺客", "甘宁", 1, 1 ,3, Card.CardType.Character, Card.HurtEffect.Backstab);
    FindCardAreaListByName( "EnemyDeckArea").InitCard(true, "蛮族巫师", "谋士", 1, 1, 2, Card.CardType.Character, Card.HurtEffect.Penetrate);

    bDrawCard = true;
  }
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
      cardarea.battlemanager = this;
      UpdateCardAreaListCount(cardarea);
      cardarea.m_AreaList = new List<Card>();
      CardAreaList.Add(cardarea);
    }
  }
  void InitPlayer(Player p,string name,int hp,int cost,bool isplayer,string animname)
  {
    p.m_PlayerName = name;
    p.m_HP = hp;
    p.m_Cost = cost;
    p.m_BattleCardList = new List<Card>();
    p.m_IsPlayer = isplayer;
    p.InitAnimation(animname);
  }
  void InitBattleCube(CardArea area, int row = 1, int column = 1)
  {
    for (int i = 0; i < row * column; i++)
    {
      GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/BattleCube");
      BattleCube cube = Instantiate(toInstantiate, area.transform).GetComponent<BattleCube>();
      cube.m_Row = i / column;
      cube.m_Column = i % column;
      if (area.m_CardAreaName.Contains("Player"))
      {
        cube.m_IsPlayer = true;
      }
      cube.battlemanager = this;
    }
  }
  void InitBattleGround()
  {
    player.PrepareForBattle();
    enemy.PrepareForBattle();
    InitBattleCube(FindCardAreaListByName("PlayerBattleArea"),3,3);
    InitBattleCube(FindCardAreaListByName("EnemyBattleArea"),3,3);
  }
	public void Tick () {
    if (bDrawCard)
    {
      UpdateAICard();
      DrawCard(FindCardAreaListByName("PlayerDropArea"), FindCardAreaListByName( "PlayerDeckArea"), FindCardAreaListByName( "PlayerHandArea"));
    }
    if (bDropCard)
    {

      DropCard(FindCardAreaListByName("PlayerDropArea"), FindCardAreaListByName("PlayerDeckArea"), FindCardAreaListByName("PlayerHandArea"));
      StartCoroutine(UpdateBattle());
      
    }
    foreach (CardArea area in CardAreaList)
    {
      UpdateCardAreaListCount(area);
      if (!bShowAttack)
        UpdateCardAnimation(area);
      UpdateCardTick(area);
    }
    player.Tick();
    enemy.Tick();
  }
  void UpdateCardTick(CardArea area)
  {
    foreach (Card card in area.m_AreaList)
    {
      card.Tick();
    }
  }
  void UpdateCardAnimation(CardArea area)
  {
    if (area.m_CardAreaName.Contains("PlayerHand"))
    {
      foreach (Card card in area.m_AreaList)
      {
        if(card.m_CardType == Card.CardType.Character)
          card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunBack);
      }
    }
    if (area.m_CardAreaName.Contains("EnemyHand"))
    {
      foreach (Card card in area.m_AreaList)
      {
        if (card.m_CardType == Card.CardType.Character)
          card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunToward);
      }
    }
    if (area.m_CardAreaName.Contains("PlayerBattle"))
    {
      foreach (Card card in area.m_AreaList)
      {
        if (!card.m_IsBattleDead && card.m_CardType == Card.CardType.Character)
          card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunBack);
      }
    }
    if (area.m_CardAreaName.Contains("EnemyBattle"))
    {
      foreach (Card card in area.m_AreaList)
      {
        if (!card.m_IsBattleDead && card.m_CardType == Card.CardType.Character)
          card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunToward);
      }
    }
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
        card.InitByClone(FindCardAreaListByName("EnemyDeckArea").m_AreaList[Random.Range(0, FindCardAreaListByName("EnemyDeckArea").m_AreaList.Count)]);
        card.m_IsInBattleGround = true;
        card.m_BattleRow = cube.m_Row;
        card.m_BattleColumn = cube.m_Column;
        FindCardAreaListByName("EnemyBattleArea").m_AreaList.Add(card);
      }
    }
    ClearCalculationPre(FindCardAreaListByName("PlayerBattleArea"), FindCardAreaListByName("EnemyBattleArea"));
    enemy.SetCurrentHurt(BattleCalculationPre(FindCardAreaListByName("PlayerBattleArea"), FindCardAreaListByName("EnemyBattleArea")));
    player.SetCurrentHurt(BattleCalculationPre(FindCardAreaListByName("EnemyBattleArea"), FindCardAreaListByName("PlayerBattleArea")));
  }
  IEnumerator UpdateBattle()
  {
    bShowAttack = true;
    for (int i = 0; i < 3; i++)
    {
      enemy.SetCurrentHP(-BattleCalculation(FindCardAreaListByName("PlayerBattleArea"), FindCardAreaListByName("EnemyBattleArea"), 3, 3, i, 0));
      player.SetCurrentHP(-BattleCalculation(FindCardAreaListByName("EnemyBattleArea"), FindCardAreaListByName("PlayerBattleArea"), 3, 3, i, 0));
      enemy.SetCurrentHP(-BattleCalculation(FindCardAreaListByName("PlayerBattleArea"), FindCardAreaListByName("EnemyBattleArea"), 3, 3, i, 1));
      player.SetCurrentHP(-BattleCalculation(FindCardAreaListByName("EnemyBattleArea"), FindCardAreaListByName("PlayerBattleArea"), 3, 3, i, 1));
      enemy.SetCurrentHP(-BattleCalculation(FindCardAreaListByName("PlayerBattleArea"), FindCardAreaListByName("EnemyBattleArea"), 3, 3, i, 2));
      player.SetCurrentHP(-BattleCalculation(FindCardAreaListByName("EnemyBattleArea"), FindCardAreaListByName("PlayerBattleArea"), 3, 3, i, 2));

      ClearCalculationPre(FindCardAreaListByName("PlayerBattleArea"), FindCardAreaListByName("EnemyBattleArea"),false, false,i);
      bool hascard = false;

      foreach (Card card in FindCardAreaListByName("PlayerBattleArea").m_AreaList)
      {
        if (card.m_BattleColumn == i)
        {
          hascard = true;
          card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.AttackBack);
        }
      }
      foreach (Card card in FindCardAreaListByName("EnemyBattleArea").m_AreaList)
      {
        if (card.m_BattleColumn == i)
        {
          hascard = true;
          card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.AttackToward);
        }
      }
      if (hascard)
      {
        yield return new WaitForSeconds(5 / UGUISpriteAnimation.FPS);
      }

      bool iscarddead = false;

      foreach (Card card in FindCardAreaListByName("PlayerBattleArea").m_AreaList)
      {
        if (card.m_BattleColumn == i)
        {
          if (card.m_IsBattleDead)
          {
            iscarddead = true;
            card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.Dead);
          }
          else
          {
            card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunBack);
          }
        }
      }
      foreach (Card card in FindCardAreaListByName("EnemyBattleArea").m_AreaList)
      {
        if (card.m_BattleColumn == i)
        {
          if (card.m_IsBattleDead)
          {
            iscarddead = true;
            card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.Dead);
          }
          else
          {
            card.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunToward);
          }
        }
      }
      if (iscarddead)
      {
        yield return new WaitForSeconds(13 / UGUISpriteAnimation.FPS);
      }
      BattleCalculationDead(FindCardAreaListByName("EnemyBattleArea"), i);
      BattleCalculationDead(FindCardAreaListByName("PlayerBattleArea"), i);

    }

    if (enemy.m_CurrentHP <= 0)
    {
      enemy.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.Dead);
      yield return new WaitForSeconds(13 / UGUISpriteAnimation.FPS);
      NextStage();
    }

    if (player.m_CurrentHP <= 0)
    {
      player.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.Dead);
      yield return new WaitForSeconds(13 / UGUISpriteAnimation.FPS);
      Dead();
    }
    bShowAttack = false;
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
    player.SetCurrentHP(10);
    enemy.m_PlayerName = "野蛮人 Lv" + Level;
    this.transform.Find("Text_Stage").GetComponent<Text>().text = "Stage 1-" + Level;
    enemy.animationConfig.SetAnimState(UGUISpriteAnimation.AnimState.RunToward);
    bDrawCard = true;
  }
  int BattleCalculation(CardArea atkarea,CardArea defarea, int row,int column,int currentcolumn,int currrentrow)
  {
    int atk = 0;
    Card.HurtEffect hurteffect = Card.HurtEffect.Normal;
    for (int i = 0; i < row * column; i++)
    {
      BattleCube cube = atkarea.transform.GetChild(i).GetComponent<BattleCube>();
      if (cube != null && cube.m_Column == currentcolumn && cube.m_Row == currrentrow && cube.transform.childCount > 0)
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
      if (hurteffect == Card.HurtEffect.Penetrate)
      {
        atk = 0;
      }
    }
    return atk;
  }
  void BattleCalculationDead(CardArea area,int currentcolumn)
  {
    for (int i = 0; i < 3; i++)
    {
      BattleCube cube = area.transform.GetChild(currentcolumn + i * 3).GetComponent<BattleCube>();
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
  int BattleCalculationPre(CardArea area1, CardArea area2)
  {
    int atk2 = 0;
    for (int i = 0; i < 9; i++)
    {
      int atk = 0;
      Card.HurtEffect hurteffect = Card.HurtEffect.Normal;
      BattleCube cube = area1.transform.GetChild(i).GetComponent<BattleCube>();
      if (cube != null && cube.transform.childCount > 0)
      {
        atk += cube.transform.GetChild(0).GetComponent<Card>().m_CurrentATK;
        hurteffect = cube.transform.GetChild(0).GetComponent<Card>().m_HurtEffect;
        if (hurteffect == Card.HurtEffect.Backstab)
        {
          for (int j = 8; j >= 0; j--)
          {
            BattleCube cube2 = area2.transform.GetChild(j).GetComponent<BattleCube>();
            if (cube2 != null && cube2.m_Column == cube.m_Column && cube2.transform.childCount > 0)
            {
              atk = cube2.transform.GetChild(0).GetComponent<Card>().GetHurtPre(atk);
            }
          }
        }
        else
        {
          for (int j = 0; j < 9; j++)
          {
            BattleCube cube2 = area2.transform.GetChild(j).GetComponent<BattleCube>();
            if (cube2 != null && cube2.m_Column == cube.m_Column && cube2.transform.childCount > 0)
            {
              if (hurteffect == Card.HurtEffect.Normal)
              {
                atk = cube2.transform.GetChild(0).GetComponent<Card>().GetHurtPre(atk);
              }
              //if (hurteffect == Card.HurtEffect.Puncture)
              //{
              //  atk = cube.transform.GetChild(0).GetComponent<Card>().GetHurtByPuncture(atk);
              //}
              if (hurteffect == Card.HurtEffect.Penetrate)
              {
                cube2.transform.GetChild(0).GetComponent<Card>().GetHurtPre(atk);
              }
            }
          }
          if (hurteffect == Card.HurtEffect.Penetrate)
          {
            atk = 0;
          }
        }
        cube.transform.GetChild(0).GetComponent<Card>().GetHurtAPra(atk);
      }
      atk2 += atk;
    }
    return atk2;
  }
  void ClearCalculationPre(CardArea area1, CardArea area2,bool isplayer = true,bool ishurta = true,int column = 3)
  {
    if(isplayer)
    { 
      player.m_CurrentHurt = 0;
      enemy.m_CurrentHurt = 0;
    }
    foreach (Card card in area1.m_AreaList)
    {
      if(column == 3 || card.m_BattleColumn == column)
      {
        card.m_CurrentHurt = 0;
        if(ishurta)
          card.m_CurrentHurtA = 0;
      }
    }
    foreach (Card card in area2.m_AreaList)
    {
      if (column == 3 || card.m_BattleColumn == column)
      {
        card.m_CurrentHurt = 0;
        if (ishurta)
          card.m_CurrentHurtA = 0;
      }
    }
  }
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
    if (!cube.m_IsPlayer)
    {
      return;
    }
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

      if (cSelectCard.m_CardType == Card.CardType.Character)
      {
        FindCardAreaListByName("PlayerBattleArea").m_AreaList.Add(cSelectCard);
        GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HandCard");
        Card card = Instantiate(toInstantiate, this.transform.Find("Recycle")).GetComponent<Card>();
        card.InitByClone(cSelectCard);
        FindCardAreaListByName("PlayerDropArea").m_AreaList.Add(card);
      }
      else
      {
        cSelectCard.m_IsInBattleGround = false;
        cSelectCard.gameObject.transform.SetParent(this.transform.Find("Recycle"));
        FindCardAreaListByName("PlayerDropArea").m_AreaList.Add(cSelectCard);
      }
      cSelectCard.m_BattleRow = cube.m_Row;
      cSelectCard.m_BattleColumn = cube.m_Column;
      CheckSpecial(cSelectCard, FindCardAreaListByName("PlayerBattleArea"));
      FindCardAreaListByName("PlayerHandArea").m_AreaList.Remove(cSelectCard);
      cSelectCard = null;
      ClearCalculationPre(FindCardAreaListByName("PlayerBattleArea"), FindCardAreaListByName("EnemyBattleArea"));
      enemy.SetCurrentHurt(BattleCalculationPre(FindCardAreaListByName("PlayerBattleArea"), FindCardAreaListByName("EnemyBattleArea")));
      player.SetCurrentHurt(BattleCalculationPre(FindCardAreaListByName("EnemyBattleArea"), FindCardAreaListByName("PlayerBattleArea")));
    }
  }
  void CheckSpecial(Card card,CardArea area)
  {
    if (card.m_CardName == "指挥官")
    {
      card.m_CurrentATK += area.m_AreaList.Count;
      card.m_CurrentHP += area.m_AreaList.Count;
      card.ChangeHP(area.m_AreaList.Count);
      card.ChangeATK(area.m_AreaList.Count);
    }
    foreach (Card forcard in area.m_AreaList)
    {
      if (card.m_CardName == "冲锋" && forcard.m_CardType == Card.CardType.Character && (card.m_BattleRow == forcard.m_BattleRow || card.m_BattleColumn == forcard.m_BattleColumn))
      {
        forcard.m_CurrentATK += 2;
        forcard.ChangeATK(2);
      }
      if (card.m_CardName == "爆发" && forcard.m_CardType == Card.CardType.Character && card.m_BattleColumn == forcard.m_BattleColumn)
      {
        forcard.m_CurrentATK *= 3;
        forcard.m_CurrentHP = 1;
        forcard.ChangeHP(1 - forcard.m_HP);
        forcard.ChangeATK(forcard.m_ATK * 2);
      }
      if (card.m_CardName == "坚守" && forcard.m_CardType == Card.CardType.Character && card.m_BattleRow == forcard.m_BattleRow)
      {
        forcard.m_CurrentHP *= 2;
        forcard.ChangeHP(forcard.m_HP);
      }
      if (card.m_CardName == "巨盾" && forcard.m_CardType == Card.CardType.Character)
      {
        forcard.m_CurrentHP += 5;
        forcard.ChangeHP(5);
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
  }
  public void OnDropButtonClick()
  {
    player.m_CurrentCost = player.m_Cost;
    bDropCard = true;
  }
}
