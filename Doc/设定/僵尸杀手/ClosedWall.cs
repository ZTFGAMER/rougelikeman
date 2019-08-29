using System;
using UnityEngine;

public class ClosedWall : MonoBehaviour
{
    [SerializeField]
    private TextMesh levelText;
    [SerializeField]
    private Transform canvasRect;
    [SerializeField]
    private int levelRequired;
    [SerializeField]
    private bool rotateTextRuntime;
    private MeshRenderer meshRenderer;

    public void Check(int currentLevel)
    {
        if (currentLevel >= this.levelRequired)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    private void Start()
    {
        this.Check(DataLoader.Instance.GetCurrentPlayerLevel());
        if (this.levelText != null)
        {
            this.meshRenderer = this.levelText.GetComponent<MeshRenderer>();
        }
        this.UpdateText();
        if ((this.canvasRect != null) && this.rotateTextRuntime)
        {
            this.canvasRect.rotation = Quaternion.identity;
            this.canvasRect.Rotate(new Vector3(75f, 0f, 0f));
        }
    }

    public void UpdateText()
    {
        if (this.levelText != null)
        {
            this.levelText.text = LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Reach_x_Level_to_Unlock).Replace("x", this.levelRequired.ToString());
            this.levelText.font = LanguageManager.instance.currentLanguage.font;
            this.meshRenderer.material = LanguageManager.instance.currentLanguage.font.material;
        }
    }
}

