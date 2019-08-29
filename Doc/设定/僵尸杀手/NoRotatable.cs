using System;
using UnityEngine;

public class NoRotatable : MonoBehaviour
{
    [SerializeField]
    private bool x;
    [SerializeField]
    private bool y = true;
    [SerializeField]
    private bool z;

    private void Update()
    {
        Vector3 localEulerAngles = base.transform.localEulerAngles;
        if (this.x)
        {
            localEulerAngles.x = -base.transform.parent.eulerAngles.x;
        }
        if (this.y)
        {
            localEulerAngles.y = -base.transform.parent.eulerAngles.y;
        }
        if (this.z)
        {
            localEulerAngles.z = -base.transform.parent.eulerAngles.z;
        }
        base.transform.localEulerAngles = localEulerAngles;
    }
}

