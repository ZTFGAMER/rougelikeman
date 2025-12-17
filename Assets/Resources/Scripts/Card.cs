using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

  public enum CardType
  {
    Character,
    Magic,
    Trap,
  };
  public enum CardSpace
  {
    PlayerDeck,
    PlayerHand,
    PlayerDrop,
    PlayerGround,
    EnemyGround,
    EnemyPre,
  };
  public enum HurtEffect
  {
    Normal = 1,//常规
    Backstab = 2,//从最后的敌人开始计算伤害
    Puncture = 4,//无视护盾造成伤害
    Penetrate = 8,//贯穿，对所有排上的敌人造成伤害
  };
  public string m_CardName;
  public string m_Description;  // 卡牌描述
  public int m_HP = 0;
  public int m_Cost;
  public int m_ATK = 0;
  public int m_Armor;
  public CardType m_CardType;
  public CardSpace m_CardSpace;
  public HurtEffect m_HurtEffect;

  public bool m_IsSelected;
  public bool m_IsInBattleGround;
  public int m_BattleRow;
  public int m_BattleColumn;
  public bool m_IsBattleDead;
  public float m_DeadAnimTime = 0f;

  public int m_CurrentHP;
  public int m_CurrentCost;
  public int m_CurrentATK;
  public int m_CurrentArmor;
  public int m_CurrentOrder;
  public int m_CurrentHurt = 0;
  public int m_CurrentHurtA = 0;
  public Text m_TextHP;
  public Text m_TextATK;
  public Text m_TextCost;
  public Text m_TextCardName;
  public GameObject m_HPLine;
  public GameObject m_ATKLine;
  public GameObject m_ObjectCardSelect;
  public bool m_IsEnemy;

  public UGUISpriteAnimation animationConfig;

  public BattleManager battleManager;

  // 详情气泡相关 / Detail bubble related
  public GameObject m_BubbleNode;      // 气泡节点
  public Text m_BubbleText;            // 气泡文本框
  private bool isShowingDetail = false;

  // 拖拽相关变量 / Drag-related variables
  private bool isDragging = false;
  private Transform originalParent;
  private Vector3 originalPosition;
  private int originalSiblingIndex;
  private Canvas canvas;
  private BattleCube currentHoveredCube;

  // Use this for initialization
  void Start() {
    canvas = GetComponentInParent<Canvas>();
  }
  public void InitByClone(Card clonecard)
  {
    this.battleManager = clonecard.battleManager;
    m_IsEnemy = clonecard.m_IsEnemy;
    ChangeHPAndATKLine();
    m_CardName = clonecard.m_CardName;
    m_Description = clonecard.m_Description;
    m_HurtEffect = clonecard.m_HurtEffect;
    ChangeHP(clonecard.m_HP);
    ChangeATK(clonecard.m_ATK);
    m_Cost = clonecard.m_Cost;
    m_CurrentHurt = 0;
    m_CurrentHurtA = 0;
    m_CardType = clonecard.m_CardType;
    InitAnimation(clonecard.animationConfig.m_SpiteName);
    PrepareForBattle();
  }
  public void ChangeHPAndATKLine()
  {
    if (m_IsEnemy)
    {
      GameObject tochange = m_HPLine;
      m_HPLine = m_ATKLine;
      m_ATKLine = tochange;
    }
  }
  public void ChangeHP(int delta)
  {
    m_HP = Mathf.Max(0,m_HP + delta);
    if (delta != 0)
    {
      ChangeCardLine(m_HPLine, m_HP, delta,"defence_icon_01");
    }
  }
  public void ChangeATK(int delta)
  {
    m_ATK = Mathf.Max(0, m_ATK + delta);
    if (delta != 0)
    {
      switch (m_HurtEffect)
      {
        case HurtEffect.Normal:
          ChangeCardLine(m_ATKLine, m_ATK, delta, "attack_icon_01");
          break;
        case HurtEffect.Backstab:
          ChangeCardLine(m_ATKLine, m_ATK, delta, "attack_icon_02");
          break;
        case HurtEffect.Penetrate:
          ChangeCardLine(m_ATKLine, m_ATK, delta, "attack_icon_03");
          break;
        default:
          ChangeCardLine(m_ATKLine, m_ATK, delta, "attack_icon_01");
          break;
      }
    }
  }
  void ChangeCardLine(GameObject line, int total, int delta,string image)
  {
    if (delta == 0)
    {
      return;
    }
    else if (delta > 0)
    {
      for (int i = 0; i < delta; i++)
      {
        GameObject toInstantiate = (GameObject)Resources.Load("Prefabs/HPLine");
        GameObject hpline = Instantiate(toInstantiate, line.transform);
        hpline.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + image);
      }
    }
    else
    {
      for (int i = 0; i > delta; i--)
      {
        Destroy(line.transform.GetChild(-i).gameObject);
      }
    }
    UpdateCardLinePosition(line);
  }
  void UpdateCardLinePosition(GameObject line)
  {
    int enemy = 1;
    if (m_IsEnemy)
    {
      enemy = -1;
    }
    for (int i = 0; i < line.transform.childCount; i++)
    {
      if (line.transform.childCount < 4)
      {
        line.transform.GetChild(i).position = new Vector3(line.transform.position.x, line.transform.position.y - 55 * enemy + enemy * (line.transform.childCount - 1 - i) * 40f);
      }
      else
      {
        line.transform.GetChild(i).position = new Vector3(line.transform.position.x, line.transform.position.y + 55 * enemy - enemy * i * 110f / (line.transform.childCount - 1));
      }
    }
  }
  public void InitAnimation(string animname)
  {
    this.animationConfig = this.transform.Find("HandCardAnim").GetComponent<UGUISpriteAnimation>();
    this.animationConfig.m_SpiteName = animname;
    this.animationConfig.InitFrame(animname);
  }
  public void PrepareForBattle()
  {
    m_CurrentHP = m_HP;
    m_CurrentCost = m_Cost;
    m_CurrentATK = m_ATK;
    m_IsSelected = false;
    m_IsInBattleGround = false;
    m_IsBattleDead = false;

    // 默认启用点击（手牌状态）
    SetRaycastTarget(true);

    // 隐藏详情气泡
    HideDetail();
  }
  public void ReadyToBattle()
  {
    if (!m_IsInBattleGround)
      battleManager.SelectHandCard(this);
  }
  public void SetPlace(int row, int column)
  {
    m_BattleColumn = column;
    m_BattleRow = row;
  }
  public bool GetPlace(int row, int column)
  {
    return row == m_BattleRow && column == m_BattleColumn;
  }
  public int GetHurtPre(int hurt)
  {
    if (hurt >= m_CurrentHP - m_CurrentHurt)
    {
      hurt -= m_CurrentHP - m_CurrentHurt;
      m_CurrentHurt = m_CurrentHP;
      return hurt;
    }
    else
    {
      m_CurrentHurt += hurt;
      return 0;
    }
  }
  public void GetHurtAPra(int hurt)
  {
    if (hurt <= m_CurrentATK)
    {
      m_CurrentHurtA = m_CurrentATK - hurt;
    }
    else
    {
      m_CurrentHurtA = 0;
    }
  }
  public int GetHurt(int hurt)
  {
    if (hurt >= m_CurrentHP)
    {
      m_IsBattleDead = true;
      hurt -= m_CurrentHP;
      m_CurrentHP = 0;
      return hurt;
    }
    else
    {
      m_CurrentHP -= hurt;
      return 0;
    }
  }
  public int GetHurtByPuncture(int hurt)
  {
    if (hurt >= m_CurrentHP)
    {
      m_IsBattleDead = true;
      hurt -= m_CurrentHP;
      m_CurrentHP = 0;
      return hurt;
    }
    else
    {
      m_CurrentHP -= hurt;
      return 0;
    }
  }
  public void Tick() {
    ShowCard();
    if (m_IsInBattleGround)
    { 
      UpdateCardLine(m_HPLine, m_HP, m_CurrentHP,m_CurrentHurt,Color.green, Color.gray,Color.yellow);
      UpdateCardLine(m_ATKLine, m_ATK, m_CurrentATK, m_CurrentHurtA, Color.red,Color.gray, Color.yellow);
    }
  }
  void UpdateCardLine(GameObject line, int total, int delta,int hurt,Color totalc,Color deltac,Color hurtc)
  {
    if (total > 0)
    {
      for (int i = 0; i < total; i++)
      {
        if (delta < total && i < total - delta)
        {
          line.transform.GetChild(i).GetComponent<Image>().color = deltac;
        }
        else if (hurt > 0 && i < total - delta + hurt)
        {
          line.transform.GetChild(i).GetComponent<Image>().color = hurtc;
        }
        else
        {
          line.transform.GetChild(i).GetComponent<Image>().color = totalc;
        }
      }
    }
  }
  void ShowCard()
  {
    m_TextCost.text = m_CurrentCost.ToString();
    m_TextCardName.text = m_CardName;
    if (this.gameObject.transform.Find("HandCardAnim/HandCard_Cost"))
    {
      this.gameObject.transform.Find("HandCardAnim/HandCard_Cost").gameObject.SetActive(!m_IsInBattleGround);
    }
    if (this.gameObject.transform.Find("HandCardAnim/HandCard_Name"))
    {
      this.gameObject.transform.Find("HandCardAnim/HandCard_Name").gameObject.SetActive(!m_IsInBattleGround);
    }
    if (this.gameObject.transform.Find("HandCardAnim/HandCard_ATK"))
    {
      this.gameObject.transform.Find("HandCardAnim/HandCard_ATK").gameObject.SetActive(!m_IsInBattleGround);
    }
    if (this.gameObject.transform.Find("HandCardAnim/HandCard_HP"))
    {
      this.gameObject.transform.Find("HandCardAnim/HandCard_HP").gameObject.SetActive(!m_IsInBattleGround);
    }
    if (m_HPLine != null)
    {
      m_HPLine.SetActive(m_IsInBattleGround);
    }
    if (m_ATKLine != null)
    {
      m_ATKLine.SetActive(m_IsInBattleGround);
    }
    if (m_CardType == CardType.Character)
    {
      m_TextATK.text = m_ATK.ToString();
      m_TextHP.text = m_HP.ToString();
    }
    else
    {
      m_TextATK.transform.parent.gameObject.SetActive(false);
      m_TextHP.transform.parent.gameObject.SetActive(false);
    }
    if (m_ObjectCardSelect != null)
    {
      m_ObjectCardSelect.SetActive(m_IsSelected);
    }
  }

  /// <summary>
  /// 设置卡牌是否接收射线检测（用于点击穿透）
  /// Set whether card receives raycast (for click-through)
  /// </summary>
  public void SetRaycastTarget(bool enabled)
  {
    // 禁用/启用所有Image组件的raycastTarget
    Image[] images = GetComponentsInChildren<Image>();
    foreach (Image img in images)
    {
      img.raycastTarget = enabled;
    }

    // 禁用/启用所有Text组件的raycastTarget
    Text[] texts = GetComponentsInChildren<Text>();
    foreach (Text txt in texts)
    {
      txt.raycastTarget = enabled;
    }
  }

  // 用于保存气泡的原始父对象，以便隐藏时恢复
  private Transform bubbleOriginalParent;
  private int bubbleOriginalSiblingIndex;
  private Vector3 bubbleOriginalLocalPosition;

  /// <summary>
  /// 显示卡牌详情
  /// Show card details
  /// </summary>
  public void ShowDetail()
  {
    if (m_BubbleNode == null || m_BubbleText == null) return;

    isShowingDetail = true;

    // 保存原始父对象和本地位置
    if (bubbleOriginalParent == null)
    {
      bubbleOriginalParent = m_BubbleNode.transform.parent;
      bubbleOriginalSiblingIndex = m_BubbleNode.transform.GetSiblingIndex();
      bubbleOriginalLocalPosition = m_BubbleNode.transform.localPosition;
    }

    // 将气泡移到Canvas层级，确保显示在最上层
    if (canvas != null)
    {
      // 保存世界位置
      Vector3 worldPosition = m_BubbleNode.transform.position;

      // 移动到Canvas层级
      m_BubbleNode.transform.SetParent(canvas.transform);
      m_BubbleNode.transform.SetAsLastSibling();

      // 恢复世界位置，保持相对于卡牌的位置
      m_BubbleNode.transform.position = worldPosition;
    }

    m_BubbleNode.SetActive(true);

    // 显示卡牌描述
    m_BubbleText.text = m_Description;
  }

  /// <summary>
  /// 隐藏卡牌详情
  /// Hide card details
  /// </summary>
  public void HideDetail()
  {
    if (m_BubbleNode == null) return;

    isShowingDetail = false;

    // 先恢复气泡到原始父对象和位置，再隐藏
    if (bubbleOriginalParent != null)
    {
      m_BubbleNode.transform.SetParent(bubbleOriginalParent);
      m_BubbleNode.transform.SetSiblingIndex(bubbleOriginalSiblingIndex);
      m_BubbleNode.transform.localPosition = bubbleOriginalLocalPosition;
    }

    // 最后隐藏气泡
    m_BubbleNode.SetActive(false);
  }

  #region 拖拽实现 / Drag Implementation

  /// <summary>
  /// 开始拖拽
  /// Begin drag
  /// </summary>
  public void OnBeginDrag(PointerEventData eventData)
  {
    // 只有在手牌区域且未在战场上的卡牌才能拖拽
    if (m_IsInBattleGround || !battleManager.bPushCard) return;

    isDragging = true;

    // 记录原始状态
    originalParent = transform.parent;
    originalPosition = transform.position;
    originalSiblingIndex = transform.GetSiblingIndex();

    // 移动到Canvas层级以便显示在最上层
    if (canvas != null)
    {
      transform.SetParent(canvas.transform);
      transform.SetAsLastSibling();
    }

    // 取消所有卡牌的选中状态并隐藏气泡
    CardArea playerHand = battleManager.transform.Find("PlayerHandArea").GetComponent<CardArea>();
    if (playerHand != null)
    {
      foreach (Card card in playerHand.m_AreaList)
      {
        card.m_IsSelected = false;
        card.HideDetail();
      }
    }
    battleManager.cSelectCard = null;
  }

  /// <summary>
  /// 拖拽中
  /// During drag
  /// </summary>
  public void OnDrag(PointerEventData eventData)
  {
    if (!isDragging) return;

    // 卡牌跟随鼠标/手指
    transform.position = eventData.position;

    // 检测当前悬停的格子
    BattleCube hoveredCube = GetBattleCubeUnderPointer(eventData);

    // 如果悬停的格子发生变化
    if (hoveredCube != currentHoveredCube)
    {
      // 清除之前格子的高亮
      if (currentHoveredCube != null)
      {
        currentHoveredCube.SetHighlight(false, false);
      }

      currentHoveredCube = hoveredCube;

      // 设置新格子的高亮
      if (currentHoveredCube != null && currentHoveredCube.m_IsPlayer)
      {
        bool canPlace = CanPlaceAtCube(currentHoveredCube);
        currentHoveredCube.SetHighlight(true, canPlace);
      }
    }
  }

  /// <summary>
  /// 结束拖拽
  /// End drag
  /// </summary>
  public void OnEndDrag(PointerEventData eventData)
  {
    if (!isDragging) return;

    isDragging = false;

    // 清除格子高亮
    if (currentHoveredCube != null)
    {
      currentHoveredCube.SetHighlight(false, false);
      currentHoveredCube = null;
    }

    // 尝试放置卡牌
    BattleCube targetCube = GetBattleCubeUnderPointer(eventData);

    if (targetCube != null && targetCube.m_IsPlayer && CanPlaceAtCube(targetCube))
    {
      // 成功放置
      battleManager.SelectBattleCube(targetCube);
    }
    else
    {
      // 放置失败，返回原位置
      transform.SetParent(originalParent);
      transform.position = originalPosition;
      transform.SetSiblingIndex(originalSiblingIndex);
    }

    // 拖动结束后，清除所有详情显示
    ClearAllDetails();
  }

  /// <summary>
  /// 清除所有卡牌的详情显示
  /// Clear all card details
  /// </summary>
  private void ClearAllDetails()
  {
    // 使用BattleManager的全局方法隐藏所有详情
    if (battleManager != null)
    {
      battleManager.HideAllCardDetails();
    }
  }

  /// <summary>
  /// 获取鼠标/手指位置下的BattleCube
  /// Get BattleCube under pointer
  /// </summary>
  private BattleCube GetBattleCubeUnderPointer(PointerEventData eventData)
  {
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventData, results);

    foreach (RaycastResult result in results)
    {
      BattleCube cube = result.gameObject.GetComponent<BattleCube>();
      if (cube != null)
      {
        return cube;
      }
    }

    return null;
  }

  /// <summary>
  /// 检查是否可以在指定格子放置卡牌
  /// Check if card can be placed at specified cube
  /// </summary>
  private bool CanPlaceAtCube(BattleCube cube)
  {
    if (!cube.m_IsPlayer || !battleManager.bPushCard) return false;

    // 检查费用
    if (battleManager.player.m_CurrentCost < m_Cost) return false;

    // 对于角色卡，检查格子是否被占用
    if (m_CardType == CardType.Character)
    {
      CardArea playerBattle = battleManager.transform.Find("PlayerBattleArea").GetComponent<CardArea>();
      if (playerBattle != null)
      {
        foreach (Card card in playerBattle.m_AreaList)
        {
          if (card.m_BattleColumn == cube.m_Column && card.m_BattleRow == cube.m_Row)
          {
            return false;
          }
        }
      }
    }

    return true;
  }

  #endregion
}
