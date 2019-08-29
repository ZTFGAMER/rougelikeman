namespace Org.BouncyCastle.Asn1.X9
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class X962Parameters : Asn1Encodable, IAsn1Choice
    {
        private readonly Asn1Object _params;

        public X962Parameters(Asn1Object obj)
        {
            this._params = obj;
        }

        public X962Parameters(DerObjectIdentifier namedCurve)
        {
            this._params = namedCurve;
        }

        public X962Parameters(X9ECParameters ecParameters)
        {
            this._params = ecParameters.ToAsn1Object();
        }

        public static X962Parameters GetInstance(object obj)
        {
            if ((obj == null) || (obj is X962Parameters))
            {
                return (X962Parameters) obj;
            }
            if (obj is Asn1Object)
            {
                return new X962Parameters((Asn1Object) obj);
            }
            if (obj is byte[])
            {
                try
                {
                    return new X962Parameters(Asn1Object.FromByteArray((byte[]) obj));
                }
                catch (Exception exception)
                {
                    throw new ArgumentException("unable to parse encoded data: " + exception.Message, exception);
                }
            }
            throw new ArgumentException("unknown object in getInstance()");
        }

        public override Asn1Object ToAsn1Object() => 
            this._params;

        public bool IsNamedCurve =>
            (this._params is DerObjectIdentifier);

        public bool IsImplicitlyCA =>
            (this._params is Asn1Null);

        public Asn1Object Parameters =>
            this._params;
    }
}

