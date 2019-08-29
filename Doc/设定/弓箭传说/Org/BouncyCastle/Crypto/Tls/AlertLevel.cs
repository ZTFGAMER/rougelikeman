namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class AlertLevel
    {
        public const byte warning = 1;
        public const byte fatal = 2;

        protected AlertLevel()
        {
        }

        public static string GetName(byte alertDescription)
        {
            if (alertDescription != 1)
            {
                if (alertDescription == 2)
                {
                    return "fatal";
                }
                return "UNKNOWN";
            }
            return "warning";
        }

        public static string GetText(byte alertDescription)
        {
            object[] objArray1 = new object[] { GetName(alertDescription), "(", alertDescription, ")" };
            return string.Concat(objArray1);
        }
    }
}

