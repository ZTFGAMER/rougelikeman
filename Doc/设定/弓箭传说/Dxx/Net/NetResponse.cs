namespace Dxx.Net
{
    using GameProtocol;
    using System;

    public class NetResponse
    {
        public IProtocol data;
        public CCommonRespMsg error;

        public bool IsSuccess
        {
            get
            {
                if ((this.data != null) && (this.data is CRespItemPacket))
                {
                    CRespItemPacket data = this.data as CRespItemPacket;
                    if ((data != null) && (data.m_commMsg != null))
                    {
                        this.error = data.m_commMsg;
                        if ((this.error.m_nStatusCode == 0) || (this.error.m_nStatusCode == 1))
                        {
                            return true;
                        }
                        this.data = null;
                        return false;
                    }
                }
                return ((this.data != null) || ((this.error != null) && ((this.error.m_nStatusCode == 0) || (this.error.m_nStatusCode == 1))));
            }
        }

        public bool IsTimeOut =>
            ((this.data == null) && (this.error == null));
    }
}

