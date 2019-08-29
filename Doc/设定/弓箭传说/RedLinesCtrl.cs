using System;
using System.Collections.Generic;

public class RedLinesCtrl
{
    private List<RedLineCtrl> list = new List<RedLineCtrl>();

    public void DeInit()
    {
        int num = 0;
        int count = this.list.Count;
        while (num < count)
        {
            this.list[num].DeInit();
            num++;
        }
        this.list.Clear();
    }

    public void Init(EntityBase entity, bool throughwall, int ReboundCount, int count, float perangle)
    {
        this.list.Clear();
        float num = (perangle * (count - 1)) / 2f;
        for (int i = 0; i < count; i++)
        {
            RedLineCtrl item = new RedLineCtrl();
            float offsetangle = num - (perangle * i);
            item.Init(entity, throughwall, ReboundCount, offsetangle);
            this.list.Add(item);
        }
    }

    public void Update()
    {
        int num = 0;
        int count = this.list.Count;
        while (num < count)
        {
            this.list[num].Update();
            num++;
        }
    }
}

