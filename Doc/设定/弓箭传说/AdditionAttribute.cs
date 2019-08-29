using System;
using System.Collections.Generic;

public class AdditionAttribute
{
    private float pValue;
    private List<float> list;

    public AdditionAttribute()
    {
        this.list = new List<float>();
    }

    public AdditionAttribute(float value)
    {
        this.list = new List<float>();
        this.UpdateAttribute(value);
    }

    private void UpdateAttribute()
    {
        this.pValue = 1f;
        int count = this.list.Count;
        for (int i = 0; i < count; i++)
        {
            this.pValue *= 1f - this.list[i];
        }
        this.pValue = 1f - this.pValue;
    }

    public void UpdateAttribute(float value)
    {
        if (value > 0f)
        {
            this.list.Add(value);
        }
        else
        {
            int count = this.list.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.list[i] == -value)
                {
                    this.list.RemoveAt(i);
                    break;
                }
            }
        }
        this.UpdateAttribute();
    }

    public float Value =>
        this.pValue;
}

