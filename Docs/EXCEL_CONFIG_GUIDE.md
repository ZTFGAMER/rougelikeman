# 📊 Excel配置系统完整指南

## 🎉 好消息！不用手动编辑JSON了！

现在你可以用 **Excel** 来配置游戏数据，非常直观方便！

---

## 📁 文件位置

```
rougelikeman/
├── Config/                    ← 配置文件夹
│   ├── cards.csv             ← 用Excel打开编辑卡牌
│   ├── decks.csv             ← 用Excel打开编辑卡组
│   ├── gameConfig.csv        ← 用Excel打开编辑设置
│   ├── convert.sh            ← Mac双击运行
│   ├── convert.bat           ← Windows双击运行
│   ├── QUICKSTART.md         ← 快速开始
│   └── README.md             ← 完整文档
│
└── Assets/Resources/Config/   ← Unity配置目录（自动）
    ├── cards.json            ← 自动生成
    ├── decks.json            ← 自动生成
    └── gameConfig.json       ← 自动生成
```

---

## 🖥️ Excel中的配置界面预览

### 1. cards.csv - 卡牌配置表

**在Excel中看起来像这样：**

| id | name | animationName | cost | hp | attack | cardType | hurtEffect | faction | description |
|----|------|---------------|------|----|----|----------|------------|---------|-------------|
| soldier | 士兵 | 近卫兵 | 1 | 3 | 3 | Character | Normal | player | 基础步兵单位 |
| archer | 弓箭手 | 黄忠(骑马) | 1 | 1 | 4 | Character | Backstab | player | 从后往前攻击的远程单位 |
| shield_guard | 盾手 | 曹仁 | 1 | 5 | 1 | Character | Normal | player | 高生命值的防御单位 |
| striker | 冲击手 | 徐晃 | 2 | 3 | 3 | Character | Penetrate | player | 贯穿攻击该列所有单位 |
| commander | 指挥官 | 夏侯敦(骑马) | 2 | 2 | 2 | Character | Normal | player | 根据场上友军数量增加攻击和生命值 |

**列说明：**
- `id`: 卡牌唯一标识（英文）
- `name`: 显示名称（中文）
- `animationName`: 动画名称
- `cost`: 能量消耗（0-5）
- `hp`: 生命值（0-10）
- `attack`: 攻击力（0-10）
- `cardType`: Character（角色）/ Magic（魔法）
- `hurtEffect`: Normal（普通）/ Backstab（背刺）/ Penetrate（贯穿）
- `faction`: player（玩家）/ enemy（敌人）
- `description`: 卡牌描述

---

### 2. decks.csv - 卡组配置表

**在Excel中看起来像这样：**

| deckId | deckName | cardId | count | description |
|--------|----------|--------|-------|-------------|
| playerStarterDeck | 圣骑士起始卡组 | soldier | 3 | 基础步兵 |
| playerStarterDeck | 圣骑士起始卡组 | archer | 3 | 远程射手 |
| playerStarterDeck | 圣骑士起始卡组 | shield_guard | 3 | 防御单位 |
| playerStarterDeck | 圣骑士起始卡组 | charge | 1 | 魔法-冲锋 |
| enemyStarterDeck | 野蛮人卡组 | barbarian_warrior | 3 | 蛮族战士 |

**列说明：**
- `deckId`: 卡组ID（同一卡组的多行）
- `deckName`: 卡组显示名称
- `cardId`: 卡牌ID（对应cards.csv中的id）
- `count`: 该卡牌的数量
- `description`: 备注说明

---

### 3. gameConfig.csv - 游戏配置表

**在Excel中看起来像这样：**

