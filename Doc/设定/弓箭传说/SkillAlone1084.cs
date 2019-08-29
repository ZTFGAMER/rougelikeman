using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;

public class SkillAlone1084 : SkillAloneBase
{
    private float time;
    private string att;
    private bool bAddAttribute;
    private Goods_goods.GoodData mAdd;
    private Goods_goods.GoodData mRemove;
    private SequencePool mSeqPool = new SequencePool();

    private void OnGotoNextRoom(RoomGenerateBase.Room room)
    {
        this.update_attribute(true);
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.mSeqPool.Get(), this.time), new TweenCallback(this, this.<OnGotoNextRoom>m__0));
    }

    protected override void OnInstall()
    {
        if (base.m_SkillData.Args.Length != 2)
        {
            object[] args = new object[] { base.m_SkillData.SkillID, base.m_SkillData.Args.Length };
            SdkManager.Bugly_Report("SkillAlone1084", Utils.FormatString("SkillID:{0} args.length:{1} != 2", args));
        }
        else if (!float.TryParse(base.m_SkillData.Args[0], out this.time))
        {
            object[] args = new object[] { base.m_SkillData.SkillID };
            SdkManager.Bugly_Report("SkillAlone1084", Utils.FormatString("SkillID:{0} args[0] is not a float type.", args));
        }
        else if (string.IsNullOrEmpty(base.m_SkillData.Args[1]))
        {
            object[] args = new object[] { base.m_SkillData.SkillID };
            SdkManager.Bugly_Report("SkillAlone1084", Utils.FormatString("SkillID:{0} args[1] is null.", args));
        }
        else
        {
            this.att = base.m_SkillData.Args[1];
            this.mAdd = Goods_goods.GetGoodData(this.att);
            this.mRemove = Goods_goods.GetGoodData(this.att);
            this.mRemove.value *= -1L;
            ReleaseModeManager mode = GameLogic.Release.Mode;
            mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        }
    }

    protected override void OnUninstall()
    {
        this.mSeqPool.Clear();
        this.update_attribute(false);
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
    }

    private void update_attribute(bool value)
    {
        if (value)
        {
            if (!this.bAddAttribute)
            {
                this.bAddAttribute = true;
                base.m_Entity.m_EntityData.ExcuteAttributes(this.mAdd);
            }
        }
        else if (this.bAddAttribute)
        {
            this.bAddAttribute = false;
            base.m_Entity.m_EntityData.ExcuteAttributes(this.mRemove);
        }
    }
}

