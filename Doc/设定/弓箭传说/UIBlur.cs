using System;
using UnityEngine;
using UnityEngine.UI;

public class UIBlur : MonoBehaviour
{
    private Material mMat;
    private Image image;
    private bool bStartUpdate;
    private float startBlur;
    private float endBlur = 4f;
    private float blurTime = 0.12f;
    private float startTime;
    private float endTime;
    private int Shader_Size;

    private void Awake()
    {
        this.image = base.GetComponent<Image>();
        this.mMat = this.image.get_material();
        this.Shader_Size = Shader.PropertyToID("_Size");
    }

    private void OnEnable()
    {
        this.bStartUpdate = true;
        this.startTime = Time.time;
        this.endTime = this.startTime + this.blurTime;
    }

    private void Update()
    {
        if (this.bStartUpdate)
        {
            float num = (Time.time - this.startTime) / this.blurTime;
            if (num < 1f)
            {
                this.mMat.SetFloat(this.Shader_Size, (num * (this.endBlur - this.startBlur)) + this.startBlur);
            }
            else
            {
                this.mMat.SetFloat(this.Shader_Size, this.endBlur);
                this.bStartUpdate = false;
            }
        }
    }
}

