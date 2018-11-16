using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

  public BattleManager battleManager;
  //public JsonUtils jsonutils;

	// Use this for initialization
	void Start () {
    battleManager.gameLogic = this;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
