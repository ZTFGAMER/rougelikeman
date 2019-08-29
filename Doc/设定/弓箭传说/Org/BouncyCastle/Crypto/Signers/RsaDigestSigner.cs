namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.Nist;
    using Org.BouncyCastle.Asn1.Pkcs;
    using Org.BouncyCastle.Asn1.TeleTrust;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Encodings;
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;

    public class RsaDigestSigner : ISigner
    {
        private readonly IAsymmetricBlockCipher rsaEngine;
        private readonly AlgorithmIdentifier algId;
        private readonly IDigest digest;
        private bool forSigning;
        private static readonly IDictionary oidMap = Platform.CreateHashtable();

        static RsaDigestSigner()
        {
            oidMap["RIPEMD128"] = TeleTrusTObjectIdentifiers.RipeMD128;
            oidMap["RIPEMD160"] = TeleTrusTObjectIdentifiers.RipeMD160;
            oidMap["RIPEMD256"] = TeleTrusTObjectIdentifiers.RipeMD256;
            oidMap["SHA-1"] = X509ObjectIdentifiers.IdSha1;
            oidMap["SHA-224"] = NistObjectIdentifiers.IdSha224;
            oidMap["SHA-256"] = NistObjectIdentifiers.IdSha256;
            oidMap["SHA-384"] = NistObjectIdentifiers.IdSha384;
            oidMap["SHA-512"] = NistObjectIdentifiers.IdSha512;
            oidMap["MD2"] = PkcsObjectIdentifiers.MD2;
            oidMap["MD4"] = PkcsObjectIdentifiers.MD4;
            oidMap["MD5"] = PkcsObjectIdentifiers.MD5;
        }

        public RsaDigestSigner(IDigest digest) : this(digest, (DerObjectIdentifier) oidMap[digest.AlgorithmName])
        {
        }

        public RsaDigestSigner(IDigest digest, DerObjectIdentifier digestOid) : this(digest, new AlgorithmIdentifier(digestOid, DerNull.Instance))
        {
        }

        public RsaDigestSigner(IDigest digest, AlgorithmIdentifier algId)
        {
            this.rsaEngine = new Pkcs1Encoding(new RsaBlindedEngine());
            this.digest = digest;
            this.algId = algId;
        }

        public virtual void BlockUpdate(byte[] input, int inOff, int length)
        {
            this.digest.BlockUpdate(input, inOff, length);
        }

        private byte[] DerEncode(byte[] hash)
        {
            if (this.algId == null)
            {
                return hash;
            }
            DigestInfo info = new DigestInfo(this.algId, hash);
            return info.GetDerEncoded();
        }

        public virtual byte[] GenerateSignature()
        {
            if (!this.forSigning)
            {
                throw new InvalidOperationException("RsaDigestSigner not initialised for signature generation.");
            }
            byte[] output = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal(output, 0);
            byte[] inBuf = this.DerEncode(output);
            return this.rsaEngine.ProcessBlock(inBuf, 0, inBuf.Length);
        }

        public virtual void Init(bool forSigning, ICipherParameters parameters)
        {
            AsymmetricKeyParameter parameter;
            this.forSigning = forSigning;
            if (parameters is ParametersWithRandom)
            {
                parameter = (AsymmetricKeyParameter) ((ParametersWithRandom) parameters).Parameters;
            }
            else
            {
                parameter = (AsymmetricKeyParameter) parameters;
            }
            if (forSigning && !parameter.IsPrivate)
            {
                throw new InvalidKeyException("Signing requires private key.");
            }
            if (!forSigning && parameter.IsPrivate)
            {
                throw new InvalidKeyException("Verification requires public key.");
            }
            this.Reset();
            this.rsaEngine.Init(forSigning, parameters);
        }

        public virtual void Reset()
        {
            this.digest.Reset();
        }

        public virtual void Update(byte input)
        {
            this.digest.Update(input);
        }

        public virtual bool VerifySignature(byte[] signature)
        {
            byte[] buffer2;
            byte[] buffer3;
            if (this.forSigning)
            {
                throw new InvalidOperationException("RsaDigestSigner not initialised for verification");
            }
            byte[] output = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal(output, 0);
            try
            {
                buffer2 = this.rsaEngine.ProcessBlock(signature, 0, signature.Length);
                buffer3 = this.DerEncode(output);
            }
            catch (Exception)
            {
                return false;
            }
            if (buffer2.Length == buffer3.Length)
            {
                return Arrays.ConstantTimeAreEqual(buffer2, buffer3);
            }
            if (buffer2.Length != (buffer3.Length - 2))
            {
                return false;
            }
            int num = (buffer2.Length - output.Length) - 2;
            int num2 = (buffer3.Length - output.Length) - 2;
            buffer3[1] = (byte) (buffer3[1] - 2);
            buffer3[3] = (byte) (buffer3[3] - 2);
            int num3 = 0;
            for (int i = 0; i < output.Length; i++)
            {
                num3 |= buffer2[num + i] ^ buffer3[num2 + i];
            }
            for (int j = 0; j < num; j++)
            {
                num3 |= buffer2[j] ^ buffer3[j];
            }
            return (num3 == 0);
        }

        public virtual string AlgorithmName =>
            (this.digest.AlgorithmName + "withRSA");
    }
}

