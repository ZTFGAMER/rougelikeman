namespace Dxx.Util
{
    using System;

    public sealed class TimerUI : TimerBase<TimerUI>
    {
        protected override void OnChanged()
        {
        }

        protected override void OnUpdate()
        {
        }

        protected override bool InGameUpdate =>
            false;
    }
}

