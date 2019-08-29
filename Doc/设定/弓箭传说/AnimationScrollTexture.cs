using System;
using UnityEngine;

public class AnimationScrollTexture : MonoBehaviour
{
    private Material mRenderMat;
    public float SpeedX = 0.25f;
    public float SpeedY = 0.25f;
    private float offsetX;
    private float offsetY;
    private Vector2 offset = new Vector2();

    private void Awake()
    {
        this.mRenderMat = base.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        this.offsetX += Time.deltaTime * this.SpeedX;
        this.offsetY += Time.deltaTime * this.SpeedY;
        this.offsetX = Mathf.Repeat(this.offsetX, 1f);
        this.offsetY = Mathf.Repeat(this.offsetY, 1f);
        this.offset.Set(this.offsetX, this.offsetY);
        this.mRenderMat.mainTextureOffset = this.offset;
    }
}

