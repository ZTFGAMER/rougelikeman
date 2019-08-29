namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    public abstract class X509NameEntryConverter
    {
        protected X509NameEntryConverter()
        {
        }

        protected bool CanBePrintable(string str) => 
            DerPrintableString.IsPrintableString(str);

        protected Asn1Object ConvertHexEncoded(string hexString, int offset) => 
            Asn1Object.FromByteArray(Hex.Decode(hexString.Substring(offset)));

        public abstract Asn1Object GetConvertedValue(DerObjectIdentifier oid, string value);
    }
}

