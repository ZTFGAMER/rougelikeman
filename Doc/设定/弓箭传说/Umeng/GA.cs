namespace Umeng
{
    using System;
    using UnityEngine;

    public class GA : Analytics
    {
        public static void Bonus(double coin, BonusSource source)
        {
            object[] args = new object[] { coin, (int) source };
            Analytics.Agent.CallStatic("bonus", args);
        }

        public static void Bonus(string item, int amount, double price, BonusSource source)
        {
            object[] args = new object[] { item, amount, price, (int) source };
            Analytics.Agent.CallStatic("bonus", args);
        }

        public static void Buy(string item, int amount, double price)
        {
            object[] args = new object[] { item, amount, price };
            Analytics.Agent.CallStatic("buy", args);
        }

        public static void FailLevel(string level)
        {
            object[] args = new object[] { level };
            Analytics.Agent.CallStatic("failLevel", args);
        }

        public static void FinishLevel(string level)
        {
            object[] args = new object[] { level };
            Analytics.Agent.CallStatic("finishLevel", args);
        }

        public static void Pay(double cash, int source, double coin)
        {
            if ((source < 1) || (source > 100))
            {
                throw new ArgumentException();
            }
            object[] args = new object[] { cash, coin, source };
            Analytics.Agent.CallStatic("pay", args);
        }

        public static void Pay(double cash, PaySource source, double coin)
        {
            object[] args = new object[] { cash, coin, (int) source };
            Analytics.Agent.CallStatic("pay", args);
        }

        public static void Pay(double cash, PaySource source, string item, int amount, double price)
        {
            object[] args = new object[] { cash, item, amount, price, (int) source };
            Analytics.Agent.CallStatic("pay", args);
        }

        public static void ProfileSignIn(string userId)
        {
            object[] args = new object[] { userId };
            Analytics.Agent.CallStatic("onProfileSignIn", args);
        }

        public static void ProfileSignIn(string userId, string provider)
        {
            object[] args = new object[] { provider, userId };
            Analytics.Agent.CallStatic("onProfileSignIn", args);
        }

        public static void ProfileSignOff()
        {
            Analytics.Agent.CallStatic("onProfileSignOff", Array.Empty<object>());
        }

        [Obsolete("SetUserInfo已弃用, 请使用ProfileSignIn")]
        public static void SetUserInfo(string userId, Gender gender, int age, string platform)
        {
            object[] args = new object[] { userId, age, (int) gender, platform };
            Analytics.Agent.CallStatic("setPlayerInfo", args);
        }

        public static void SetUserLevel(int level)
        {
            object[] args = new object[] { level };
            Analytics.Agent.CallStatic("setPlayerLevel", args);
        }

        [Obsolete("SetUserLevel(string level) 已弃用, 请使用 SetUserLevel(int level)")]
        public static void SetUserLevel(string level)
        {
            Debug.LogWarning("SetUserLevel(string level) 已弃用, 请使用 SetUserLevel(int level)");
        }

        public static void StartLevel(string level)
        {
            object[] args = new object[] { level };
            Analytics.Agent.CallStatic("startLevel", args);
        }

        public static void Use(string item, int amount, double price)
        {
            object[] args = new object[] { item, amount, price };
            Analytics.Agent.CallStatic("use", args);
        }

        public enum BonusSource
        {
            玩家赠送 = 1,
            Source2 = 2,
            Source3 = 3,
            Source4 = 4,
            Source5 = 5,
            Source6 = 6,
            Source7 = 7,
            Source8 = 8,
            Source9 = 9,
            Source10 = 10
        }

        public enum Gender
        {
            Unknown,
            Male,
            Female
        }

        public enum PaySource
        {
            AppStore = 1,
            支付宝 = 2,
            网银 = 3,
            财付通 = 4,
            移动 = 5,
            联通 = 6,
            电信 = 7,
            Paypal = 8,
            Source9 = 9,
            Source10 = 10,
            Source11 = 11,
            Source12 = 12,
            Source13 = 13,
            Source14 = 14,
            Source15 = 15,
            Source16 = 0x10,
            Source17 = 0x11,
            Source18 = 0x12,
            Source19 = 0x13,
            Source20 = 20,
            Source21 = 0x15,
            Source22 = 0x16,
            Source23 = 0x17,
            Source24 = 0x18,
            Source25 = 0x19,
            Source26 = 0x1a,
            Source27 = 0x1b,
            Source28 = 0x1c,
            Source29 = 0x1d,
            Source30 = 30
        }
    }
}

