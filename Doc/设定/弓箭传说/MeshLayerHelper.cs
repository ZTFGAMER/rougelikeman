using System;
using UnityEngine;

public class MeshLayerHelper : MonoBehaviour
{
    public string LayerName = "Hit";
    public int order;
    public bool changechild;

    private void Start()
    {
        if (!this.changechild)
        {
            MeshRenderer component = base.GetComponent<MeshRenderer>();
            if (component != null)
            {
                component.sortingLayerName = this.LayerName;
            }
        }
        else
        {
            MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>(true);
            if (componentsInChildren != null)
            {
                int index = 0;
                int length = componentsInChildren.Length;
                while (index < length)
                {
                    if (componentsInChildren[index] != null)
                    {
                        componentsInChildren[index].sortingLayerName = this.LayerName;
                        componentsInChildren[index].sortingOrder = this.order;
                    }
                    index++;
                }
            }
        }
        Object.Destroy(this);
    }
}

