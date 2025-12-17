using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// 配置表转换工具 - Unity编辑器扩展
/// Config Table Converter - Unity Editor Extension
/// </summary>
public class ConfigTableConverter : EditorWindow
{
    private static string configPath = "Config";
    private static string unityConfigPath = "Assets/Resources/Config";

    [MenuItem("Tools/导表 (CSV → JSON)", false, 1)]
    public static void ConvertConfigTables()
    {
        Debug.Log("=== 开始转换配置表 ===");

        bool success = true;
        int totalCards = 0;
        int totalDecks = 0;

        try
        {
            // 获取项目根目录
            string projectPath = Application.dataPath.Replace("/Assets", "");
            string csvDir = Path.Combine(projectPath, configPath);
            string jsonDir = Path.Combine(projectPath, unityConfigPath);

            // 确保输出目录存在
            if (!Directory.Exists(jsonDir))
            {
                Directory.CreateDirectory(jsonDir);
            }

            // 1. 转换 cards.csv
            string cardsCSV = Path.Combine(csvDir, "cards.csv");
            string cardsJSON = Path.Combine(jsonDir, "cards.json");
            if (File.Exists(cardsCSV))
            {
                totalCards = ConvertCardsCSV(cardsCSV, cardsJSON);
                Debug.Log($"✅ 卡牌配置转换完成: {totalCards} 张卡牌");
            }
            else
            {
                Debug.LogWarning($"⚠️ 未找到文件: {cardsCSV}");
                success = false;
            }

            // 2. 转换 decks.csv
            string decksCSV = Path.Combine(csvDir, "decks.csv");
            string decksJSON = Path.Combine(jsonDir, "decks.json");
            if (File.Exists(decksCSV))
            {
                totalDecks = ConvertDecksCSV(decksCSV, decksJSON);
                Debug.Log($"✅ 卡组配置转换完成: {totalDecks} 个卡组");
            }
            else
            {
                Debug.LogWarning($"⚠️ 未找到文件: {decksCSV}");
                success = false;
            }

            // 3. 转换 gameConfig.csv
            string gameConfigCSV = Path.Combine(csvDir, "gameConfig.csv");
            string gameConfigJSON = Path.Combine(jsonDir, "gameConfig.json");
            if (File.Exists(gameConfigCSV))
            {
                ConvertGameConfigCSV(gameConfigCSV, gameConfigJSON);
                Debug.Log($"✅ 游戏配置转换完成");
            }
            else
            {
                Debug.LogWarning($"⚠️ 未找到文件: {gameConfigCSV}");
                success = false;
            }

            // 刷新Unity资源
            AssetDatabase.Refresh();

            if (success)
            {
                Debug.Log("=================================================");
                Debug.Log("✨ 配置表转换完成！");
                Debug.Log($"   📊 卡牌数量: {totalCards}");
                Debug.Log($"   🎴 卡组数量: {totalDecks}");
                Debug.Log($"   📁 输出目录: {unityConfigPath}");
                Debug.Log("=================================================");

                EditorUtility.DisplayDialog(
                    "导表成功",
                    $"配置表转换完成！\n\n" +
                    $"✅ 卡牌: {totalCards} 张\n" +
                    $"✅ 卡组: {totalDecks} 个\n" +
                    $"✅ 游戏配置: 已更新\n\n" +
                    $"文件已保存到:\n{unityConfigPath}",
                    "确定"
                );
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "导表警告",
                    "部分配置文件未找到或转换失败。\n请查看Console了解详情。",
                    "确定"
                );
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"❌ 转换失败: {e.Message}");
            Debug.LogError($"堆栈跟踪: {e.StackTrace}");

            EditorUtility.DisplayDialog(
                "导表失败",
                $"转换过程中发生错误:\n\n{e.Message}\n\n请查看Console了解详情。",
                "确定"
            );
        }
    }

    /// <summary>
    /// 转换 cards.csv 到 cards.json
    /// </summary>
    private static int ConvertCardsCSV(string csvPath, string jsonPath)
    {
        List<CardConfigData> cards = new List<CardConfigData>();

        using (StreamReader reader = new StreamReader(csvPath, Encoding.UTF8))
        {
            // 跳过表头
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] values = ParseCSVLine(line);
                if (values.Length < 10) continue;

                CardConfigData card = new CardConfigData
                {
                    id = values[0],
                    name = values[1],
                    animationName = values[2],
                    cost = ParseInt(values[3]),
                    hp = ParseInt(values[4]),
                    attack = ParseInt(values[5]),
                    cardType = values[6],
                    hurtEffect = values[7],
                    faction = values[8],
                    description = values[9]
                };

                cards.Add(card);
            }
        }

        // 生成JSON
        CardDatabaseJson data = new CardDatabaseJson { cards = cards };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(jsonPath, json, Encoding.UTF8);

        return cards.Count;
    }

    /// <summary>
    /// 转换 decks.csv 到 decks.json
    /// </summary>
    private static int ConvertDecksCSV(string csvPath, string jsonPath)
    {
        Dictionary<string, DeckData> decks = new Dictionary<string, DeckData>();

        using (StreamReader reader = new StreamReader(csvPath, Encoding.UTF8))
        {
            // 跳过表头
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] values = ParseCSVLine(line);
                if (values.Length < 4) continue;

                string deckId = values[0];
                string deckName = values[1];
                string cardId = values[2];
                int count = ParseInt(values[3]);

                if (!decks.ContainsKey(deckId))
                {
                    decks[deckId] = new DeckData
                    {
                        name = deckName,
                        description = $"{deckName}配置",
                        cards = new List<DeckCardEntry>()
                    };
                }

                decks[deckId].cards.Add(new DeckCardEntry
                {
                    cardId = cardId,
                    count = count
                });
            }
        }

        // 计算总卡牌数
        foreach (var deck in decks.Values)
        {
            int total = 0;
            foreach (var card in deck.cards)
            {
                total += card.count;
            }
            deck.totalCards = total;
        }

        // 生成JSON（手动构建以支持嵌套结构）
        StringBuilder json = new StringBuilder();
        json.AppendLine("{");
        json.AppendLine("  \"decks\": {");

        bool first = true;
        foreach (var kvp in decks)
        {
            if (!first) json.AppendLine(",");
            first = false;

            json.AppendLine($"    \"{kvp.Key}\": {{");
            json.AppendLine("      \"cards\": [");

            for (int i = 0; i < kvp.Value.cards.Count; i++)
            {
                var card = kvp.Value.cards[i];
                json.Append($"        {{\"cardId\": \"{card.cardId}\", \"count\": {card.count}}}");
                if (i < kvp.Value.cards.Count - 1)
                    json.AppendLine(",");
                else
                    json.AppendLine();
            }

            json.AppendLine("      ],");
            json.AppendLine($"      \"name\": \"{kvp.Value.name}\",");
            json.AppendLine($"      \"description\": \"{kvp.Value.description}\",");
            json.Append($"      \"totalCards\": {kvp.Value.totalCards}");
            json.AppendLine();
            json.Append("    }");
        }

        json.AppendLine();
        json.AppendLine("  }");
        json.AppendLine("}");

        File.WriteAllText(jsonPath, json.ToString(), Encoding.UTF8);

        return decks.Count;
    }

    /// <summary>
    /// 转换 gameConfig.csv 到 gameConfig.json
    /// </summary>
    private static void ConvertGameConfigCSV(string csvPath, string jsonPath)
    {
        Dictionary<string, Dictionary<string, object>> config = new Dictionary<string, Dictionary<string, object>>
        {
            { "gameConstants", new Dictionary<string, object>() },
            { "playerConfig", new Dictionary<string, object>() },
            { "enemyConfig", new Dictionary<string, object>() }
        };

        using (StreamReader reader = new StreamReader(csvPath, Encoding.UTF8))
        {
            // 跳过表头
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] values = ParseCSVLine(line);
                if (values.Length < 3) continue;

                string category = values[0];
                string key = values[1];
                string valueStr = values[2];

                // 尝试转换为整数
                object value = valueStr;
                if (int.TryParse(valueStr, out int intValue))
                {
                    value = intValue;
                }

                if (config.ContainsKey(category))
                {
                    config[category][key] = value;
                }
            }
        }

        // 生成JSON（手动构建以支持混合类型）
        StringBuilder json = new StringBuilder();
        json.AppendLine("{");

        // gameConstants
        json.AppendLine("  \"gameConstants\": {");
        WriteJsonDict(json, config["gameConstants"], "    ");
        json.AppendLine("  },");

        // playerConfig
        json.AppendLine("  \"playerConfig\": {");
        WriteJsonDict(json, config["playerConfig"], "    ");
        json.AppendLine("  },");

        // enemyConfig
        json.AppendLine("  \"enemyConfig\": {");
        WriteJsonDict(json, config["enemyConfig"], "    ");
        json.AppendLine("  },");

        // cardAreaNames (固定值)
        json.AppendLine("  \"cardAreaNames\": {");
        json.AppendLine("    \"playerHandArea\": \"PlayerHandArea\",");
        json.AppendLine("    \"playerBattleArea\": \"PlayerBattleArea\",");
        json.AppendLine("    \"playerDeckArea\": \"PlayerDeckArea\",");
        json.AppendLine("    \"playerDropArea\": \"PlayerDropArea\",");
        json.AppendLine("    \"enemyHandArea\": \"EnemyHandArea\",");
        json.AppendLine("    \"enemyBattleArea\": \"EnemyBattleArea\",");
        json.AppendLine("    \"enemyDeckArea\": \"EnemyDeckArea\",");
        json.AppendLine("    \"enemyDropArea\": \"EnemyDropArea\"");
        json.AppendLine("  }");

        json.AppendLine("}");

        File.WriteAllText(jsonPath, json.ToString(), Encoding.UTF8);
    }

    /// <summary>
    /// 写入JSON字典
    /// </summary>
    private static void WriteJsonDict(StringBuilder json, Dictionary<string, object> dict, string indent)
    {
        int count = 0;
        foreach (var kvp in dict)
        {
            count++;
            if (kvp.Value is int)
            {
                json.Append($"{indent}\"{kvp.Key}\": {kvp.Value}");
            }
            else
            {
                json.Append($"{indent}\"{kvp.Key}\": \"{kvp.Value}\"");
            }

            if (count < dict.Count)
                json.AppendLine(",");
            else
                json.AppendLine();
        }
    }

    /// <summary>
    /// 解析CSV行（处理逗号和引号）
    /// </summary>
    private static string[] ParseCSVLine(string line)
    {
        List<string> values = new List<string>();
        bool inQuotes = false;
        StringBuilder currentValue = new StringBuilder();

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                values.Add(currentValue.ToString());
                currentValue.Clear();
            }
            else
            {
                currentValue.Append(c);
            }
        }

        values.Add(currentValue.ToString());
        return values.ToArray();
    }

    /// <summary>
    /// 安全解析整数
    /// </summary>
    private static int ParseInt(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return result;
        }
        return 0;
    }

    [MenuItem("Tools/打开配置文件夹", false, 2)]
    public static void OpenConfigFolder()
    {
        string projectPath = Application.dataPath.Replace("/Assets", "");
        string csvDir = Path.Combine(projectPath, configPath);

        if (Directory.Exists(csvDir))
        {
            EditorUtility.RevealInFinder(csvDir);
        }
        else
        {
            EditorUtility.DisplayDialog(
                "文件夹不存在",
                $"配置文件夹不存在:\n{csvDir}",
                "确定"
            );
        }
    }

    [MenuItem("Tools/打开Unity配置文件夹", false, 3)]
    public static void OpenUnityConfigFolder()
    {
        string projectPath = Application.dataPath.Replace("/Assets", "");
        string jsonDir = Path.Combine(projectPath, unityConfigPath);

        if (Directory.Exists(jsonDir))
        {
            EditorUtility.RevealInFinder(jsonDir);
        }
        else
        {
            EditorUtility.DisplayDialog(
                "文件夹不存在",
                $"Unity配置文件夹不存在:\n{jsonDir}",
                "确定"
            );
        }
    }
}