| category | key | value | description |
|----------|-----|-------|-------------|
| gameConstants | playerInitialHP | 35 | 玩家初始生命值 |
| gameConstants | playerInitialEnergy | 3 | 玩家初始能量 |
| gameConstants | enemyInitialHP | 30 | 敌人初始生命值 |
| gameConstants | drawCardCount | 5 | 每回合抽牌数 |
| playerConfig | name | 圣骑士 | 玩家角色名称 |
| playerConfig | animationName | 曹操(骑马) | 玩家动画名称 |
| enemyConfig | name | 野蛮人 | 敌人角色名称 |

**列说明：**
- `category`: 配置类别
  - `gameConstants`: 游戏常量
  - `playerConfig`: 玩家配置
  - `enemyConfig`: 敌人配置
- `key`: 配置项名称
- `value`: 配置值
- `description`: 说明

---

## 🎯 使用流程

### 步骤1: 打开CSV文件

**Windows:**
```
1. 右键点击 cards.csv
2. 选择 "打开方式"
3. 选择 "Microsoft Excel"
```

**Mac:**
```
1. 双击 cards.csv
2. 用 Numbers 或 Excel 打开
```

**效果：**
- ✅ 看到清晰的表格
- ✅ 可以像操作Excel一样编辑
- ✅ 支持排序、筛选、公式

---

### 步骤2: 编辑数据

**示例：添加新卡牌 "法师"**

1. 打开 `cards.csv`
2. 滚动到底部
3. 添加新行：

| id | name | animationName | cost | hp | attack | cardType | hurtEffect | faction | description |
|----|------|---------------|------|----|----|----------|------------|---------|-------------|
| mage | 法师 | 法师动画 | 3 | 2 | 4 | Character | Normal | player | 强力魔法攻击者 |

4. 保存文件（Ctrl+S / Cmd+S）

5. 打开 `decks.csv`，添加到卡组：

| deckId | deckName | cardId | count | description |
|--------|----------|--------|-------|-------------|
| playerStarterDeck | 圣骑士起始卡组 | mage | 2 | 法师 |

6. 保存文件

---

### 步骤3: 转换为JSON

**最简单方式：**

- **Mac**: 双击 `Config/convert.sh`
- **Windows**: 双击 `Config/convert.bat`

**或者命令行：**
```bash
cd Config
python3 csv_to_json.py
```

**输出：**
```
============================================================
🎮 游戏配置转换工具 - CSV to JSON Converter
============================================================

📖 读取 cards.csv...
✅ 生成 cards.json - 13 张卡牌
📦 复制到 Unity: Assets/Resources/Config/cards.json

✨ 转换完成！
============================================================
```

---

### 步骤4: 在Unity中测试

1. 打开Unity
2. Unity自动检测到文件变化
3. 运行游戏
4. 新卡牌立即生效！✨

---

## 💡 Excel高级技巧

### 1. 使用筛选查看特定卡牌

```
1. 选中表头行
2. 菜单：数据 → 筛选
3. 点击 faction 列的下拉箭头
4. 只勾选 "player" 查看玩家卡牌
```

**效果：** 只显示玩家卡牌，隐藏敌人卡牌

---

### 2. 按消耗排序

```
1. 选中 cost 列
2. 菜单：数据 → 排序
3. 选择 "升序"
```

**效果：** 卡牌按消耗从低到高排列

---

### 3. 使用公式自动生成描述

在 `description` 列使用公式：
```excel
=CONCATENATE(B2, " - 消耗:", D2, " 攻击:", F2, " 生命:", E2)
```

**结果：**
```
士兵 - 消耗:1 攻击:3 生命:3
```

---

### 4. 条件格式高亮

**高亮高消耗卡牌：**
```
1. 选中 cost 列
2. 菜单：开始 → 条件格式
3. 新建规则：大于 3 → 设置红色背景
```

**效果：** 消耗>3的卡牌显示红色

---

### 5. 冻结首行

```
1. 选中第2行
2. 菜单：视图 → 冻结窗格
3. 选择 "冻结首行"
```

**效果：** 滚动时表头始终可见

---

### 6. 数据验证

