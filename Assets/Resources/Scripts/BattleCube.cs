using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCube : MonoBehaviour {

  public int m_Row;
  public int m_Column;
  public bool m_IsPlayer = false;
  public BattleManager battlemanager;

  private Image backgroundImage;
  private Color originalColor;
  private bool isHighlighted = false;

  // 高亮颜色 / Highlight colors
  private Color validColor = new Color(0.5f, 1f, 0.5f, 0.3f);   // 淡绿色
  private Color invalidColor = new Color(1f, 0.5f, 0.5f, 0.3f); // 淡红色
  private Color selectedColor = new Color(1f, 1f, 1f, 0.3f);   // 淡白色（选中）

  // Use this for initialization
  void Start () {
    backgroundImage = GetComponent<Image>();
    if (backgroundImage != null)
    {
      originalColor = backgroundImage.color;
    }
  }

  // Update is called once per frame
  void Update () {

	}

  public void PushCard()
  {
    // 优先检查格子上是否有单位
    if (this.transform.childCount > 0)
    {
      // 如果有单位，选中场上单位（无论是否有手牌选中）
      Card card = this.transform.GetChild(0).GetComponent<Card>();
      if (card != null)
      {
        battlemanager.SelectBattleCard(card, this);
      }
    }
    // 如果格子是空的，且有选中的手牌，尝试放置卡牌
    else if (battlemanager.cSelectCard != null)
    {
      battlemanager.SelectBattleCube(this);
    }
  }

  /// <summary>
  /// 设置格子高亮状态
  /// Set cube highlight state
  /// </summary>
  /// <param name="highlight">是否高亮</param>
  /// <param name="isValid">是否可放置（绿色/红色）</param>
  public void SetHighlight(bool highlight, bool isValid)
  {
    if (backgroundImage == null)
    {
      backgroundImage = GetComponent<Image>();
      if (backgroundImage == null) return;
    }

    if (highlight)
    {
      isHighlighted = true;
      backgroundImage.color = isValid ? validColor : invalidColor;
    }
    else
    {
      isHighlighted = false;
      backgroundImage.color = originalColor;
    }
  }

  /// <summary>
  /// 设置选中状态（淡白色）
  /// Set selected state (light white)
  /// </summary>
  public void SetSelected(bool selected)
  {
    if (backgroundImage == null)
    {
      backgroundImage = GetComponent<Image>();
      if (backgroundImage == null) return;
    }

    if (selected)
    {
      backgroundImage.color = selectedColor;
    }
    else
    {
      backgroundImage.color = originalColor;
    }
  }
}
