namespace Org.BouncyCastle.Asn1.Utilities
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;

    public sealed class Asn1Dump
    {
        private static readonly string NewLine = Platform.NewLine;
        private const string Tab = "    ";
        private const int SampleSize = 0x20;

        private Asn1Dump()
        {
        }

        private static void AsString(string indent, bool verbose, Asn1Object obj, StringBuilder buf)
        {
            switch (obj)
            {
                case (Asn1Sequence _):
                {
                    string str = indent + "    ";
                    buf.Append(indent);
                    if (obj is BerSequence)
                    {
                        buf.Append("BER Sequence");
                    }
                    else if (obj is DerSequence)
                    {
                        buf.Append("DER Sequence");
                    }
                    else
                    {
                        buf.Append("Sequence");
                    }
                    buf.Append(NewLine);
                    IEnumerator enumerator = ((Asn1Sequence) obj).GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            Asn1Encodable current = (Asn1Encodable) enumerator.Current;
                            if ((current == null) || (current is Asn1Null))
                            {
                                buf.Append(str);
                                buf.Append("NULL");
                                buf.Append(NewLine);
                            }
                            else
                            {
                                AsString(str, verbose, current.ToAsn1Object(), buf);
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator is IDisposable disposable)
                        {
                            IDisposable disposable;
                            disposable.Dispose();
                        }
                    }
                    break;
                }
                default:
                    if (obj is DerTaggedObject)
                    {
                        string str2 = indent + "    ";
                        buf.Append(indent);
                        if (obj is BerTaggedObject)
                        {
                            buf.Append("BER Tagged [");
                        }
                        else
                        {
                            buf.Append("Tagged [");
                        }
                        DerTaggedObject obj2 = (DerTaggedObject) obj;
                        buf.Append(obj2.TagNo.ToString());
                        buf.Append(']');
                        if (!obj2.IsExplicit())
                        {
                            buf.Append(" IMPLICIT ");
                        }
                        buf.Append(NewLine);
                        if (obj2.IsEmpty())
                        {
                            buf.Append(str2);
                            buf.Append("EMPTY");
                            buf.Append(NewLine);
                        }
                        else
                        {
                            AsString(str2, verbose, obj2.GetObject(), buf);
                        }
                    }
                    else
                    {
                        switch (obj)
                        {
                            case (BerSet _):
                            {
                                string str3 = indent + "    ";
                                buf.Append(indent);
                                buf.Append("BER Set");
                                buf.Append(NewLine);
                                IEnumerator enumerator2 = ((Asn1Set) obj).GetEnumerator();
                                try
                                {
                                    while (enumerator2.MoveNext())
                                    {
                                        Asn1Encodable current = (Asn1Encodable) enumerator2.Current;
                                        if (current == null)
                                        {
                                            buf.Append(str3);
                                            buf.Append("NULL");
                                            buf.Append(NewLine);
                                        }
                                        else
                                        {
                                            AsString(str3, verbose, current.ToAsn1Object(), buf);
                                        }
                                    }
                                }
                                finally
                                {
                                    if (enumerator2 is IDisposable disposable2)
                                    {
                                        IDisposable disposable2;
                                        disposable2.Dispose();
                                    }
                                }
                                break;
                            }
                            default:
                                if (obj is DerSet)
                                {
                                    string str4 = indent + "    ";
                                    buf.Append(indent);
                                    buf.Append("DER Set");
                                    buf.Append(NewLine);
                                    IEnumerator enumerator3 = ((Asn1Set) obj).GetEnumerator();
                                    try
                                    {
                                        while (enumerator3.MoveNext())
                                        {
                                            Asn1Encodable current = (Asn1Encodable) enumerator3.Current;
                                            if (current == null)
                                            {
                                                buf.Append(str4);
                                                buf.Append("NULL");
                                                buf.Append(NewLine);
                                            }
                                            else
                                            {
                                                AsString(str4, verbose, current.ToAsn1Object(), buf);
                                            }
                                        }
                                    }
                                    finally
                                    {
                                        if (enumerator3 is IDisposable disposable3)
                                        {
                                            IDisposable disposable3;
                                            disposable3.Dispose();
                                        }
                                    }
                                }
                                else if (obj is DerObjectIdentifier)
                                {
                                    buf.Append(indent + "ObjectIdentifier(" + ((DerObjectIdentifier) obj).Id + ")" + NewLine);
                                }
                                else if (obj is DerBoolean)
                                {
                                    buf.Append(string.Concat(new object[] { indent, "Boolean(", ((DerBoolean) obj).IsTrue, ")", NewLine }));
                                }
                                else if (obj is DerInteger)
                                {
                                    buf.Append(string.Concat(new object[] { indent, "Integer(", ((DerInteger) obj).Value, ")", NewLine }));
                                }
                                else if (obj is BerOctetString)
                                {
                                    byte[] octets = ((Asn1OctetString) obj).GetOctets();
                                    string str5 = !verbose ? string.Empty : dumpBinaryDataAsString(indent, octets);
                                    buf.Append(string.Concat(new object[] { indent, "BER Octet String[", octets.Length, "] ", str5, NewLine }));
                                }
                                else if (obj is DerOctetString)
                                {
                                    byte[] octets = ((Asn1OctetString) obj).GetOctets();
                                    string str6 = !verbose ? string.Empty : dumpBinaryDataAsString(indent, octets);
                                    buf.Append(string.Concat(new object[] { indent, "DER Octet String[", octets.Length, "] ", str6, NewLine }));
                                }
                                else if (obj is DerBitString)
                                {
                                    DerBitString str7 = (DerBitString) obj;
                                    byte[] bytes = str7.GetBytes();
                                    string str8 = !verbose ? string.Empty : dumpBinaryDataAsString(indent, bytes);
                                    buf.Append(string.Concat(new object[] { indent, "DER Bit String[", bytes.Length, ", ", str7.PadBits, "] ", str8, NewLine }));
                                }
                                else if (obj is DerIA5String)
                                {
                                    buf.Append(indent + "IA5String(" + ((DerIA5String) obj).GetString() + ") " + NewLine);
                                }
                                else if (obj is DerUtf8String)
                                {
                                    buf.Append(indent + "UTF8String(" + ((DerUtf8String) obj).GetString() + ") " + NewLine);
                                }
                                else if (obj is DerPrintableString)
                                {
                                    buf.Append(indent + "PrintableString(" + ((DerPrintableString) obj).GetString() + ") " + NewLine);
                                }
                                else if (obj is DerVisibleString)
                                {
                                    buf.Append(indent + "VisibleString(" + ((DerVisibleString) obj).GetString() + ") " + NewLine);
                                }
                                else if (obj is DerBmpString)
                                {
                                    buf.Append(indent + "BMPString(" + ((DerBmpString) obj).GetString() + ") " + NewLine);
                                }
                                else if (obj is DerT61String)
                                {
                                    buf.Append(indent + "T61String(" + ((DerT61String) obj).GetString() + ") " + NewLine);
                                }
                                else if (obj is DerGraphicString)
                                {
                                    buf.Append(indent + "GraphicString(" + ((DerGraphicString) obj).GetString() + ") " + NewLine);
                                }
                                else if (obj is DerVideotexString)
                                {
                                    buf.Append(indent + "VideotexString(" + ((DerVideotexString) obj).GetString() + ") " + NewLine);
                                }
                                else if (obj is DerUtcTime)
                                {
                                    buf.Append(indent + "UTCTime(" + ((DerUtcTime) obj).TimeString + ") " + NewLine);
                                }
                                else if (obj is DerGeneralizedTime)
                                {
                                    buf.Append(indent + "GeneralizedTime(" + ((DerGeneralizedTime) obj).GetTime() + ") " + NewLine);
                                }
                                else if (obj is BerApplicationSpecific)
                                {
                                    buf.Append(outputApplicationSpecific("BER", indent, verbose, (BerApplicationSpecific) obj));
                                }
                                else if (obj is DerApplicationSpecific)
                                {
                                    buf.Append(outputApplicationSpecific("DER", indent, verbose, (DerApplicationSpecific) obj));
                                }
                                else if (obj is DerEnumerated)
                                {
                                    DerEnumerated enumerated = (DerEnumerated) obj;
                                    buf.Append(string.Concat(new object[] { indent, "DER Enumerated(", enumerated.Value, ")", NewLine }));
                                }
                                else if (obj is DerExternal)
                                {
                                    DerExternal external = (DerExternal) obj;
                                    buf.Append(indent + "External " + NewLine);
                                    string str9 = indent + "    ";
                                    if (external.DirectReference != null)
                                    {
                                        buf.Append(str9 + "Direct Reference: " + external.DirectReference.Id + NewLine);
                                    }
                                    if (external.IndirectReference != null)
                                    {
                                        buf.Append(str9 + "Indirect Reference: " + external.IndirectReference.ToString() + NewLine);
                                    }
                                    if (external.DataValueDescriptor != null)
                                    {
                                        AsString(str9, verbose, external.DataValueDescriptor, buf);
                                    }
                                    buf.Append(string.Concat(new object[] { str9, "Encoding: ", external.Encoding, NewLine }));
                                    AsString(str9, verbose, external.ExternalContent, buf);
                                }
                                else
                                {
                                    buf.Append(indent + obj.ToString() + NewLine);
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        private static string calculateAscString(byte[] bytes, int off, int len)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = off; i != (off + len); i++)
            {
                char ch = (char) bytes[i];
                if ((ch >= ' ') && (ch <= '~'))
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }

        public static string DumpAsString(Asn1Encodable obj) => 
            DumpAsString(obj, false);

        [Obsolete("Use version accepting Asn1Encodable")]
        public static string DumpAsString(object obj)
        {
            if (obj is Asn1Encodable)
            {
                StringBuilder buf = new StringBuilder();
                AsString(string.Empty, false, ((Asn1Encodable) obj).ToAsn1Object(), buf);
                return buf.ToString();
            }
            return ("unknown object type " + obj.ToString());
        }

        public static string DumpAsString(Asn1Encodable obj, bool verbose)
        {
            StringBuilder buf = new StringBuilder();
            AsString(string.Empty, verbose, obj.ToAsn1Object(), buf);
            return buf.ToString();
        }

        private static string dumpBinaryDataAsString(string indent, byte[] bytes)
        {
            indent = indent + "    ";
            StringBuilder builder = new StringBuilder(NewLine);
            for (int i = 0; i < bytes.Length; i += 0x20)
            {
                if ((bytes.Length - i) > 0x20)
                {
                    builder.Append(indent);
                    builder.Append(Hex.ToHexString(bytes, i, 0x20));
                    builder.Append("    ");
                    builder.Append(calculateAscString(bytes, i, 0x20));
                    builder.Append(NewLine);
                }
                else
                {
                    builder.Append(indent);
                    builder.Append(Hex.ToHexString(bytes, i, bytes.Length - i));
                    for (int j = bytes.Length - i; j != 0x20; j++)
                    {
                        builder.Append("  ");
                    }
                    builder.Append("    ");
                    builder.Append(calculateAscString(bytes, i, bytes.Length - i));
                    builder.Append(NewLine);
                }
            }
            return builder.ToString();
        }

        private static string outputApplicationSpecific(string type, string indent, bool verbose, DerApplicationSpecific app)
        {
            StringBuilder buf = new StringBuilder();
            if (app.IsConstructed())
            {
                try
                {
                    Asn1Sequence instance = Asn1Sequence.GetInstance(app.GetObject(0x10));
                    buf.Append(string.Concat(new object[] { indent, type, " ApplicationSpecific[", app.ApplicationTag, "]", NewLine }));
                    IEnumerator enumerator = instance.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            AsString(indent + "    ", verbose, ((Asn1Encodable) enumerator.Current).ToAsn1Object(), buf);
                        }
                    }
                    finally
                    {
                        if (enumerator is IDisposable disposable)
                        {
                            IDisposable disposable;
                            disposable.Dispose();
                        }
                    }
                }
                catch (IOException exception)
                {
                    buf.Append(exception);
                }
                return buf.ToString();
            }
            object[] objArray2 = new object[] { indent, type, " ApplicationSpecific[", app.ApplicationTag, "] (", Hex.ToHexString(app.GetContents()), ")", NewLine };
            return string.Concat(objArray2);
        }
    }
}

