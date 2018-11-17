
using UnityEngine;
using LitJson;
using System.IO;

public class JsonUtils:MonoBehaviour
{
  //用于操作json数据的对象
  JsonData data = new JsonData();
  //路径
  public string filepath;
  //json格式字符串
  public string jsonStr;

  // Use this for initialization
  void Start()
  {
    data["m_PlayerName"] = "圣骑士";
    data["m_HP"] = 40;
    data["m_Cost"] = 3;
    data["m_BattleCardList"] = new JsonData();
    data["m_BattleCardList"]["1"] = "圣教军";
    data["m_BattleCardList"]["2"] = "圣教军";
    data["m_BattleCardList"]["3"] = "圣教军";
    data["m_BattleCardList"]["4"] = "弓箭手";
    data["m_BattleCardList"]["5"] = "弓箭手";
    data["m_BattleCardList"]["6"] = "弓箭手";
    data["m_BattleCardList"]["7"] = "盾牌手";
    data["m_BattleCardList"]["8"] = "盾牌手";
    data["m_BattleCardList"]["9"] = "盾牌手";
    data["m_BattleCardList"]["10"] = "冲锋手";
    data["m_BattleCardList"]["11"] = "冲锋手";
    data["m_BattleCardList"]["12"] = "圣十字军";
    data["m_BattleCardList"]["13"] = "无尽之盾";
    data["m_BattleCardList"]["14"] = "英勇号角";
    data["m_BattleCardList"]["15"] = "牺牲权杖";
    data["m_AnimationManager.AnimationList"] = new JsonData();
    data["m_AnimationManager.AnimationList"]["Normal"] = new JsonData();
    data["m_AnimationManager.AnimationList"]["Normal"]["1"] = "Resource/参军_01";
    data["m_AnimationManager.AnimationList"]["Normal"]["2"] = "Resource/参军_02";
    data["m_AnimationManager.AnimationList"]["Normal"]["3"] = "Resource/参军_03";
    data["m_AnimationManager.AnimationList"]["Normal"]["4"] = "Resource/参军_04";
    data["m_AnimationManager.AnimationList"]["Attack"] = new JsonData();
    data["m_AnimationManager.AnimationList"]["Attack"]["1"] = "Resource/参军_melee_01";
    data["m_AnimationManager.AnimationList"]["Attack"]["2"] = "Resource/参军_melee_01";
    data["m_AnimationManager.AnimationList"]["Attack"]["3"] = "Resource/参军_melee_01";
    data["m_AnimationManager.AnimationList"]["Attack"]["4"] = "Resource/参军_melee_01";
    jsonStr = data.ToJson();
    OnSaveFile("player");

    data.Clear();
    data["m_PlayerName"] = "野蛮人";
    data["m_HP"] = 30;
    data["m_Cost"] = 3;
    data["m_AICardList"] = new JsonData();
    data["m_AICardList"]["1"] = "蛮族勇士";
    data["m_AICardList"]["2"] = "蛮族勇士";
    data["m_AICardList"]["3"] = "蛮族勇士";
    data["m_AICardList"]["4"] = "嗜血打击";
    data["m_AICardList"]["5"] = "牺牲";
    data["m_AnimationManager.AnimationList"] = new JsonData();
    data["m_AnimationManager.AnimationList"]["Normal"] = new JsonData();
    data["m_AnimationManager.AnimationList"]["Normal"]["1"] = "Resource/参军_01";
    data["m_AnimationManager.AnimationList"]["Normal"]["2"] = "Resource/参军_02";
    data["m_AnimationManager.AnimationList"]["Normal"]["3"] = "Resource/参军_03";
    data["m_AnimationManager.AnimationList"]["Normal"]["4"] = "Resource/参军_04";
    data["m_AnimationManager.AnimationList"]["Attack"] = new JsonData();
    data["m_AnimationManager.AnimationList"]["Attack"]["1"] = "Resource/参军_melee_01";
    data["m_AnimationManager.AnimationList"]["Attack"]["2"] = "Resource/参军_melee_01";
    data["m_AnimationManager.AnimationList"]["Attack"]["3"] = "Resource/参军_melee_01";
    data["m_AnimationManager.AnimationList"]["Attack"]["4"] = "Resource/参军_melee_01";
    jsonStr = data.ToJson();
    OnSaveFile("enemy");

    ReadBattleData();


  }

  public void ReadBattleData()
  {
    string player = OnReadFile("player");
  }

  public string OnReadFile(string path)
  {
    //根据path路径读取
    filepath = Application.dataPath + "/" + path + ".json";
    //文件信息操作类
    FileInfo info = new FileInfo(filepath);
    //判断路径是否存在
    if (!info.Exists)
    {
      return "";
    }
    else
    {
      //流读取器
      StreamReader sr = new FileInfo(filepath).OpenText();
      //读取文本内容，直到结束
      string json = sr.ReadToEnd();
      //将json格式的字符串转换成EnemyData对象
      data = JsonMapper.ToObject<JsonData>(json);
      jsonStr = data.ToJson();
      sr.Close();
      return jsonStr;
    }
  }

  public void OnSaveFile(string path)
  {
    //保存文件
    SaveJson(jsonStr, path);

  }
  void SaveJson(string text, string path)
  {
    //流写入器
    StreamWriter sw;
    //根据path路径读取
    filepath = Application.dataPath + "/" + path + ".json";
    //文件信息操作类
    FileInfo info = new FileInfo(filepath);
    //判断路径是否存在
    if (!info.Exists)
    {
      sw = info.CreateText();
    }
    else
    {
      //先删除再创建
      info.Delete();
      sw = info.CreateText();
    }

    sw.WriteLine(text);
    sw.Close();
  }
  // Update is called once per frame
  void Update()
  {

  }
}
