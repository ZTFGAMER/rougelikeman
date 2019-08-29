namespace Dxx.Util
{
    using System;

    public class TimeRepeat
    {
        private float updatetime;
        private float starttime;
        private float delaytime;
        private bool firstdo;
        private Action mCallback;
        private string name;

        public TimeRepeat(string name, float updatetime, Action callback, bool firstdo, float delaytime)
        {
            this.name = name;
            this.Init(updatetime, callback, firstdo, delaytime);
        }

        private void Init(float updatetime, Action callback, bool firstdo, float delaytime)
        {
            this.delaytime = delaytime;
            this.updatetime = updatetime;
            this.mCallback = callback;
            this.firstdo = firstdo;
            if (this.firstdo)
            {
                this.starttime = (Updater.AliveTime + delaytime) - updatetime;
            }
            else
            {
                this.starttime = Updater.AliveTime + delaytime;
            }
            this.Register();
        }

        private void Register()
        {
            object[] args = new object[] { this.name };
            Updater.AddUpdate(Utils.FormatString("TimeRepeat.{0}", args), new Action<float>(this.Update), false);
        }

        public void UnRegister()
        {
            object[] args = new object[] { this.name };
            Updater.RemoveUpdate(Utils.FormatString("TimeRepeat.{0}", args), new Action<float>(this.Update));
        }

        private void Update(float delta)
        {
            if ((Updater.AliveTime - this.starttime) >= this.updatetime)
            {
                this.starttime += this.updatetime;
                if (this.mCallback != null)
                {
                    this.mCallback();
                }
            }
        }
    }
}

