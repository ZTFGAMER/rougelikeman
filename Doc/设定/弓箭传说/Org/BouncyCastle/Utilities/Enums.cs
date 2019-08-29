namespace Org.BouncyCastle.Utilities
{
    using Org.BouncyCastle.Utilities.Date;
    using System;

    internal abstract class Enums
    {
        protected Enums()
        {
        }

        internal static Enum GetArbitraryValue(Type enumType)
        {
            Array enumValues = GetEnumValues(enumType);
            int index = ((int) (DateTimeUtilities.CurrentUnixMs() & 0x7fffffffL)) % enumValues.Length;
            return (Enum) enumValues.GetValue(index);
        }

        internal static Enum GetEnumValue(Type enumType, string s)
        {
            if (((s.Length <= 0) || !char.IsLetter(s[0])) || (s.IndexOf(',') >= 0))
            {
                throw new ArgumentException();
            }
            s = s.Replace('-', '_');
            s = s.Replace('/', '_');
            return (Enum) Enum.Parse(enumType, s, false);
        }

        internal static Array GetEnumValues(Type enumType) => 
            Enum.GetValues(enumType);

        internal static bool IsEnumType(Type t) => 
            t.IsEnum;
    }
}

