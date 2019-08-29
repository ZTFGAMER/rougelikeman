using Newtonsoft.Json;
using System;
using System.IO;

[Serializable]
public abstract class CProtocolBase : IProtocol
{
    [JsonIgnore]
    private string _strUserID = string.Empty;
    [JsonIgnore]
    private ushort _nSoftVersion;
    [JsonIgnore]
    private string _strDeviceID = string.Empty;
    [JsonIgnore]
    private string _strAccessToken = string.Empty;

    protected CProtocolBase()
    {
    }

    public virtual byte[] buildPacket()
    {
        BinaryWriter writer = ProtocolBuffer.Writer;
        writer.Write((byte) 13);
        writer.Write(this.GetMsgType);
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer2 = new CustomBinaryWriter(stream);
        this.WriteToStream(writer2);
        ushort length = (ushort) stream.ToArray().Length;
        writer.Write(length);
        writer.Write(stream.ToArray());
        return ProtocolBuffer.CacheStream.ToArray();
    }

    protected abstract void OnReadFromStream(BinaryReader reader);
    protected abstract void OnWriteToStream(BinaryWriter writer);
    public void ReadFromStream(BinaryReader reader)
    {
        this.m_strUserID = reader.ReadString();
        this.m_nSoftVersion = reader.ReadUInt16();
        this.m_strDeviceID = reader.ReadString();
        this.m_strAccessToken = reader.ReadString();
        this.OnReadFromStream(reader);
    }

    public void WriteToStream(BinaryWriter writer)
    {
        writer.Write(this.m_strUserID);
        writer.Write(this.m_nSoftVersion);
        writer.Write(this.m_strDeviceID);
        writer.Write(this.m_strAccessToken);
        this.OnWriteToStream(writer);
    }

    public abstract ushort GetMsgType { get; }

    [JsonIgnore]
    public string m_strUserID
    {
        get
        {
            if (this._strUserID == string.Empty)
            {
                this._strUserID = LocalSave.Instance.GetUserID();
            }
            return this._strUserID;
        }
        set => 
            (this._strUserID = value);
    }

    [JsonIgnore]
    public ushort m_nSoftVersion
    {
        get
        {
            if (this._nSoftVersion == 0)
            {
                this._nSoftVersion = PlatformHelper.GetAppVersionCode();
            }
            return this._nSoftVersion;
        }
        set => 
            (this._nSoftVersion = value);
    }

    [JsonIgnore]
    public string m_strDeviceID
    {
        get
        {
            if (this._strDeviceID == string.Empty)
            {
                this._strDeviceID = PlatformHelper.GetUUID();
            }
            return this._strDeviceID;
        }
        set => 
            (this._strDeviceID = value);
    }

    [JsonIgnore]
    public string m_strAccessToken
    {
        get
        {
            if (this._strAccessToken == string.Empty)
            {
                this._strAccessToken = LocalSave.Instance.GetServerUserID().ToString();
            }
            return this._strAccessToken;
        }
        set => 
            (this._strAccessToken = value);
    }
}

