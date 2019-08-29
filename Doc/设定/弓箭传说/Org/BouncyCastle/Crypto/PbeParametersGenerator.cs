namespace Org.BouncyCastle.Crypto
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Text;

    public abstract class PbeParametersGenerator
    {
        protected byte[] mPassword;
        protected byte[] mSalt;
        protected int mIterationCount;

        protected PbeParametersGenerator()
        {
        }

        public abstract ICipherParameters GenerateDerivedMacParameters(int keySize);
        [Obsolete("Use version with 'algorithm' parameter")]
        public abstract ICipherParameters GenerateDerivedParameters(int keySize);
        [Obsolete("Use version with 'algorithm' parameter")]
        public abstract ICipherParameters GenerateDerivedParameters(int keySize, int ivSize);
        public abstract ICipherParameters GenerateDerivedParameters(string algorithm, int keySize);
        public abstract ICipherParameters GenerateDerivedParameters(string algorithm, int keySize, int ivSize);
        [Obsolete("Use 'Password' property")]
        public byte[] GetPassword() => 
            this.Password;

        [Obsolete("Use 'Salt' property")]
        public byte[] GetSalt() => 
            this.Salt;

        public virtual void Init(byte[] password, byte[] salt, int iterationCount)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            if (salt == null)
            {
                throw new ArgumentNullException("salt");
            }
            this.mPassword = Arrays.Clone(password);
            this.mSalt = Arrays.Clone(salt);
            this.mIterationCount = iterationCount;
        }

        public static byte[] Pkcs12PasswordToBytes(char[] password) => 
            Pkcs12PasswordToBytes(password, false);

        public static byte[] Pkcs12PasswordToBytes(char[] password, bool wrongPkcs12Zero)
        {
            if ((password == null) || (password.Length < 1))
            {
                return new byte[!wrongPkcs12Zero ? 0 : 2];
            }
            byte[] bytes = new byte[(password.Length + 1) * 2];
            Encoding.BigEndianUnicode.GetBytes(password, 0, password.Length, bytes, 0);
            return bytes;
        }

        public static byte[] Pkcs5PasswordToBytes(char[] password)
        {
            if (password == null)
            {
                return new byte[0];
            }
            return Strings.ToByteArray(password);
        }

        [Obsolete("Use version taking 'char[]' instead")]
        public static byte[] Pkcs5PasswordToBytes(string password)
        {
            if (password == null)
            {
                return new byte[0];
            }
            return Strings.ToByteArray(password);
        }

        public static byte[] Pkcs5PasswordToUtf8Bytes(char[] password)
        {
            if (password == null)
            {
                return new byte[0];
            }
            return Encoding.UTF8.GetBytes(password);
        }

        [Obsolete("Use version taking 'char[]' instead")]
        public static byte[] Pkcs5PasswordToUtf8Bytes(string password)
        {
            if (password == null)
            {
                return new byte[0];
            }
            return Encoding.UTF8.GetBytes(password);
        }

        public virtual byte[] Password =>
            Arrays.Clone(this.mPassword);

        public virtual byte[] Salt =>
            Arrays.Clone(this.mSalt);

        public virtual int IterationCount =>
            this.mIterationCount;
    }
}

