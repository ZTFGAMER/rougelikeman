namespace Org.BouncyCastle.X509.Extension
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Security.Certificates;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.X509;
    using System;
    using System.Collections;
    using System.IO;

    public class X509ExtensionUtilities
    {
        public static Asn1Object FromExtensionValue(Asn1OctetString extensionValue) => 
            Asn1Object.FromByteArray(extensionValue.GetOctets());

        private static ICollection GetAlternativeName(Asn1OctetString extVal)
        {
            IList list = Platform.CreateArrayList();
            if (extVal != null)
            {
                try
                {
                    IEnumerator enumerator = Asn1Sequence.GetInstance(FromExtensionValue(extVal)).GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            GeneralName current = (GeneralName) enumerator.Current;
                            IList list2 = Platform.CreateArrayList();
                            list2.Add(current.TagNo);
                            switch (current.TagNo)
                            {
                                case 0:
                                case 3:
                                case 5:
                                    list2.Add(current.Name.ToAsn1Object());
                                    break;

                                case 1:
                                case 2:
                                case 6:
                                    list2.Add(((IAsn1String) current.Name).GetString());
                                    break;

                                case 4:
                                    list2.Add(X509Name.GetInstance(current.Name).ToString());
                                    break;

                                case 7:
                                    list2.Add(Asn1OctetString.GetInstance(current.Name).GetOctets());
                                    break;

                                case 8:
                                    list2.Add(DerObjectIdentifier.GetInstance(current.Name).Id);
                                    break;

                                default:
                                    throw new IOException("Bad tag number: " + current.TagNo);
                            }
                            list.Add(list2);
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
                catch (Exception exception)
                {
                    throw new CertificateParsingException(exception.Message);
                }
            }
            return list;
        }

        public static ICollection GetIssuerAlternativeNames(X509Certificate cert) => 
            GetAlternativeName(cert.GetExtensionValue(X509Extensions.IssuerAlternativeName));

        public static ICollection GetSubjectAlternativeNames(X509Certificate cert) => 
            GetAlternativeName(cert.GetExtensionValue(X509Extensions.SubjectAlternativeName));
    }
}

