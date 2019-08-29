namespace Org.BouncyCastle.Utilities.Net
{
    using System;
    using System.Globalization;

    public class IPAddress
    {
        private static bool IsMaskValue(string component, int size)
        {
            int num = int.Parse(component);
            try
            {
                return ((num >= 0) && (num <= size));
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }
            return false;
        }

        public static bool IsValid(string address) => 
            (IsValidIPv4(address) || IsValidIPv6(address));

        public static bool IsValidIPv4(string address)
        {
            try
            {
                return unsafeIsValidIPv4(address);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }
            return false;
        }

        public static bool IsValidIPv4WithNetmask(string address)
        {
            int index = address.IndexOf('/');
            string str = address.Substring(index + 1);
            return (((index > 0) && IsValidIPv4(address.Substring(0, index))) && (IsValidIPv4(str) || IsMaskValue(str, 0x20)));
        }

        public static bool IsValidIPv6(string address)
        {
            try
            {
                return unsafeIsValidIPv6(address);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }
            return false;
        }

        public static bool IsValidIPv6WithNetmask(string address)
        {
            int index = address.IndexOf('/');
            string str = address.Substring(index + 1);
            return ((index > 0) && (IsValidIPv6(address.Substring(0, index)) && (IsValidIPv6(str) || IsMaskValue(str, 0x80))));
        }

        public static bool IsValidWithNetMask(string address) => 
            (IsValidIPv4WithNetmask(address) || IsValidIPv6WithNetmask(address));

        private static bool unsafeIsValidIPv4(string address)
        {
            int num2;
            if (address.Length == 0)
            {
                return false;
            }
            int num = 0;
            string str = address + ".";
            int startIndex = 0;
            while ((startIndex < str.Length) && ((num2 = str.IndexOf('.', startIndex)) > startIndex))
            {
                if (num == 4)
                {
                    return false;
                }
                int num4 = int.Parse(str.Substring(startIndex, num2 - startIndex));
                if ((num4 < 0) || (num4 > 0xff))
                {
                    return false;
                }
                startIndex = num2 + 1;
                num++;
            }
            return (num == 4);
        }

        private static bool unsafeIsValidIPv6(string address)
        {
            int num2;
            if (address.Length == 0)
            {
                return false;
            }
            int num = 0;
            string str = address + ":";
            bool flag = false;
            int startIndex = 0;
            while ((startIndex < str.Length) && ((num2 = str.IndexOf(':', startIndex)) >= startIndex))
            {
                if (num == 8)
                {
                    return false;
                }
                if (startIndex != num2)
                {
                    string str2 = str.Substring(startIndex, num2 - startIndex);
                    if ((num2 != (str.Length - 1)) || (str2.IndexOf('.') <= 0))
                    {
                        int num4 = int.Parse(str.Substring(startIndex, num2 - startIndex), NumberStyles.AllowHexSpecifier);
                        if ((num4 < 0) || (num4 > 0xffff))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!IsValidIPv4(str2))
                        {
                            return false;
                        }
                        num++;
                    }
                }
                else
                {
                    if (((num2 != 1) && (num2 != (str.Length - 1))) && flag)
                    {
                        return false;
                    }
                    flag = true;
                }
                startIndex = num2 + 1;
                num++;
            }
            return ((num == 8) || flag);
        }
    }
}

