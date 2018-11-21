using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  public static GameManager instance = null;
  public BattleManager battlemanager;
  public static float FPS = 10f;

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
    battlemanager = Instantiate(toInstantiate,this.transform.Find("Canvas")).GetComponent<BattleManager>();
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
