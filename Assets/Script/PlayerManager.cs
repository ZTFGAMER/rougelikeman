using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

  public Player player;
  public Player enemy;

	// Use this for initialization
	void Start () {
    player.InitType(Player.PlayerType.Player);
    enemy.InitType(Player.PlayerType.Enemy);
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  public void EndTurnClick(){
    player.EndTurn();
  }
}
