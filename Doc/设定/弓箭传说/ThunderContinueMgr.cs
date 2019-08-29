using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ThunderContinueMgr
{
    public static ThunderContinueReceive GetThunderContinue(ThunderContinueData data)
    {
        <GetThunderContinue>c__AnonStorey1 storey = new <GetThunderContinue>c__AnonStorey1 {
            data = data,
            receive = new ThunderContinueReceive()
        };
        storey.receive.seqs = new List<Sequence>();
        storey.poslist = new List<Vector3>();
        storey.receive.prev_effects = new List<GameObject>();
        for (int i = 0; i < storey.data.count; i++)
        {
            <GetThunderContinue>c__AnonStorey0 storey2 = new <GetThunderContinue>c__AnonStorey0 {
                <>f__ref$1 = storey,
                index = i
            };
            Sequence item = DOTween.Sequence();
            TweenSettingsExtensions.AppendInterval(item, storey2.index * storey.data.delay);
            TweenSettingsExtensions.AppendCallback(item, new TweenCallback(storey2, this.<>m__0));
            TweenSettingsExtensions.AppendInterval(item, 1.4f);
            TweenSettingsExtensions.AppendCallback(item, new TweenCallback(storey2, this.<>m__1));
            storey.receive.seqs.Add(item);
        }
        return storey.receive;
    }

    [CompilerGenerated]
    private sealed class <GetThunderContinue>c__AnonStorey0
    {
        internal int index;
        internal ThunderContinueMgr.<GetThunderContinue>c__AnonStorey1 <>f__ref$1;

        internal void <>m__0()
        {
            Vector3 item = GameLogic.Release.MapCreatorCtrl.RandomPosition(this.index % 4);
            for (int i = 0; i < 20; i++)
            {
                Vector3 vector2 = item - this.<>f__ref$1.data.entity.position;
                if (vector2.magnitude < 2f)
                {
                    item = GameLogic.Release.MapCreatorCtrl.RandomPosition(this.index % 4);
                }
            }
            this.<>f__ref$1.poslist.Add(item);
            GameObject obj2 = GameLogic.EffectGet("Game/PrevEffect/BulletPrev_1083");
            obj2.transform.position = this.<>f__ref$1.data.entity.position;
            obj2.transform.localScale = Vector3.one * this.<>f__ref$1.data.prev_scale;
            this.<>f__ref$1.receive.prev_effects.Add(obj2);
            ShortcutExtensions.DOMove(obj2.transform, this.<>f__ref$1.poslist[this.index], this.<>f__ref$1.data.delay, false);
        }

        internal void <>m__1()
        {
            GameLogic.Release.Bullet.CreateBullet(this.<>f__ref$1.data.entity, this.<>f__ref$1.data.bulletid, this.<>f__ref$1.poslist[this.index], 0f);
        }
    }

    [CompilerGenerated]
    private sealed class <GetThunderContinue>c__AnonStorey1
    {
        internal ThunderContinueMgr.ThunderContinueData data;
        internal List<Vector3> poslist;
        internal ThunderContinueMgr.ThunderContinueReceive receive;
    }

    public class ThunderContinueData
    {
        public EntityBase entity;
        public int bulletid;
        public int count;
        public float delay;
        public float prev_scale = 1f;
        public string prev_effect = "Game/PrevEffect/BulletPrev_1083";
    }

    public class ThunderContinueReceive
    {
        public List<Sequence> seqs;
        public List<GameObject> prev_effects;

        public void Deinit()
        {
            int num = 0;
            int count = this.seqs.Count;
            while (num < count)
            {
                Sequence sequence = this.seqs[num];
                if (sequence != null)
                {
                    TweenExtensions.Kill(sequence, false);
                }
                num++;
            }
            int num3 = 0;
            int num4 = this.prev_effects.Count;
            while (num3 < num4)
            {
                ShortcutExtensions.DOKill(this.prev_effects[num3].transform, false);
                GameLogic.EffectCache(this.prev_effects[num3]);
                num3++;
            }
            this.prev_effects.Clear();
            this.seqs.Clear();
        }
    }
}

