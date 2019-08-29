namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class DefaultTlsClient : AbstractTlsClient
    {
        public DefaultTlsClient()
        {
        }

        public DefaultTlsClient(TlsCipherFactory cipherFactory) : base(cipherFactory)
        {
        }

        protected virtual TlsKeyExchange CreateDheKeyExchange(int keyExchange) => 
            new TlsDheKeyExchange(keyExchange, base.mSupportedSignatureAlgorithms, null);

        protected virtual TlsKeyExchange CreateDHKeyExchange(int keyExchange) => 
            new TlsDHKeyExchange(keyExchange, base.mSupportedSignatureAlgorithms, null);

        protected virtual TlsKeyExchange CreateECDheKeyExchange(int keyExchange) => 
            new TlsECDheKeyExchange(keyExchange, base.mSupportedSignatureAlgorithms, base.mNamedCurves, base.mClientECPointFormats, base.mServerECPointFormats);

        protected virtual TlsKeyExchange CreateECDHKeyExchange(int keyExchange) => 
            new TlsECDHKeyExchange(keyExchange, base.mSupportedSignatureAlgorithms, base.mNamedCurves, base.mClientECPointFormats, base.mServerECPointFormats);

        protected virtual TlsKeyExchange CreateRsaKeyExchange() => 
            new TlsRsaKeyExchange(base.mSupportedSignatureAlgorithms);

        public override int[] GetCipherSuites() => 
            new int[] { 0xc02b, 0xc023, 0xc009, 0xc02f, 0xc027, 0xc013, 0xa2, 0x40, 50, 0x9e, 0x67, 0x33, 0x9c, 60, 0x2f };

        public override TlsKeyExchange GetKeyExchange()
        {
            int keyExchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm(base.mSelectedCipherSuite);
            switch (keyExchangeAlgorithm)
            {
                case 1:
                    return this.CreateRsaKeyExchange();

                case 3:
                case 5:
                    return this.CreateDheKeyExchange(keyExchangeAlgorithm);

                case 7:
                case 9:
                    return this.CreateDHKeyExchange(keyExchangeAlgorithm);

                case 0x10:
                case 0x12:
                case 20:
                    return this.CreateECDHKeyExchange(keyExchangeAlgorithm);

                case 0x11:
                case 0x13:
                    return this.CreateECDheKeyExchange(keyExchangeAlgorithm);
            }
            throw new TlsFatalAlert(80);
        }
    }
}

