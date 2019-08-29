namespace Org.BouncyCastle.Asn1.X9
{
    using System;

    public abstract class X9ECParametersHolder
    {
        private X9ECParameters parameters;

        protected X9ECParametersHolder()
        {
        }

        protected abstract X9ECParameters CreateParameters();

        public X9ECParameters Parameters
        {
            get
            {
                object obj2 = this;
                lock (obj2)
                {
                    if (this.parameters == null)
                    {
                        this.parameters = this.CreateParameters();
                    }
                    return this.parameters;
                }
            }
        }
    }
}

