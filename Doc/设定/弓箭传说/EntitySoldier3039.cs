using Dxx.Util;
using System;

public class EntitySoldier3039 : EntityMonsterBase
{
    protected override HittedData OnHittedData(HittedData data, bool bulletthrough, float bulletangle)
    {
        if (!bulletthrough)
        {
            float y = base.eulerAngles.y;
            if (((MathDxx.Abs((float) (y - bulletangle)) < 90f) || (MathDxx.Abs((float) ((y - bulletangle) + 360f)) < 90f)) || (MathDxx.Abs((float) ((y - bulletangle) - 360f)) < 90f))
            {
                return data;
            }
            if (base.m_MoveCtrl.GetMoving())
            {
                base.PlayEffect(0x13d621, base.m_Body.SpecialHitMask.transform.position);
                data.type = EHittedType.eDefence;
                data.hitratio = 0.4f;
                data.backtatio = 0.7f;
                return data;
            }
        }
        return data;
    }
}