**限制 cardType 只能输入固定值：**
```
1. 选中 cardType 列
2. 菜单：数据 → 数据验证
3. 允许：列表
4. 来源：Character,Magic,Trap
```

**效果：** 该列只能从下拉菜单选择，防止输入错误

---

## 🎨 Excel模板美化建议

### 给表头添加颜色

```
1. 选中第1行（表头）
2. 设置背景色：深蓝色
3. 设置字体颜色：白色
4. 字体加粗
```

### 交替行颜色

```
1. 选中数据区域
2. 菜单：开始 → 套用表格格式
3. 选择喜欢的样式
```

### 列宽自动调整

```
1. 选中所有列
2. 双击列边界
```

---

## 📊 配置对比：JSON vs CSV

### 以前 (JSON) ❌

```json
{
  "cards": [
    {
      "id": "soldier",
      "name": "士兵",
      "cost": 1,
      "hp": 3,
      "attack": 3
    }
  ]
}
```

**问题：**
- ❌ 难以阅读和编辑
- ❌ 容易出现语法错误（逗号、括号）
- ❌ 无法排序和筛选
- ❌ 无法使用Excel功能

---

### 现在 (CSV + Excel) ✅

| id | name | cost | hp | attack |
|----|------|------|----|----|
| soldier | 士兵 | 1 | 3 | 3 |

**优势：**
- ✅ 清晰直观的表格
- ✅ 在Excel中随意编辑
- ✅ 支持排序、筛选、公式
- ✅ 不会出现语法错误
- ✅ 可以导出为Excel文件

---

## 🔄 完整工作流程

```
┌─────────────────────┐
│ 1. 打开 Excel       │
│    编辑 CSV 文件    │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ 2. 修改数据         │
│    - 添加卡牌       │
│    - 调整数值       │
│    - 修改卡组       │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ 3. 保存 CSV 文件    │
│    (Ctrl+S / Cmd+S) │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ 4. 运行转换脚本     │
│    - Mac: convert.sh│
│    - Win: convert.bat│
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ 5. JSON 自动生成    │
│    并复制到 Unity   │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ 6. Unity 自动重载   │
│    立即生效！       │
└─────────────────────┘
```

---

## 📚 常见使用场景

### 场景1: 游戏平衡调整

**需求：** 降低游戏难度

**操作：**
1. 打开 `gameConfig.csv`
2. 找到 `playerInitialHP` 行
3. 修改 value: `35` → `50`
4. 找到 `enemyInitialHP` 行
5. 修改 value: `30` → `20`
6. 保存并转换
7. 完成！

**用时：** 不到1分钟

---

### 场景2: 批量调整卡牌消耗

**需求：** 所有魔法卡消耗-1

**操作：**
1. 打开 `cards.csv`
2. 启用筛选，筛选 `cardType = Magic`
3. 选中所有魔法卡的 `cost` 列
4. 使用公式：`=cost-1`（或手动修改）
5. 保存并转换
6. 完成！

**用时：** 不到2分钟

---

### 场景3: 创建新卡组

**需求：** 创建"法师卡组"

**操作：**
1. 在 `cards.csv` 中添加法师系列卡牌
2. 在 `decks.csv` 中添加：
```csv
mageDeck,法师卡组,mage,3,法师
mageDeck,法师卡组,fireball,3,火球术
mageDeck,法师卡组,ice_spike,2,冰刺
```
3. 保存并转换
4. 在代码中使用：
```csharp
ConfigManager.Instance.GetDeck("mageDeck");
```
5. 完成！

---

## 🎓 Excel使用教程

### 基础操作

**复制粘贴：**
```
Ctrl+C / Cmd+C - 复制
Ctrl+V / Cmd+V - 粘贴
```

**撤销重做：**
```
Ctrl+Z / Cmd+Z - 撤销
Ctrl+Y / Cmd+Y - 重做
```

**保存：**
```
Ctrl+S / Cmd+S - 保存
```

