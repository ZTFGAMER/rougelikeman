using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class BodyShaderBase
{
    protected EntityBase m_Entity;
    protected BodyMask m_Body;
    protected SkinnedMeshRenderer BodyMeshRenderer;
    protected MeshRenderer BodyMeshRenderer2;
    protected Shader m_outlineShader;
    protected Shader m_alphaShader;
    protected Shader m_deadShader;
    protected Material mMaterial;
    protected int Property_Brightness;
    private int Property_EmissionColor;
    protected int Property_RimColor;
    protected int Property_TextureColor;
    protected float Brightness_valueinit;

    public void Init(EntityBase entity)
    {
        this.m_Entity = entity;
        this.m_Body = this.m_Entity.m_Body;
        this.BodyMeshRenderer = this.m_Body.Body.GetComponent<SkinnedMeshRenderer>();
        if (this.BodyMeshRenderer != null)
        {
            this.BodyMeshRenderer.sortingLayerName = "Player";
            this.BodyMeshRenderer.shadowCastingMode = ShadowCastingMode.On;
        }
        else
        {
            this.BodyMeshRenderer2 = this.m_Body.Body.GetComponent<MeshRenderer>();
            if (this.BodyMeshRenderer2 != null)
            {
                this.BodyMeshRenderer2.sortingLayerName = "Player";
                this.BodyMeshRenderer2.shadowCastingMode = ShadowCastingMode.On;
            }
        }
        this.m_alphaShader = Shader.Find("Custom/OutLight");
        this.m_deadShader = Shader.Find("Custom/DeadShader");
        this.Property_Brightness = Shader.PropertyToID("_RimPower");
        this.Property_EmissionColor = Shader.PropertyToID("_EmissionColor");
        this.Property_RimColor = Shader.PropertyToID("_RimColor");
        this.Property_TextureColor = Shader.PropertyToID("_TextureColor");
        this.OnInit();
        if (this.mMaterial != null)
        {
            if (this.m_Entity.IsElite)
            {
                this.mMaterial.shader = this.m_alphaShader;
                this.mMaterial.SetColor(this.Property_RimColor, new Color(0f, 0.2588235f, 1f));
                this.mMaterial.SetFloat(this.Property_Brightness, 1.23f);
                this.SetLightShader();
            }
            if (this.mMaterial.HasProperty(this.Property_Brightness))
            {
                this.Brightness_valueinit = this.mMaterial.GetFloat(this.Property_Brightness);
            }
        }
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnReturnToDefault()
    {
    }

    public void OnUpdateHitted(float value)
    {
        if ((this.BodyMeshRenderer != null) || (this.BodyMeshRenderer2 != null))
        {
            if (this.BodyMeshRenderer != null)
            {
                this.BodyMeshRenderer.material.SetFloat(this.Property_Brightness, value);
            }
            else if (this.BodyMeshRenderer2 != null)
            {
                this.BodyMeshRenderer2.material.SetFloat(this.Property_Brightness, value);
            }
        }
    }

    public void ReturnToDefault()
    {
        this.OnReturnToDefault();
    }

    public void SetElement(Color color)
    {
        if ((this.BodyMeshRenderer != null) || (this.BodyMeshRenderer2 != null))
        {
            if (this.BodyMeshRenderer != null)
            {
                this.BodyMeshRenderer.material.shader = this.m_alphaShader;
                this.BodyMeshRenderer.material.SetFloat(this.Property_Brightness, 1f);
                this.BodyMeshRenderer.material.SetColor(this.Property_RimColor, color);
            }
            else if (this.BodyMeshRenderer2 != null)
            {
                this.BodyMeshRenderer2.material.shader = this.m_alphaShader;
                this.BodyMeshRenderer2.material.SetFloat(this.Property_Brightness, 1f);
                this.BodyMeshRenderer2.material.SetColor(this.Property_RimColor, color);
            }
        }
    }

    public void SetHitted()
    {
        if ((this.BodyMeshRenderer != null) || (this.BodyMeshRenderer2 != null))
        {
            if (this.BodyMeshRenderer != null)
            {
                this.BodyMeshRenderer.material.shader = this.m_alphaShader;
                this.BodyMeshRenderer.material.SetColor(this.Property_RimColor, Color.white);
                this.BodyMeshRenderer.material.SetColor(this.Property_RimColor, Color.white);
            }
            else if (this.BodyMeshRenderer2 != null)
            {
                this.BodyMeshRenderer2.material.shader = this.m_alphaShader;
                this.BodyMeshRenderer2.material.SetColor(this.Property_RimColor, Color.white);
                this.BodyMeshRenderer2.material.SetColor(this.Property_RimColor, Color.white);
            }
        }
    }

    protected void SetLightShader()
    {
        if ((this.BodyMeshRenderer != null) || (this.BodyMeshRenderer2 != null))
        {
            if (this.BodyMeshRenderer != null)
            {
                if (this.BodyMeshRenderer.material.HasProperty(this.Property_RimColor))
                {
                    this.BodyMeshRenderer.material.SetColor(this.Property_RimColor, new Color(0f, 0.2588235f, 1f));
                }
                if (this.BodyMeshRenderer.material.HasProperty(this.Property_Brightness))
                {
                    this.BodyMeshRenderer.material.SetFloat(this.Property_Brightness, 1.23f);
                }
            }
            else if (this.BodyMeshRenderer2 != null)
            {
                if (this.BodyMeshRenderer2.material.HasProperty(this.Property_RimColor))
                {
                    this.BodyMeshRenderer2.material.SetColor(this.Property_RimColor, new Color(0f, 0.2588235f, 1f));
                }
                if (this.BodyMeshRenderer2.material.HasProperty(this.Property_Brightness))
                {
                    this.BodyMeshRenderer2.material.SetFloat(this.Property_Brightness, 1.23f);
                }
            }
            int num = 0;
            int count = this.m_Body.Body_Extra.Count;
            while (num < count)
            {
                SkinnedMeshRenderer renderer = this.m_Body.Body_Extra[num];
                if (renderer != null)
                {
                    renderer.material = this.mMaterial;
                    if (renderer.material.HasProperty(this.Property_RimColor))
                    {
                        renderer.material.SetColor(this.Property_RimColor, new Color(0f, 0.2588235f, 1f));
                    }
                    if (renderer.material.HasProperty(this.Property_Brightness))
                    {
                        renderer.material.SetFloat(this.Property_Brightness, 1.23f);
                    }
                }
                num++;
            }
        }
    }

    public void SetOrder(int order)
    {
        if (this.BodyMeshRenderer != null)
        {
            this.BodyMeshRenderer.sortingOrder = order;
        }
        else if (this.BodyMeshRenderer2 != null)
        {
            this.BodyMeshRenderer2.sortingOrder = order;
        }
    }

    public void SetStrengh()
    {
        if ((this.mMaterial != null) && this.mMaterial.HasProperty("_MainColor"))
        {
            this.mMaterial.SetColor("_MainColor", new Color(0.3882353f, 0.3882353f, 0.3882353f));
        }
    }

    public void SetTexture(string textureid)
    {
        object[] args = new object[] { textureid };
        Texture texture = ResourceManager.Load<Texture>(Utils.FormatString("Game/ModelsTexture/{0}", args));
        if (texture != null)
        {
            if (this.mMaterial != null)
            {
                this.mMaterial.SetTexture("_MainTex", texture);
            }
            if (this.m_Body.Body_Extra != null)
            {
                int num = 0;
                int count = this.m_Body.Body_Extra.Count;
                while (num < count)
                {
                    this.m_Body.Body_Extra[num].material.SetTexture("_MainTex", texture);
                    num++;
                }
            }
        }
    }
}

