# 🛠️ Unity编辑器工具使用指南

## 🎯 概述

现在你可以**直接在Unity编辑器中**进行配置表转换，无需运行外部脚本！

---

## 📋 Unity菜单工具

在Unity编辑器顶部菜单栏中，你会看到：

```
Tools
  ├─ 导表 (CSV → JSON)          ⭐ 转换配置表
  ├─ 打开配置文件夹              📁 打开CSV文件夹
  └─ 打开Unity配置文件夹         📁 打开JSON文件夹
```

---

## 🚀 使用方法

### 方法1: Unity编辑器（推荐）⭐

**最简单的方式！**

#### 步骤：

1. **在Excel中编辑配置**
   - 打开 `Config/cards.csv`
   - 打开 `Config/decks.csv`
   - 打开 `Config/gameConfig.csv`
   - 修改数据并保存

2. **在Unity中点击菜单**
   ```
   Unity菜单栏 → Tools → 导表 (CSV → JSON)
   ```

3. **完成！**
   - ✅ 自动转换CSV到JSON
   - ✅ 自动保存到 `Assets/Resources/Config/`
   - ✅ Unity自动刷新资源
   - ✅ 弹窗显示转换结果

**用时：1秒！**

---

### 方法2: 命令行脚本

如果你喜欢命令行：

**Mac/Linux:**
```bash
cd Config
./convert.sh
```

**Windows:**
```bash
cd Config
convert.bat
```

**或者:**
```bash
cd Config
python3 csv_to_json.py
```

---

## 📊 Unity工具详细说明

### 1. 导表 (CSV → JSON) ⭐

**功能：** 将CSV配置文件转换为JSON格式

**转换内容：**
- `Config/cards.csv` → `Assets/Resources/Config/cards.json`
- `Config/decks.csv` → `Assets/Resources/Config/decks.json`
- `Config/gameConfig.csv` → `Assets/Resources/Config/gameConfig.json`

**操作：**
1. Unity菜单栏 → Tools → 导表 (CSV → JSON)
2. 等待转换完成（通常1秒内）
3. 查看弹窗提示

**成功提示：**
```
导表成功

配置表转换完成！

✅ 卡牌: 12 张
✅ 卡组: 2 个
✅ 游戏配置: 已更新

文件已保存到:
Assets/Resources/Config
```

**控制台输出：**
```
=== 开始转换配置表 ===
✅ 卡牌配置转换完成: 12 张卡牌
✅ 卡组配置转换完成: 2 个卡组
✅ 游戏配置转换完成
=================================================
✨ 配置表转换完成！
   📊 卡牌数量: 12
   🎴 卡组数量: 2
   📁 输出目录: Assets/Resources/Config
=================================================
```

---

### 2. 打开配置文件夹 📁

**功能：** 在文件管理器中打开CSV配置文件夹

**路径：** `项目根目录/Config/`

**操作：**
```
Unity菜单栏 → Tools → 打开配置文件夹
```

**包含文件：**
- cards.csv
- decks.csv
- gameConfig.csv
- convert.sh / convert.bat
- 相关文档

---

### 3. 打开Unity配置文件夹 📁

**功能：** 在文件管理器中打开Unity的JSON配置文件夹

**路径：** `Assets/Resources/Config/`

**操作：**
```
Unity菜单栏 → Tools → 打开Unity配置文件夹
```

**包含文件：**
- cards.json
- decks.json
- gameConfig.json

---

## 🎮 完整工作流程

### 场景：添加新卡牌

**传统方式：**
```
1. 打开CSV文件
2. 编辑数据
3. 保存CSV
4. 切换到终端
5. 运行转换脚本
6. 切换回Unity
7. 等待Unity刷新
⏱️ 用时：2-3分钟
```

**现在（Unity工具）：**
```
1. 打开CSV文件
2. 编辑数据
3. 保存CSV
4. Unity菜单 → Tools → 导表
5. 完成！
⏱️ 用时：30秒
```

**效率提升：4倍！**

---

## 📝 使用示例

### 示例1: 调整卡牌平衡性

**需求：** 增强"士兵"卡的属性

**步骤：**

1. **Unity菜单栏 → Tools → 打开配置文件夹**
   - 自动打开 `Config/` 文件夹

2. **用Excel打开 `cards.csv`**
   - 找到 "soldier" 行
   - 修改 hp: `3` → `4`
   - 修改 attack: `3` → `4`
   - 保存文件（Ctrl+S）

3. **Unity菜单栏 → Tools → 导表 (CSV → JSON)**
   - 等待1秒
   - 看到"导表成功"弹窗
   - 点击"确定"

4. **运行游戏测试**
   - 士兵卡的属性已更新！

**用时：1分钟**

---

### 示例2: 添加新卡牌

**需求：** 添加"法师"卡牌

**步骤：**

1. **Unity菜单栏 → Tools → 打开配置文件夹**

2. **编辑 `cards.csv`**（用Excel打开）
   - 滚动到底部
   - 添加新行：
   ```
   mage,法师,法师动画,3,2,5,Character,Normal,player,强力魔法攻击者
   ```
   - 保存

3. **编辑 `decks.csv`**（用Excel打开）
   - 添加新行：
   ```
   playerStarterDeck,圣骑士起始卡组,mage,2,法师
   ```
   - 保存

4. **Unity菜单栏 → Tools → 导表 (CSV → JSON)**

