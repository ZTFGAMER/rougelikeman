using System;
using System.IO;

public class CustomBinaryReader : BinaryReader
{
    public CustomBinaryReader(Stream stream) : base(stream)
    {
    }

    public override string ReadString()
    {
        ushort count = this.ReadUInt16();
        byte[] bytes = this.ReadBytes(count);
        return ProtocolBuffer.Encoding.GetString(bytes, 0, count);
    }
}

