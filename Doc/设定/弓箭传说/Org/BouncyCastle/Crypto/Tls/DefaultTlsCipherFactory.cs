namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Crypto.Modes;
    using System;

    public class DefaultTlsCipherFactory : AbstractTlsCipherFactory
    {
        protected virtual IAeadBlockCipher CreateAeadBlockCipher_Aes_Ccm() => 
            new CcmBlockCipher(this.CreateAesEngine());

        protected virtual IAeadBlockCipher CreateAeadBlockCipher_Aes_Gcm() => 
            new GcmBlockCipher(this.CreateAesEngine());

        protected virtual IAeadBlockCipher CreateAeadBlockCipher_Aes_Ocb() => 
            new OcbBlockCipher(this.CreateAesEngine(), this.CreateAesEngine());

        protected virtual IAeadBlockCipher CreateAeadBlockCipher_Camellia_Gcm() => 
            new GcmBlockCipher(this.CreateCamelliaEngine());

        protected virtual IBlockCipher CreateAesBlockCipher() => 
            new CbcBlockCipher(this.CreateAesEngine());

        protected virtual TlsBlockCipher CreateAESCipher(TlsContext context, int cipherKeySize, int macAlgorithm) => 
            new TlsBlockCipher(context, this.CreateAesBlockCipher(), this.CreateAesBlockCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), cipherKeySize);

        protected virtual IBlockCipher CreateAesEngine() => 
            new AesEngine();

        protected virtual IBlockCipher CreateCamelliaBlockCipher() => 
            new CbcBlockCipher(this.CreateCamelliaEngine());

        protected virtual TlsBlockCipher CreateCamelliaCipher(TlsContext context, int cipherKeySize, int macAlgorithm) => 
            new TlsBlockCipher(context, this.CreateCamelliaBlockCipher(), this.CreateCamelliaBlockCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), cipherKeySize);

        protected virtual IBlockCipher CreateCamelliaEngine() => 
            new CamelliaEngine();

        protected virtual TlsCipher CreateChaCha20Poly1305(TlsContext context) => 
            new Chacha20Poly1305(context);

        public override TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm)
        {
            switch (encryptionAlgorithm)
            {
                case 7:
                    return this.CreateDesEdeCipher(context, macAlgorithm);

                case 8:
                    return this.CreateAESCipher(context, 0x10, macAlgorithm);

                case 9:
                    return this.CreateAESCipher(context, 0x20, macAlgorithm);

                case 10:
                    return this.CreateCipher_Aes_Gcm(context, 0x10, 0x10);

                case 11:
                    return this.CreateCipher_Aes_Gcm(context, 0x20, 0x10);

                case 12:
                    return this.CreateCamelliaCipher(context, 0x10, macAlgorithm);

                case 13:
                    return this.CreateCamelliaCipher(context, 0x20, macAlgorithm);

                case 14:
                    return this.CreateSeedCipher(context, macAlgorithm);

                case 15:
                    return this.CreateCipher_Aes_Ccm(context, 0x10, 0x10);

                case 0x10:
                    return this.CreateCipher_Aes_Ccm(context, 0x10, 8);

                case 0x11:
                    return this.CreateCipher_Aes_Ccm(context, 0x20, 0x10);

                case 0x12:
                    return this.CreateCipher_Aes_Ccm(context, 0x20, 8);

                case 0x13:
                    return this.CreateCipher_Camellia_Gcm(context, 0x10, 0x10);

                case 20:
                    return this.CreateCipher_Camellia_Gcm(context, 0x20, 0x10);

                case 0x66:
                    return this.CreateChaCha20Poly1305(context);

                case 0x67:
                    return this.CreateCipher_Aes_Ocb(context, 0x10, 12);

                case 0x68:
                    return this.CreateCipher_Aes_Ocb(context, 0x20, 12);

                case 0:
                    return this.CreateNullCipher(context, macAlgorithm);

                case 2:
                    return this.CreateRC4Cipher(context, 0x10, macAlgorithm);
            }
            throw new TlsFatalAlert(80);
        }

        protected virtual TlsAeadCipher CreateCipher_Aes_Ccm(TlsContext context, int cipherKeySize, int macSize) => 
            new TlsAeadCipher(context, this.CreateAeadBlockCipher_Aes_Ccm(), this.CreateAeadBlockCipher_Aes_Ccm(), cipherKeySize, macSize);

        protected virtual TlsAeadCipher CreateCipher_Aes_Gcm(TlsContext context, int cipherKeySize, int macSize) => 
            new TlsAeadCipher(context, this.CreateAeadBlockCipher_Aes_Gcm(), this.CreateAeadBlockCipher_Aes_Gcm(), cipherKeySize, macSize);

        protected virtual TlsAeadCipher CreateCipher_Aes_Ocb(TlsContext context, int cipherKeySize, int macSize) => 
            new TlsAeadCipher(context, this.CreateAeadBlockCipher_Aes_Ocb(), this.CreateAeadBlockCipher_Aes_Ocb(), cipherKeySize, macSize, 2);

        protected virtual TlsAeadCipher CreateCipher_Camellia_Gcm(TlsContext context, int cipherKeySize, int macSize) => 
            new TlsAeadCipher(context, this.CreateAeadBlockCipher_Camellia_Gcm(), this.CreateAeadBlockCipher_Camellia_Gcm(), cipherKeySize, macSize);

        protected virtual IBlockCipher CreateDesEdeBlockCipher() => 
            new CbcBlockCipher(new DesEdeEngine());

        protected virtual TlsBlockCipher CreateDesEdeCipher(TlsContext context, int macAlgorithm) => 
            new TlsBlockCipher(context, this.CreateDesEdeBlockCipher(), this.CreateDesEdeBlockCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), 0x18);

        protected virtual IDigest CreateHMacDigest(int macAlgorithm)
        {
            switch (macAlgorithm)
            {
                case 0:
                    return null;

                case 1:
                    return TlsUtilities.CreateHash((byte) 1);

                case 2:
                    return TlsUtilities.CreateHash((byte) 2);

                case 3:
                    return TlsUtilities.CreateHash((byte) 4);

                case 4:
                    return TlsUtilities.CreateHash((byte) 5);

                case 5:
                    return TlsUtilities.CreateHash((byte) 6);
            }
            throw new TlsFatalAlert(80);
        }

        protected virtual TlsNullCipher CreateNullCipher(TlsContext context, int macAlgorithm) => 
            new TlsNullCipher(context, this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm));

        protected virtual TlsStreamCipher CreateRC4Cipher(TlsContext context, int cipherKeySize, int macAlgorithm) => 
            new TlsStreamCipher(context, this.CreateRC4StreamCipher(), this.CreateRC4StreamCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), cipherKeySize, false);

        protected virtual IStreamCipher CreateRC4StreamCipher() => 
            new RC4Engine();

        protected virtual IBlockCipher CreateSeedBlockCipher() => 
            new CbcBlockCipher(new SeedEngine());

        protected virtual TlsBlockCipher CreateSeedCipher(TlsContext context, int macAlgorithm) => 
            new TlsBlockCipher(context, this.CreateSeedBlockCipher(), this.CreateSeedBlockCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), 0x10);
    }
}

