using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

  public List<Card> HandCardList = new List<Card>();
  public List<Card> PlayerBattleCardList = new List<Card>();
  public List<Card> EnemyBattleCardList = new List<Card>();

  // Use this for initialization
  void Start () {
    Random rd = new Random();
    for (int i = 1; i <= 10; i++)
    {
      Card card = new Card();
      Card.CardType a = (Card.CardType)Random.Range(0, 4);
      card.InitType(a);
      HandCardList.Add(card);
    }


	}
	
	// Update is called once per frame
	void Update () {
		
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
    HandCardList.Remove(card);
    return true;
  }

  public static void OnButtonClick()
  {

  }
}
