using System;

public class Weapon5074 : Weapon5073
{
    protected override void OnInit()
    {
        if (base.m_Entity.m_Body.BulletList.Count > 0)
        {
            base.effectparent = base.m_Entity.m_Body.BulletList[0].transform;
        }
        base.OnInit();
    }
}

