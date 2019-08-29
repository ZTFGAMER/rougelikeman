using System;
using System.IO;

public class CustomBinaryWriter : BinaryWriter
{
    public CustomBinaryWriter(Stream stream) : base(stream)
    {
    }

    public override void Write(string value)
    {
        ushort byteCount = (ushort) ProtocolBuffer.Encoding.GetByteCount(value);
        byte[] bytes = ProtocolBuffer.Encoding.GetBytes(value);
        base.Write(byteCount);
        if (bytes.Length > 0)
        {
            base.Write(bytes);
        }
    }
}

