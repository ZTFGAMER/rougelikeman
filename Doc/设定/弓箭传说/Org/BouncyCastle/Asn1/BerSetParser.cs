﻿namespace Org.BouncyCastle.Asn1
{
    using System;

    public class BerSetParser : Asn1SetParser, IAsn1Convertible
    {
        private readonly Asn1StreamParser _parser;

        internal BerSetParser(Asn1StreamParser parser)
        {
            this._parser = parser;
        }

        public IAsn1Convertible ReadObject() => 
            this._parser.ReadObject();

        public Asn1Object ToAsn1Object() => 
            new BerSet(this._parser.ReadVector(), false);
    }
}

