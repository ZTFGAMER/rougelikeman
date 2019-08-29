namespace Dxx.Util
{
    using Dxx.Net;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    public static class Utils
    {
        private static Vector3 GetDirection_dir = new Vector3();
        private static float getAngle_angle;
        private static StringBuilder mStringBudier = new StringBuilder();
        private static StringBuilder mFormatStringBudier = new StringBuilder();
        private static object mFormatLock = new object();
        private static StringBuilder mFormatStringBudierThread = new StringBuilder();
        private static object mFormatThreadLock = new object();
        private static DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1, 0, 0, 0, 0, DateTimeKind.Local));
        private static StringBuilder stringGetSecond3String = new StringBuilder();
        private static StringBuilder stringGetSecond2String = new StringBuilder();
        private static long _startUpTime = 0L;

        public static int Ceil(float value) => 
            ((int) Math.Ceiling((double) value));

        public static void ClearEvents(this object ctrl)
        {
            if (ctrl != null)
            {
                BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;
                EventInfo[] events = ctrl.GetType().GetEvents(bindingAttr);
                if ((events != null) && (events.Length >= 1))
                {
                    for (int i = 0; i < events.Length; i++)
                    {
                        try
                        {
                            EventInfo info = events[i];
                            FieldInfo field = info.DeclaringType.GetField("Event" + info.Name, bindingAttr);
                            if (field != null)
                            {
                                field.SetValue(ctrl, null);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        public static double ConvertDateTimeInt(DateTime time)
        {
            DateTime time2 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1, 0, 0, 0, 0, DateTimeKind.Local));
            TimeSpan span = (TimeSpan) (time - time2);
            return span.TotalSeconds;
        }

        public static DateTime ConvertIntDateTime(double d) => 
            TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)).AddSeconds(d);

        public static string CutString(string str, int maxlength)
        {
            if (str.Length > maxlength)
            {
                object[] args = new object[] { str.Substring(0, maxlength - 3) };
                return FormatString("{0}...", args);
            }
            return str;
        }

        public static long DateTimeToUnixTimestamp(DateTime dateTime, bool milliseconds = true)
        {
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, dateTime.Kind);
            if (milliseconds)
            {
                TimeSpan span = (TimeSpan) (dateTime - time);
                return Convert.ToInt64(span.TotalMilliseconds);
            }
            TimeSpan span2 = (TimeSpan) (dateTime - time);
            return Convert.ToInt64(span2.TotalSeconds);
        }

        public static float ExcuteReboundWall(float angle, Vector3 pos, GameObject o)
        {
            float x = MathDxx.Sin(angle);
            float z = MathDxx.Cos(angle);
            Vector3 direction = new Vector3(x, 0f, z);
            Vector3 origin = pos - direction;
            foreach (RaycastHit hit in Physics.RaycastAll(origin, direction, 2f, ((int) 1) << o.gameObject.layer))
            {
                if (hit.collider.gameObject == o)
                {
                    Vector3 vector3 = hit.point - hit.collider.transform.position;
                    Vector3 size = hit.collider.GetComponent<BoxCollider>().size;
                    float num4 = (size.x * hit.transform.localScale.x) / ((size.z * 1.23f) * hit.transform.localScale.z);
                    if (vector3.z > 0f)
                    {
                        float introduced12 = Mathf.Abs(vector3.x);
                        if (introduced12 <= (vector3.z * num4))
                        {
                            return (180f - angle);
                        }
                    }
                    if (vector3.z < 0f)
                    {
                        float introduced13 = Mathf.Abs(vector3.x);
                        if (introduced13 <= (-vector3.z * num4))
                        {
                            return (180f - angle);
                        }
                    }
                    if (vector3.x > 0f)
                    {
                        float introduced14 = Mathf.Abs(vector3.z);
                        if (introduced14 <= (vector3.x / num4))
                        {
                            return -angle;
                        }
                    }
                    return -angle;
                }
            }
            return 0f;
        }

        private static float ExcuteReboundWall(Vector3 position, float angle, Collider o, float offsetdir)
        {
            float x = MathDxx.Sin(angle);
            float z = MathDxx.Cos(angle);
            Vector3 direction = new Vector3(x, 0f, z);
            RaycastHit[] hitArray = Physics.RaycastAll(position + (offsetdir * direction), direction, 100f, ((int) 1) << o.gameObject.layer);
            float num3 = getAngle(direction);
            for (int i = 0; i < hitArray.Length; i++)
            {
                RaycastHit hit = hitArray[i];
                if (hit.collider.gameObject == o.gameObject)
                {
                    Vector3 vector2 = hit.point - hit.collider.transform.position;
                    Vector3 size = hit.collider.GetComponent<BoxCollider>().size;
                    float num5 = (size.x * hit.transform.localScale.x) / ((size.z * 1.23f) * hit.transform.localScale.z);
                    if (vector2.z > 0f)
                    {
                        float introduced12 = Mathf.Abs(vector2.x);
                        if (introduced12 < (vector2.z * num5))
                        {
                            return (180f - angle);
                        }
                    }
                    if (vector2.z < 0f)
                    {
                        float introduced13 = Mathf.Abs(vector2.x);
                        if (introduced13 < (-vector2.z * num5))
                        {
                            return (180f - angle);
                        }
                    }
                    if (vector2.x > 0f)
                    {
                        float introduced14 = Mathf.Abs(vector2.z);
                        if (introduced14 < (vector2.x / num5))
                        {
                            return -angle;
                        }
                    }
                    return -angle;
                }
            }
            return -angle;
        }

        public static float ExcuteReboundWallRedLine(Transform transform, Collider o) => 
            ExcuteReboundWall(transform.position, transform.eulerAngles.y, o, 0f);

        public static float ExcuteReboundWallSkill(float angle, Vector3 position, SphereCollider s, Collider o) => 
            ExcuteReboundWall(position, angle, o, -3f);

        public static int Floor(float value) => 
            ((int) Math.Floor((double) value));

        public static string FormatString(string format, params object[] args)
        {
            string str;
            object mFormatLock = Utils.mFormatLock;
            lock (mFormatLock)
            {
                try
                {
                    mFormatStringBudier.Clear();
                    mFormatStringBudier.AppendFormat(format, args);
                    str = mFormatStringBudier.ToString();
                }
                catch (Exception exception)
                {
                    SdkManager.Bugly_Report("Utils.FormatString", "mFormatStringBudier try failure!!! string :" + format, exception.StackTrace);
                    str = format;
                }
            }
            return str;
        }

        public static string FormatStringThread(string format, params object[] args)
        {
            string str;
            object mFormatThreadLock = Utils.mFormatThreadLock;
            lock (mFormatThreadLock)
            {
                try
                {
                    mFormatStringBudierThread.Clear();
                    mFormatStringBudierThread.AppendFormat(format, args);
                    str = mFormatStringBudierThread.ToString();
                }
                catch (Exception exception)
                {
                    SdkManager.Bugly_Report("Utils.FormatStringThread", "mFormatStringBudierThread try failure!!! string :" + format, exception.StackTrace);
                    str = format;
                }
            }
            return str;
        }

        public static string GenerateUUID()
        {
            object[] args = new object[] { Guid.NewGuid().ToString("N"), GameLogic.Random(0, 0x186a0) };
            return FormatString("{0}{1:D5}", args);
        }

        public static float getAngle(Vector2 dir) => 
            getAngle(dir.x, dir.y);

        public static float getAngle(Vector3 dir) => 
            getAngle(dir.x, dir.z);

        public static float getAngle(float x, float y)
        {
            getAngle_angle = 90f - (Mathf.Atan2(y, x) * 57.29578f);
            getAngle_angle = (getAngle_angle + 360f) % 360f;
            return GetFloat2(getAngle_angle);
        }

        public static float GetBulletAngle(int current, int count, float allangle)
        {
            if (allangle >= 360f)
            {
                object[] args = new object[] { allangle };
                SdkManager.Bugly_Report("Utils.GetBulletAngle", FormatString("allangle：{0} >= 360f!!!", args));
            }
            float num = allangle / ((float) (count - 1));
            return ((current * num) - (allangle / 2f));
        }

        public static DateTime GetCurrentDataTime() => 
            ConvertIntDateTime((double) GetTimeStamp());

        public static Vector3 GetDirection(float angle)
        {
            GetDirection_dir.x = MathDxx.Sin(angle);
            GetDirection_dir.z = MathDxx.Cos(angle);
            return GetDirection_dir;
        }

        public static float GetFloat1(float f) => 
            (((float) ((int) (f * 10f))) / 10f);

        public static float GetFloat2(float f) => 
            (((float) ((int) (f * 100f))) / 100f);

        public static float GetFloat3(float f) => 
            (((float) ((int) (f * 1000f))) / 1000f);

        public static long GetLocalTime()
        {
            TimeSpan span = (TimeSpan) (DateTime.Now.ToUniversalTime() - new DateTime(0x7b2, 1, 1));
            return Convert.ToInt64(span.TotalSeconds);
        }

        public static string GetSecond2String(int second)
        {
            stringGetSecond2String.Remove(0, stringGetSecond2String.Length);
            stringGetSecond2String.AppendFormat("{0:D2}:{1:D2}", second / 60, second % 60);
            return stringGetSecond2String.ToString();
        }

        public static string GetSecond3String(long second)
        {
            stringGetSecond3String.Remove(0, stringGetSecond3String.Length);
            stringGetSecond3String.AppendFormat("{0:D2}:{1:D2}:{2:D2}", second / 0xe10L, (second % 0xe10L) / 60L, second % 60L);
            return stringGetSecond3String.ToString();
        }

        public static string GetString(params object[] args)
        {
            mStringBudier.Clear();
            int index = 0;
            int length = args.Length;
            while (index < length)
            {
                mStringBudier.Append(args[index]);
                index++;
            }
            return mStringBudier.ToString();
        }

        public static TimeSpan GetTime(long second) => 
            new TimeSpan((second * 0x3e8L) * 0x2710L);

        public static string GetTimeGo(double d)
        {
            DateTime time = ConvertIntDateTime(d);
            TimeSpan span = (TimeSpan) (GetCurrentDataTime() - time);
            if (span.Days >= 0x16d)
            {
                object[] objArray1 = new object[] { span.Days / 0x16d };
                return GameLogic.Hold.Language.GetLanguageByTID("几年前", objArray1);
            }
            if (span.Days >= 30)
            {
                object[] objArray2 = new object[] { span.Days / 30 };
                return GameLogic.Hold.Language.GetLanguageByTID("几月前", objArray2);
            }
            if (span.Days > 0)
            {
                object[] objArray3 = new object[] { span.Days };
                return GameLogic.Hold.Language.GetLanguageByTID("几天前", objArray3);
            }
            if (span.Hours > 0)
            {
                object[] objArray4 = new object[] { span.Hours };
                return GameLogic.Hold.Language.GetLanguageByTID("几小时前", objArray4);
            }
            if (span.Minutes > 0)
            {
                object[] objArray5 = new object[] { span.Minutes };
                return GameLogic.Hold.Language.GetLanguageByTID("几分钟前", objArray5);
            }
            object[] args = new object[] { span.Seconds };
            return GameLogic.Hold.Language.GetLanguageByTID("几秒前", args);
        }

        public static long GetTimeStamp()
        {
            if (NetManager.NetTime == 0L)
            {
                return NetManager.LocalTime;
            }
            return Convert.ToInt64((float) ((NetManager.NetTime + Time.realtimeSinceStartup) - NetManager.unitytime));
        }

        public static string NormalizeTimpstamp0(long timpStamp)
        {
            long ticks = timpStamp * 0x989680L;
            TimeSpan span = new TimeSpan(ticks);
            return dtStart.Add(span).ToString("yyyy-MM-dd");
        }

        public static byte[] strToHexByte(string hexString)
        {
            Debug.Log("strToHexByte start " + hexString);
            hexString = hexString.Replace(" ", string.Empty);
            if ((hexString.Length % 2) != 0)
            {
                hexString = hexString.Insert(hexString.Length - 1, 0.ToString());
            }
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 0x10);
            }
            Debug.Log("strToHexByte " + hexString + " -> " + ToHexString(bytes));
            return bytes;
        }

        public static string ToHexString(byte[] bytes)
        {
            string str = string.Empty;
            if (bytes == null)
            {
                return str;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("X2"));
            }
            return builder.ToString();
        }

        public static byte[] UlongToByte(ulong ul) => 
            new byte[] { ((byte) ((ul >> 0x38) & ((ulong) 0xffL))), ((byte) ((ul >> 0x30) & ((ulong) 0xffL))), ((byte) ((ul >> 40) & ((ulong) 0xffL))), ((byte) ((ul >> 0x20) & ((ulong) 0xffL))), ((byte) ((ul >> 0x18) & ((ulong) 0xffL))), ((byte) ((ul >> 0x10) & ((ulong) 0xffL))), ((byte) ((ul >> 8) & ((ulong) 0xffL))), ((byte) (ul & ((ulong) 0xffL))) };

        public static DateTime UnixTimestampToDateTime(DateTime target, long timestamp)
        {
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, target.Kind);
            return time.AddSeconds((double) timestamp);
        }

        public static Vector3 World2Screen(Vector3 worldpos)
        {
            Vector3 vector = GameNode.m_Camera.WorldToViewportPoint(worldpos);
            return new Vector3(GameLogic.Width * vector.x, GameLogic.Height * vector.y, 0f);
        }

        public static long StartUpTime
        {
            get
            {
                if (NetManager.NetTime > 0L)
                {
                    return NetManager.NetTime;
                }
                if (_startUpTime == 0L)
                {
                    _startUpTime = GetTimeStamp();
                }
                return _startUpTime;
            }
        }

        public static long CurrentTime =>
            (StartUpTime + ((int) Time.realtimeSinceStartup));
    }
}