5. **完成！** 新卡牌已添加到游戏

**用时：2分钟**

---

## 🔧 技术细节

### 转换脚本位置

**Unity Editor脚本：**
```
Assets/Editor/ConfigTableConverter.cs
```

**功能特点：**
- ✅ 纯C#实现，无外部依赖
- ✅ 自动检测CSV文件
- ✅ 智能解析CSV格式
- ✅ 自动生成格式化JSON
- ✅ 错误处理和提示
- ✅ Unity资源自动刷新

---

### 转换逻辑

**cards.csv → cards.json:**
```csharp
1. 读取CSV文件（UTF-8编码）
2. 解析每一行为CardConfigData对象
3. 序列化为JSON格式
4. 保存到Assets/Resources/Config/cards.json
```

**decks.csv → decks.json:**
```csharp
1. 读取CSV文件
2. 按deckId分组卡牌
3. 计算每个卡组的总卡牌数
4. 生成嵌套JSON结构
5. 保存到Assets/Resources/Config/decks.json
```

**gameConfig.csv → gameConfig.json:**
```csharp
1. 读取CSV文件
2. 按category分类配置项
3. 自动识别数值类型
4. 添加固定的cardAreaNames
5. 保存到Assets/Resources/Config/gameConfig.json
```

---

## ⚠️ 注意事项

### ✅ 推荐做法

1. **总是在Unity中转换**
   - 使用 Tools → 导表
   - 自动刷新Unity资源

2. **编辑CSV后立即转换**
   - 保持CSV和JSON同步
   - 避免使用过期配置

3. **使用版本控制**
   ```bash
   git add Config/*.csv
   git commit -m "调整卡牌平衡性"
   ```

---

### ❌ 避免做法

1. **不要直接编辑JSON文件**
   - 会被转换工具覆盖
   - 除非你不使用CSV配置

2. **不要在转换时修改CSV**
   - 可能导致数据不一致

3. **不要删除CSV表头**
   - 转换脚本需要表头

---

## 🎯 快捷键（可选）

你可以为常用操作设置快捷键：

**编辑 `ConfigTableConverter.cs`：**

```csharp
// 添加快捷键 Ctrl+Shift+T
[MenuItem("Tools/导表 (CSV → JSON) %#t", false, 1)]
public static void ConvertConfigTables()
{
    // ...
}
```

**快捷键格式：**
- `%` = Ctrl (Windows) / Cmd (Mac)
- `#` = Shift
- `&` = Alt
- `t` = T键

**示例：**
- `%#t` = Ctrl+Shift+T / Cmd+Shift+T
- `%&e` = Ctrl+Alt+E / Cmd+Alt+E

---

## 📊 工作流对比

### Unity工具 vs 命令行脚本

| 特性 | Unity工具 | 命令行脚本 |
|-----|----------|-----------|
| 便捷性 | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ |
| 速度 | 快（1秒） | 快（2秒） |
| 提示 | 弹窗+控制台 | 终端输出 |
| Unity刷新 | 自动 | 自动 |
| 需要切换窗口 | 否 | 是 |
| 推荐度 | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ |

**结论：** Unity工具更方便，推荐使用！

---

## 🐛 故障排查

### 问题1: 菜单中没有Tools选项

**原因：**
- Editor脚本未正确放置
- Unity未编译脚本

**解决：**
1. 确认 `Assets/Editor/ConfigTableConverter.cs` 存在
2. Unity菜单 → Assets → Refresh (Ctrl+R)
3. 等待编译完成
4. 重启Unity编辑器

---

### 问题2: 点击"导表"没反应

**原因：**
- CSV文件不存在
- 路径配置错误

**解决：**
1. 检查 `Config/` 文件夹是否存在
2. 检查CSV文件是否存在
3. 查看Unity Console的错误信息

---

### 问题3: 转换后JSON格式错误

**原因：**
- CSV格式不正确
- 特殊字符未转义

**解决：**
1. 用Excel打开CSV，检查格式
2. 确保逗号正确分隔
3. 特殊字符使用引号包裹
4. 重新保存为UTF-8编码

---

### 问题4: 转换成功但游戏中没生效

**原因：**
- Unity未重新加载配置
- 代码缓存了旧配置

**解决：**
1. Unity菜单 → Assets → Refresh
2. 重新启动游戏场景
3. 清除游戏缓存

---

## 📚 相关文档

### 快速上手
```
Config/QUICKSTART.md - 5分钟快速开始
```

### Excel教程
```
EXCEL_CONFIG_GUIDE.md - 完整Excel配置教程
```

### 配置系统
```
CONFIG_SYSTEM_GUIDE.md - 配置系统技术文档
```

### 完成报告
```
数据表格化完成报告.md - 项目总结
```

---

## 🎉 总结

### Unity工具的优势

✅ **一键转换** - 点击菜单即可
✅ **无需切换** - 无需离开Unity
✅ **实时反馈** - 弹窗显示结果
✅ **自动刷新** - Unity资源自动更新
✅ **错误提示** - 清晰的错误信息
✅ **快捷打开** - 一键打开配置文件夹

### 完整工作流

```
Excel编辑 → Unity导表 → 自动刷新 → 立即测试
   (CSV)     (Tools菜单)    (自动)      (游戏)
```

---

**现在开始使用Unity工具吧！**

**Unity菜单栏 → Tools → 导表 (CSV → JSON)**

**轻松管理游戏配置！🎮✨**
