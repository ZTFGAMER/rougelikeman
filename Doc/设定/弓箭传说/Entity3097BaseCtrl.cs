using Dxx.Util;
using System;
using UnityEngine;

public class Entity3097BaseCtrl : MonoBehaviour
{
    public MeshRenderer mesh;

    public void SetTexture(string value)
    {
        if (this.mesh != null)
        {
            object[] args = new object[] { value };
            Texture texture = ResourceManager.Load<Texture>(Utils.FormatString("Game/ModelsTexture/{0}", args));
            if (texture != null)
            {
                this.mesh.material.SetTexture("_MainTex", texture);
            }
        }
    }
}

