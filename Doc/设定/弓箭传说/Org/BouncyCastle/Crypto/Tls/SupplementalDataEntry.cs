namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public class SupplementalDataEntry
    {
        protected readonly int mDataType;
        protected readonly byte[] mData;

        public SupplementalDataEntry(int dataType, byte[] data)
        {
            this.mDataType = dataType;
            this.mData = data;
        }

        public virtual int DataType =>
            this.mDataType;

        public virtual byte[] Data =>
            this.mData;
    }
}

