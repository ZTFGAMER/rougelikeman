﻿namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class CrlEntry : Asn1Encodable
    {
        internal Asn1Sequence seq;
        internal DerInteger userCertificate;
        internal Time revocationDate;
        internal X509Extensions crlEntryExtensions;

        public CrlEntry(Asn1Sequence seq)
        {
            if ((seq.Count < 2) || (seq.Count > 3))
            {
                throw new ArgumentException("Bad sequence size: " + seq.Count);
            }
            this.seq = seq;
            this.userCertificate = DerInteger.GetInstance(seq[0]);
            this.revocationDate = Time.GetInstance(seq[1]);
        }

        public override Asn1Object ToAsn1Object() => 
            this.seq;

        public DerInteger UserCertificate =>
            this.userCertificate;

        public Time RevocationDate =>
            this.revocationDate;

        public X509Extensions Extensions
        {
            get
            {
                if ((this.crlEntryExtensions == null) && (this.seq.Count == 3))
                {
                    this.crlEntryExtensions = X509Extensions.GetInstance(this.seq[2]);
                }
                return this.crlEntryExtensions;
            }
        }
    }
}