**查找替换：**
```
Ctrl+F / Cmd+F - 查找
Ctrl+H / Cmd+H - 替换
```

---

### 高级操作

**快速填充：**
```
1. 输入前两个单元格：1, 2
2. 选中这两个单元格
3. 拖动右下角的填充柄
4. 自动填充：3, 4, 5...
```

**批量修改：**
```
1. 选中多个单元格
2. 输入新值
3. Ctrl+Enter - 同时修改所有选中单元格
```

**插入行/列：**
```
右键 → 插入
```

**删除行/列：**
```
右键 → 删除
```

---

## ⚠️ 注意事项

### ✅ 推荐做法

1. **定期备份**
   ```bash
   cp cards.csv cards.csv.backup
   ```

2. **使用版本控制**
   ```bash
   git add Config/*.csv
   git commit -m "调整卡牌平衡性"
   ```

3. **增量修改**
   - 一次只改动几个数值
   - 及时测试效果

4. **添加注释**
   - 在 description 列详细说明

---

### ❌ 避免做法

1. **不要编辑 JSON 文件**
   - ❌ 手动编辑 cards.json
   - ✅ 编辑 cards.csv

2. **不要删除表头**
   - 表头行必须保留

3. **不要使用特殊符号**
   - 避免：逗号`,` 引号`"` 在非必要时
   - 原因：CSV 使用逗号作为分隔符

4. **不要在Excel中添加额外工作表**
   - CSV 只支持单个表格

---

## 🆚 CSV vs XLSX

| 特性 | CSV | XLSX |
|-----|-----|------|
| 文件大小 | 小 | 大 |
| 兼容性 | 好（所有程序） | 中（需要Excel） |
| 格式支持 | 无（纯文本） | 有（颜色、格式） |
| 多表格 | 否 | 是 |
| 公式 | 否 | 是 |
| 版本控制 | 友好 | 不友好 |

**建议：**
- ✅ 用 CSV 存储数据（版本控制友好）
- ✅ 可以在 Excel 中编辑 CSV
- ✅ 如需保留格式，另存为 XLSX 作为本地副本

---

## 🎯 效率对比

### 传统方式（编辑JSON）

```
修改10张卡牌的攻击力：
1. 打开JSON文件
2. 搜索每张卡
3. 修改 "attack": 3 → "attack": 4
4. 检查语法（逗号、括号）
5. 保存
⏱️ 预计用时：10-15分钟
❌ 容易出错
```

### 新方式（Excel+CSV）

```
修改10张卡牌的攻击力：
1. 打开 cards.csv
2. 筛选需要修改的卡
3. 批量修改 attack 列
4. 保存
5. 运行转换脚本
⏱️ 预计用时：2-3分钟
✅ 不会出错
```

**效率提升：5倍！**

---

## 📞 技术支持

### 常见问题

**Q: CSV文件乱码？**
```
A: 用 VSCode 打开 → 另存为 UTF-8 编码
```

**Q: Excel修改后没生效？**
```
A:
1. 确认保存了CSV文件
2. 确认运行了转换脚本
3. 检查Unity是否重新加载
```

**Q: 转换脚本报错？**
```
A:
1. 检查Python 3是否安装
2. 检查CSV格式是否正确
3. 查看错误信息提示
```

---

## 🎉 总结

### 现在你可以：

✅ 用 **Excel** 编辑游戏配置（直观易用）
✅ **一键转换** 为JSON（自动化）
✅ **立即生效** 在Unity中（无缝集成）
✅ **不会出错**（无语法错误）
✅ **效率提升5倍**（批量编辑）

### 不再需要：

❌ 手动编辑JSON
❌ 担心语法错误
❌ 逐个修改数值
❌ 重复性工作

---

**开始使用吧！打开 `Config/QUICKSTART.md` 快速上手！**

**完整文档：`Config/README.md`**
