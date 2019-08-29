using System;
using UnityEngine;

public class BodyShaderGold : BodyShaderBase
{
    private static Color mDiffuseColor = new Color(0.9921569f, 0.5803922f, 0f);
    private static Color mOutColor = new Color(0.9607843f, 0.9607843f, 0.9607843f);

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
        if (base.mMaterial != null)
        {
            base.mMaterial.shader = Shader.Find("Custom/OutLight");
            if (base.BodyMeshRenderer != null)
            {
                base.BodyMeshRenderer.material = base.mMaterial;
            }
            else if (base.BodyMeshRenderer2 != null)
            {
                base.BodyMeshRenderer2.material = base.mMaterial;
            }
        }
        base.m_Entity.PlayEffect(0x325aa1);
    }

    protected override void OnReturnToDefault()
    {
        if ((base.BodyMeshRenderer != null) || (base.BodyMeshRenderer2 != null))
        {
            base.mMaterial.SetColor(base.Property_TextureColor, mDiffuseColor);
            base.mMaterial.SetColor(base.Property_RimColor, mOutColor);
            base.mMaterial.SetFloat(base.Property_Brightness, 0.1f);
            if (base.BodyMeshRenderer != null)
            {
                base.BodyMeshRenderer.material = base.mMaterial;
            }
            else if (base.BodyMeshRenderer2 != null)
            {
                base.BodyMeshRenderer2.material = base.mMaterial;
            }
        }
    }
}

