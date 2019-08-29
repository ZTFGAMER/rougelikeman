using AudienceNetwork;
using System;
using UnityEngine;

[RequireComponent(typeof(CanvasRenderer)), RequireComponent(typeof(RectTransform)), RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class AdQuad : MonoBehaviour
{
    public AdManager adManager;
    public bool useIconImage;
    public bool useCoverImage;
    private bool adRendered;

    private void OnGUI()
    {
        NativeAd nativeAd = this.adManager.nativeAd;
        if (((nativeAd != null) && this.adManager.IsAdLoaded()) && !this.adRendered)
        {
            Sprite coverImage = null;
            if (this.useCoverImage)
            {
                coverImage = nativeAd.CoverImage;
            }
            else if (this.useIconImage)
            {
                coverImage = nativeAd.IconImage;
            }
            if (coverImage != null)
            {
                MeshRenderer component = base.GetComponent<MeshRenderer>();
                Renderer renderer2 = base.GetComponent<Renderer>();
                renderer2.enabled = true;
                Texture2D texture = coverImage.texture;
                Material material = new Material(Shader.Find("Sprites/Default")) {
                    color = Color.white
                };
                material.SetTexture("texture", texture);
                component.sharedMaterial = material;
                renderer2.material.mainTexture = texture;
                this.adRendered = true;
            }
        }
    }

    private void Start()
    {
        base.GetComponent<Renderer>().enabled = false;
        this.adRendered = false;
    }
}

