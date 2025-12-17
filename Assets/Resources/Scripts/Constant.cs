using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏常量类 - 已迁移到配置文件
/// Game constants - Migrated to config files
/// 注意：这些值现在从 gameConfig.json 读取
/// Note: These values are now loaded from gameConfig.json
/// 这里的值仅作为默认值/回退值保留
/// Values here are kept as defaults/fallbacks only
/// </summary>
public static class Constant  {

  public static int PLAYER_HP = 35;
  public static int PLAYER_ENERGY = 3;
  public static int ENEMY_HP = 30;

}
