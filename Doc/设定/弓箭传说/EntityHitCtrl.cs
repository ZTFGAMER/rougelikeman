using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityHitCtrl
{
    public EntityBase m_Entity;
    public BoxCollider m_BoxCollider;
    public SphereCollider m_SphereCollider;
    public CapsuleCollider m_CapsuleCollider;
    private bool bEnable = true;
    private Vector3 box_scale;
    private float sphere_scale;
    private float capsule_scale;
    private List<float> scales = new List<float>();
    protected Dictionary<string, BoxCollider> m_ChildsBoxCollider = new Dictionary<string, BoxCollider>();
    protected Dictionary<string, SphereCollider> m_ChildsSphereCollider = new Dictionary<string, SphereCollider>();
    protected Dictionary<string, CapsuleCollider> m_ChildsCapsuleCollider = new Dictionary<string, CapsuleCollider>();
    protected const string Entity2MapOutWall = "Entity2MapOutWall";
    protected const string Entity2Stone = "Entity2Stone";
    protected const string Entity2Water = "Entity2Water";
    public Action<Collider> Event_TriggerEnter;
    public Action<Collider> Event_TriggerExit;
    public Action<Collision> Event_CollisionEnter;
    private int triggerCount;

    private float addscale(float scale)
    {
        this.scales.Add(scale);
        float num = 1f;
        int num2 = 0;
        int count = this.scales.Count;
        while (num2 < count)
        {
            num *= this.scales[num2];
            num2++;
        }
        return num;
    }

    private void CreateCollider(string name, int layer)
    {
        GameObject obj2 = new GameObject(name);
        obj2.transform.SetParent(this.m_Entity.transform);
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localScale = Vector3.one;
        obj2.transform.localRotation = Quaternion.identity;
        obj2.layer = layer;
        if (this.m_BoxCollider != null)
        {
            BoxCollider collider = obj2.AddComponent<BoxCollider>();
            collider.center = new Vector3(this.m_BoxCollider.center.x, 0f, this.m_BoxCollider.center.z);
            collider.size = this.m_BoxCollider.size;
            this.m_ChildsBoxCollider.Add(name, collider);
        }
        else if (this.m_SphereCollider != null)
        {
            SphereCollider collider2 = obj2.AddComponent<SphereCollider>();
            collider2.center = new Vector3(this.m_SphereCollider.center.x, 0f, this.m_SphereCollider.center.z);
            collider2.radius = this.m_SphereCollider.radius;
            this.m_ChildsSphereCollider.Add(name, collider2);
        }
        else if (this.m_CapsuleCollider != null)
        {
            CapsuleCollider collider3 = obj2.AddComponent<CapsuleCollider>();
            collider3.center = new Vector3(this.m_CapsuleCollider.center.x, 0f, this.m_CapsuleCollider.center.z);
            collider3.radius = this.m_CapsuleCollider.radius;
            collider3.height = this.m_CapsuleCollider.height;
            collider3.direction = this.m_CapsuleCollider.direction;
            this.m_ChildsCapsuleCollider.Add(name, collider3);
        }
    }

    public bool GetColliderEnable() => 
        this.bEnable;

    public float GetColliderHeight() => 
        this.m_CapsuleCollider.height;

    public float GetCollidersSize() => 
        this.m_CapsuleCollider.radius;

    public bool GetColliderTrigger() => 
        ((this.m_CapsuleCollider != null) && this.m_CapsuleCollider.isTrigger);

    public bool GetTrigger()
    {
        if (this.m_BoxCollider != null)
        {
            return this.m_BoxCollider.isTrigger;
        }
        if (this.m_CapsuleCollider != null)
        {
            return this.m_CapsuleCollider.isTrigger;
        }
        if (this.m_SphereCollider != null)
        {
            return this.m_SphereCollider.isTrigger;
        }
        return true;
    }

    public void Init(EntityBase entity)
    {
        this.m_Entity = entity;
        this.m_BoxCollider = entity.GetComponent<BoxCollider>();
        this.m_SphereCollider = entity.GetComponent<SphereCollider>();
        this.m_CapsuleCollider = entity.GetComponent<CapsuleCollider>();
        if (this.m_BoxCollider != null)
        {
            this.box_scale = this.m_BoxCollider.size;
        }
        if (this.m_SphereCollider != null)
        {
            this.sphere_scale = this.m_SphereCollider.radius;
        }
        if (this.m_CapsuleCollider != null)
        {
            this.capsule_scale = this.m_CapsuleCollider.radius;
        }
        this.InitCollider();
    }

    private void InitCollider()
    {
        this.CreateCollider("Entity2MapOutWall", LayerManager.Entity2MapOutWall);
        this.CreateCollider("Entity2Stone", LayerManager.Entity2Stone);
        this.CreateCollider("Entity2Water", LayerManager.Entity2Water);
    }

    private void OnCollisionEnter(Collision o)
    {
        if (this.Event_CollisionEnter != null)
        {
            this.Event_CollisionEnter(o);
        }
    }

    private void OnTriggerEnter(Collider o)
    {
        if (this.Event_TriggerEnter != null)
        {
            this.Event_TriggerEnter(o);
        }
    }

    private void OnTriggerExit(Collider o)
    {
        if (this.Event_TriggerExit != null)
        {
            this.Event_TriggerExit(o);
        }
    }

    public void RemoveColliders()
    {
        Dictionary<string, BoxCollider>.Enumerator enumerator = this.m_ChildsBoxCollider.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, BoxCollider> current = enumerator.Current;
            if (current.Value != null)
            {
                Object.Destroy(current.Value.gameObject);
            }
        }
        this.m_ChildsBoxCollider.Clear();
        Dictionary<string, SphereCollider>.Enumerator enumerator2 = this.m_ChildsSphereCollider.GetEnumerator();
        while (enumerator2.MoveNext())
        {
            KeyValuePair<string, SphereCollider> current = enumerator2.Current;
            if (current.Value != null)
            {
                Object.Destroy(current.Value.gameObject);
            }
        }
        this.m_ChildsSphereCollider.Clear();
        Dictionary<string, CapsuleCollider>.Enumerator enumerator3 = this.m_ChildsCapsuleCollider.GetEnumerator();
        while (enumerator3.MoveNext())
        {
            KeyValuePair<string, CapsuleCollider> current = enumerator3.Current;
            if (current.Value != null)
            {
                Object.Destroy(current.Value.gameObject);
            }
        }
        this.m_ChildsCapsuleCollider.Clear();
    }

    public void SetBodyScale(float scale)
    {
        float num = this.addscale(scale);
        if (this.m_BoxCollider != null)
        {
            this.m_BoxCollider.size = this.box_scale * num;
        }
        if (this.m_SphereCollider != null)
        {
            this.m_SphereCollider.radius = this.sphere_scale * num;
        }
        if (this.m_CapsuleCollider != null)
        {
            this.m_CapsuleCollider.radius = this.capsule_scale * num;
        }
    }

    public void SetCollider(bool enable)
    {
        this.bEnable = enable;
        if (this.m_BoxCollider != null)
        {
            this.m_BoxCollider.enabled = enable;
        }
        if (this.m_CapsuleCollider != null)
        {
            this.m_CapsuleCollider.enabled = enable;
        }
        if (this.m_SphereCollider != null)
        {
            this.m_SphereCollider.enabled = enable;
        }
        Dictionary<string, BoxCollider>.Enumerator enumerator = this.m_ChildsBoxCollider.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, BoxCollider> current = enumerator.Current;
            if (current.Value != null)
            {
                KeyValuePair<string, BoxCollider> pair2 = enumerator.Current;
                pair2.Value.enabled = enable;
            }
        }
        Dictionary<string, SphereCollider>.Enumerator enumerator2 = this.m_ChildsSphereCollider.GetEnumerator();
        while (enumerator2.MoveNext())
        {
            KeyValuePair<string, SphereCollider> current = enumerator2.Current;
            if (current.Value != null)
            {
                KeyValuePair<string, SphereCollider> pair4 = enumerator2.Current;
                pair4.Value.enabled = enable;
            }
        }
        Dictionary<string, CapsuleCollider>.Enumerator enumerator3 = this.m_ChildsCapsuleCollider.GetEnumerator();
        while (enumerator3.MoveNext())
        {
            KeyValuePair<string, CapsuleCollider> current = enumerator3.Current;
            if (current.Value != null)
            {
                KeyValuePair<string, CapsuleCollider> pair6 = enumerator3.Current;
                pair6.Value.enabled = enable;
            }
        }
    }

    public void SetCollidersScale(float scale)
    {
        float num = this.addscale(scale);
        if (this.m_BoxCollider != null)
        {
            this.m_BoxCollider.size = this.box_scale * num;
        }
        if (this.m_CapsuleCollider != null)
        {
            this.m_CapsuleCollider.radius = this.capsule_scale * num;
        }
        if (this.m_SphereCollider != null)
        {
            this.m_SphereCollider.radius = this.sphere_scale * num;
        }
        Dictionary<string, BoxCollider>.Enumerator enumerator = this.m_ChildsBoxCollider.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, BoxCollider> current = enumerator.Current;
            if (current.Value != null)
            {
                KeyValuePair<string, BoxCollider> pair2 = enumerator.Current;
                pair2.Value.transform.localScale = Vector3.one * scale;
            }
        }
        Dictionary<string, SphereCollider>.Enumerator enumerator2 = this.m_ChildsSphereCollider.GetEnumerator();
        while (enumerator2.MoveNext())
        {
            KeyValuePair<string, SphereCollider> current = enumerator2.Current;
            if (current.Value != null)
            {
                KeyValuePair<string, SphereCollider> pair4 = enumerator2.Current;
                pair4.Value.transform.localScale = Vector3.one * scale;
            }
        }
        Dictionary<string, CapsuleCollider>.Enumerator enumerator3 = this.m_ChildsCapsuleCollider.GetEnumerator();
        while (enumerator3.MoveNext())
        {
            KeyValuePair<string, CapsuleCollider> current = enumerator3.Current;
            if (current.Value != null)
            {
                KeyValuePair<string, CapsuleCollider> pair6 = enumerator3.Current;
                pair6.Value.transform.localScale = Vector3.one * scale;
            }
        }
    }

    public void SetFlyOne(string layer, bool fly)
    {
        if (this.m_ChildsBoxCollider.TryGetValue(layer, out BoxCollider collider) && (collider != null))
        {
            collider.enabled = !fly;
        }
        if (this.m_ChildsSphereCollider.TryGetValue(layer, out SphereCollider collider2) && (collider2 != null))
        {
            collider2.enabled = !fly;
        }
        if (this.m_ChildsCapsuleCollider.TryGetValue(layer, out CapsuleCollider collider3) && (collider3 != null))
        {
            collider3.enabled = !fly;
        }
    }

    public void SetTrigger(bool value)
    {
        this.triggerCount += !value ? -1 : 1;
        bool flag = this.triggerCount > 0;
        if (this.m_BoxCollider != null)
        {
            this.m_BoxCollider.isTrigger = flag;
        }
        if (this.m_CapsuleCollider != null)
        {
            this.m_CapsuleCollider.isTrigger = flag;
        }
        if (this.m_SphereCollider != null)
        {
            this.m_SphereCollider.isTrigger = flag;
        }
    }
}

