using System;
using UnityEngine;

public class BodyShaderDefault : BodyShaderBase
{
    private float outlineWidth;
    private Color m_oulineColor;
    private bool bTargetColor;
    private static Color TargetColor = new Color(1f, 0f, 0f);
    private const float TargetWidth = 0.1f;

    protected override void OnInit()
    {
        if (base.BodyMeshRenderer != null)
        {
            base.mMaterial = base.BodyMeshRenderer.material;
        }
        else if (base.BodyMeshRenderer2 != null)
        {
            base.mMaterial = base.BodyMeshRenderer2.material;
        }
        if ((base.mMaterial != null) && base.mMaterial.HasProperty("_Factor"))
        {
            this.outlineWidth = base.mMaterial.GetFloat("_Factor");
        }
        if (base.mMaterial != null)
        {
            base.m_outlineShader = base.mMaterial.shader;
        }
        if ((base.mMaterial != null) && base.mMaterial.HasProperty("_OutLineColor"))
        {
            this.m_oulineColor = base.mMaterial.GetColor("_OutLineColor");
        }
    }

    protected override void OnReturnToDefault()
    {
        if (((base.BodyMeshRenderer != null) || (base.BodyMeshRenderer2 != null)) && (base.m_outlineShader != null))
        {
            if (base.m_Entity.IsElite)
            {
                if (base.BodyMeshRenderer != null)
                {
                    base.BodyMeshRenderer.material = base.mMaterial;
                }
                else if (base.BodyMeshRenderer2 != null)
                {
                    base.BodyMeshRenderer2.material = base.mMaterial;
                }
                base.SetLightShader();
            }
            else
            {
                base.mMaterial.shader = base.m_outlineShader;
                if (base.BodyMeshRenderer != null)
                {
                    base.BodyMeshRenderer.sharedMaterial = base.mMaterial;
                    base.BodyMeshRenderer.sharedMaterial.SetColor("_OutLineColor", !this.bTargetColor ? this.m_oulineColor : TargetColor);
                    base.BodyMeshRenderer.sharedMaterial.SetFloat("_Factor", !this.bTargetColor ? this.outlineWidth : 0.1f);
                    if (base.BodyMeshRenderer.sharedMaterial.HasProperty(base.Property_Brightness))
                    {
                        base.BodyMeshRenderer.sharedMaterial.SetFloat(base.Property_Brightness, base.Brightness_valueinit);
                    }
                }
                else if (base.BodyMeshRenderer2 != null)
                {
                    base.BodyMeshRenderer2.sharedMaterial = base.mMaterial;
                    base.BodyMeshRenderer2.sharedMaterial.SetColor("_OutLineColor", !this.bTargetColor ? this.m_oulineColor : TargetColor);
                    base.BodyMeshRenderer2.sharedMaterial.SetFloat("_Factor", !this.bTargetColor ? this.outlineWidth : 0.1f);
                    if (base.BodyMeshRenderer2.sharedMaterial.HasProperty(base.Property_Brightness))
                    {
                        base.BodyMeshRenderer2.sharedMaterial.SetFloat(base.Property_Brightness, base.Brightness_valueinit);
                    }
                }
            }
        }
    }
}

