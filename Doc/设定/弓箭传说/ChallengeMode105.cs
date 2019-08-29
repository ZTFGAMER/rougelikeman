using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;

public class ChallengeMode105 : ChallengeMode102
{
    private int updatetime;
    private int createtime = 4;
    private float percent;
    private SequencePool m_pool;
    private bool bSync = true;
    private int[,] rects;

    private bool check_full(Vector2Int trypos)
    {
        int length = this.rects.GetLength(0);
        int num2 = this.rects.GetLength(1);
        int[,] checks = (int[,]) this.rects.Clone();
        this.excute_checks(checks, trypos);
        for (int i = 0; i < length; i++)
        {
            for (int k = 0; k < num2; k++)
            {
                if (this.rects[i, k] == 1)
                {
                    this.excute_checks(checks, new Vector2Int(i, k));
                }
            }
        }
        for (int j = 0; j < length; j++)
        {
            for (int k = 0; k < num2; k++)
            {
                if (checks[j, k] == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void ClearPool()
    {
        this.m_pool.Clear();
    }

    private void CreateBombOne()
    {
        Vector3 endpos = this.get_random_position();
        BulletBombDodge dodge = GameLogic.Release.Bullet.CreateBullet(null, 0xbda, endpos + new Vector3(0f, 21f, 0f), 0f) as BulletBombDodge;
        dodge.SetEndPos(endpos);
        dodge.SetTarget(null, 1);
        dodge.mBulletTransmit.SetAttack((long) MathDxx.CeilToInt(0.35f * GameLogic.Self.m_EntityData.MaxHP));
    }

    private void CreateBombs()
    {
        this.ClearPool();
        this.rects = (int[,]) GameLogic.Release.MapCreatorCtrl.GetRects().Clone();
        float num = 1f + (this.percent * 3f);
        int num2 = (int) GameLogic.Random((float) (5f * num), (float) (8f * num));
        if (this.bSync)
        {
            for (int i = 0; i < num2; i++)
            {
                this.CreateBombOne();
            }
        }
        else
        {
            Sequence sequence = this.m_pool.Get();
            for (int i = 0; i < num2; i++)
            {
                TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.CreateBombOne));
                TweenSettingsExtensions.AppendInterval(sequence, 0.3f);
            }
        }
    }

    private void debug()
    {
        int length = this.rects.GetLength(0);
        int num2 = this.rects.GetLength(1);
        int[,] checks = (int[,]) this.rects.Clone();
        for (int i = 0; i < length; i++)
        {
            for (int k = 0; k < num2; k++)
            {
                if (this.rects[i, k] == 1)
                {
                    this.excute_checks(checks, new Vector2Int(i, k));
                }
            }
        }
        for (int j = 0; j < length; j++)
        {
            string str2 = j + " : ";
            for (int k = 0; k < num2; k++)
            {
                str2 = str2 + checks[j, k] + " ";
                if (checks[j, k] == 0)
                {
                    return;
                }
            }
        }
    }

    private void excute_checks(int[,] checks, Vector2Int v)
    {
        int length = checks.GetLength(0);
        int num2 = checks.GetLength(1);
        for (int i = v.x; i >= 0; i--)
        {
            if (checks[i, v.y] > 1)
            {
                break;
            }
            checks[i, v.y] = 1;
        }
        for (int j = v.x + 1; j < length; j++)
        {
            if (checks[j, v.y] > 1)
            {
                break;
            }
            checks[j, v.y] = 1;
        }
        for (int k = v.y; k >= 0; k--)
        {
            if (checks[v.x, k] > 1)
            {
                break;
            }
            checks[v.x, k] = 1;
        }
        for (int m = v.y + 1; m < length; m++)
        {
            if (checks[v.x, m] > 1)
            {
                break;
            }
            checks[v.x, m] = 1;
        }
    }

    private Vector2Int get_empty()
    {
        int x = GameLogic.Random(0, this.rects.GetLength(0));
        int y = GameLogic.Random(0, this.rects.GetLength(1));
        while (this.rects[x, y] != 0)
        {
            x = GameLogic.Random(0, this.rects.GetLength(0));
            y = GameLogic.Random(0, this.rects.GetLength(1));
        }
        return new Vector2Int(x, y);
    }

    private Vector3 get_random_position()
    {
        if (!this.bSync)
        {
            return GameLogic.Release.MapCreatorCtrl.RandomPosition();
        }
        Vector2Int trypos = this.get_empty();
        while (this.check_full(trypos))
        {
            trypos = this.get_empty();
        }
        this.rects[trypos.x, trypos.y] = 1;
        return GameLogic.Release.MapCreatorCtrl.GetWorldPosition(trypos);
    }

    protected override void OnDeInit()
    {
        base.OnDeInit();
        this.ClearPool();
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.m_pool = new SequencePool();
    }

    protected override void OnUpdate()
    {
        this.updatetime++;
        if (this.updatetime == this.createtime)
        {
            this.updatetime -= this.createtime;
            this.percent = ((float) (base.alltime - base.currenttime)) / ((float) base.alltime);
            this.CreateBombs();
        }
    }
}

