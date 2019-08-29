namespace Dxx.Util
{
    using System;
    using UnityEngine;

    public class ModelUtils
    {
        public static GameObject GenerateModel(string bodyPath, string weaponPath)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(bodyPath));
            if (!string.IsNullOrEmpty(weaponPath))
            {
                GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(weaponPath));
                if (child != null)
                {
                    BodyMask component = obj2.GetComponent<BodyMask>();
                    if ((component != null) && (component.LeftWeapon != null))
                    {
                        child.SetParentNormal(component.LeftWeapon);
                        MeshRenderer componentInChildren = child.transform.GetComponentInChildren<MeshRenderer>();
                        if (((componentInChildren != null) && (componentInChildren.material != null)) && componentInChildren.material.HasProperty("_Factor"))
                        {
                            componentInChildren.material.SetFloat("_Factor", 0f);
                        }
                    }
                }
            }
            return obj2;
        }
    }
}

