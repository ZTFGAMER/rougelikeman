using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScripts : MonoBehaviour {

  public static GameManagerScripts instance = null;
  public BattleManager battlemanager;
  public static float FPS = 30f;

  private float deltaTime = 0f;
     
  //public JsonUtils jsonutils;
  void Awake()
  {
    if (instance == null)
      instance = this;
    else if (instance != this)
      Destroy(gameObject);
    DontDestroyOnLoad(gameObject);
    
    InitGame();
  }

  void InitGame()
  {
    GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/BattleManager");
    battlemanager = Instantiate(toInstantiate,GameObject.Find("Canvas").transform).GetComponent<BattleManager>();
  }

	// Update is called once per frame
	void Update () {
    if (deltaTime * FPS > 1)
    {
      deltaTime -= 1 / FPS;
      battlemanager.Tick();
    }
    else
    {
      deltaTime += Time.deltaTime;
    }

	}
}
