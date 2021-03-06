﻿namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeAchievement : BaseReferenceHolder
    {
        private const ulong MinusOne = ulong.MaxValue;

        internal NativeAchievement(IntPtr selfPointer) : base(selfPointer)
        {
        }

        internal Achievement AsAchievement()
        {
            Achievement achievement = new Achievement {
                Id = this.Id(),
                Name = this.Name(),
                Description = this.Description()
            };
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            ulong num = this.LastModifiedTime();
            if (num == ulong.MaxValue)
            {
                num = 0L;
            }
            achievement.LastModifiedTime = time.AddMilliseconds((double) num);
            achievement.Points = this.getXP();
            achievement.RevealedImageUrl = this.getRevealedImageUrl();
            achievement.UnlockedImageUrl = this.getUnlockedImageUrl();
            if (this.Type() == Types.AchievementType.INCREMENTAL)
            {
                achievement.IsIncremental = true;
                achievement.CurrentSteps = (int) this.CurrentSteps();
                achievement.TotalSteps = (int) this.TotalSteps();
            }
            achievement.IsRevealed = (this.State() == Types.AchievementState.REVEALED) || (this.State() == Types.AchievementState.UNLOCKED);
            achievement.IsUnlocked = this.State() == Types.AchievementState.UNLOCKED;
            return achievement;
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            Achievement.Achievement_Dispose(selfPointer);
        }

        internal uint CurrentSteps() => 
            Achievement.Achievement_CurrentSteps(base.SelfPtr());

        internal string Description() => 
            PInvokeUtilities.OutParamsToString((out_string, out_size) => Achievement.Achievement_Description(base.SelfPtr(), out_string, out_size));

        internal string getRevealedImageUrl() => 
            PInvokeUtilities.OutParamsToString((out_string, out_size) => Achievement.Achievement_RevealedIconUrl(base.SelfPtr(), out_string, out_size));

        internal string getUnlockedImageUrl() => 
            PInvokeUtilities.OutParamsToString((out_string, out_size) => Achievement.Achievement_UnlockedIconUrl(base.SelfPtr(), out_string, out_size));

        internal ulong getXP() => 
            Achievement.Achievement_XP(base.SelfPtr());

        internal string Id() => 
            PInvokeUtilities.OutParamsToString((out_string, out_size) => Achievement.Achievement_Id(base.SelfPtr(), out_string, out_size));

        internal ulong LastModifiedTime()
        {
            if (Achievement.Achievement_Valid(base.SelfPtr()))
            {
                return Achievement.Achievement_LastModifiedTime(base.SelfPtr());
            }
            return 0L;
        }

        internal string Name() => 
            PInvokeUtilities.OutParamsToString((out_string, out_size) => Achievement.Achievement_Name(base.SelfPtr(), out_string, out_size));

        internal Types.AchievementState State() => 
            Achievement.Achievement_State(base.SelfPtr());

        internal uint TotalSteps() => 
            Achievement.Achievement_TotalSteps(base.SelfPtr());

        internal Types.AchievementType Type() => 
            Achievement.Achievement_Type(base.SelfPtr());
    }
}

