using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleResourceCreator : CInstance<BattleResourceCreator>
{
    public T Get<T>(string path) where T: Component => 
        this.get_gameobject(path).GetComponent<T>();

    private GameObject get_gameobject(string path) => 
        Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(path));

    public Entity3097BaseCtrl Get3097Base(Transform parent = null)
    {
        Entity3097BaseCtrl ctrl = this.Get<Entity3097BaseCtrl>("Effect/Monster/3097_base");
        if (parent != null)
        {
            ctrl.transform.SetParentNormal(parent);
            ctrl.transform.position = new Vector3(ctrl.transform.position.x, 0f, ctrl.transform.position.z);
        }
        return ctrl;
    }

    public GameObject GetFoodEquipEffect_Equip(Transform parent = null)
    {
        GameObject child = this.get_gameobject("Game/Food/effect_equip");
        if (parent != null)
        {
            child.SetParentNormal(parent);
        }
        return child;
    }

    public GameObject GetFoodEquipEffect_EquipExp(Transform parent = null)
    {
        GameObject child = this.get_gameobject("Game/Food/effect_equipexp");
        if (parent != null)
        {
            child.SetParentNormal(parent);
        }
        return child;
    }

    public GameObject GetFootCircle(Transform parent = null)
    {
        GameObject child = this.get_gameobject("Game/Player/FootCircle");
        if (parent != null)
        {
            child.SetParentNormal(parent);
        }
        return child;
    }

    public GameObject GetHead_Pet(Transform parent = null)
    {
        GameObject child = this.get_gameobject("Game/UI/Head_Pet");
        if (parent != null)
        {
            child.SetParentNormal(parent);
        }
        return child;
    }
}

