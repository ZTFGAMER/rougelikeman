using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletRedLineCtrls
{
    private float time;
    private float alltime = 0.5f;
    private List<BulletRedLineCtrl> list = new List<BulletRedLineCtrl>();
    private List<float> mindiss = new List<float>();

    public void Deinit()
    {
        Updater.RemoveUpdate("Weapon1066", new Action<float>(this.OnUpdate));
        int num = 0;
        int count = this.list.Count;
        while (num < count)
        {
            Object.Destroy(this.list[num].gameObject);
            num++;
        }
        this.list.Clear();
        this.mindiss.Clear();
    }

    public void Init(EntityBase entity, int count, float[] angles)
    {
        this.Deinit();
        this.time = 0f;
        for (int i = 0; i < count; i++)
        {
            GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/Bullet/Bullet1066_RedLine"));
            child.SetParentNormal(entity.m_Body.transform);
            child.transform.localRotation = Quaternion.Euler(0f, angles[i], 0f);
            BulletRedLineCtrl component = child.GetComponent<BulletRedLineCtrl>();
            component.SetLine(true, 0f);
            Vector3 vector = new Vector3(MathDxx.Sin(angles[i] + 90f), 0f, MathDxx.Cos(angles[i] + 90f)) * 0.5f;
            RayCastManager.CastMinDistance(entity.m_Body.transform.position + vector, entity.eulerAngles.y, false, out float num2);
            RayCastManager.CastMinDistance(entity.m_Body.transform.position - vector, entity.eulerAngles.y, false, out float num3);
            this.mindiss.Add((num2 >= num3) ? num3 : num2);
            this.list.Add(component);
        }
        Updater.AddUpdate("Weapon1066", new Action<float>(this.OnUpdate), false);
    }

    private void OnUpdate(float delta)
    {
        this.time += delta;
        this.time = MathDxx.Clamp(this.time, 0f, this.alltime);
        int num = 0;
        int count = this.list.Count;
        while (num < count)
        {
            this.list[num].SetLine(true, this.mindiss[num] * (this.time / this.alltime));
            num++;
        }
    }
}

