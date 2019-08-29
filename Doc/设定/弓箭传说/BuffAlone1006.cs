using System;

public class BuffAlone1006 : BuffAloneBase
{
    protected override void OnRemove()
    {
    }

    protected override void OnStart()
    {
    }

    protected override float OnValue(float value)
    {
        if (base.args.Length > 0)
        {
            return (value * base.args[0]);
        }
        return value;
    }
}

