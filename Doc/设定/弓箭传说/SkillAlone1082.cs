using DG.Tweening;
using Dxx.Util;
using System;

public class SkillAlone1082 : SkillAloneBase
{
    private float time;
    private int buffid;
    private SequencePool mSeqPool = new SequencePool();

    protected override void OnInstall()
    {
        if (base.m_SkillData.Args.Length != 2)
        {
            object[] args = new object[] { base.m_SkillData.SkillID, base.m_SkillData.Args.Length };
            SdkManager.Bugly_Report("SkillAlone1082", Utils.FormatString("SkillID:{0} args.length:{1} != 2", args));
        }
        else if (!float.TryParse(base.m_SkillData.Args[0], out this.time))
        {
            object[] args = new object[] { base.m_SkillData.SkillID };
            SdkManager.Bugly_Report("SkillAlone1081", Utils.FormatString("SkillID:{0} args[0] is not a float type.", args));
        }
        else if (!int.TryParse(base.m_SkillData.Args[1], out this.buffid))
        {
            object[] args = new object[] { base.m_SkillData.SkillID };
            SdkManager.Bugly_Report("SkillAlone1081", Utils.FormatString("SkillID:{0} args[1] is not a int type.", args));
        }
        else
        {
            TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.mSeqPool.Get(), this.time), new TweenCallback(this, this.<OnInstall>m__0)), -1);
        }
    }

    protected override void OnUninstall()
    {
        this.mSeqPool.Clear();
    }
}

