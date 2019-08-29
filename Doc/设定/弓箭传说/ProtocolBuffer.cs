using System;
using System.IO;
using System.Text;

public sealed class ProtocolBuffer
{
    public static readonly MemoryStream CacheStream = new MemoryStream();
    public static readonly BinaryWriter MemoryWriter = new CustomBinaryWriter(CacheStream);
    public const uint Zero = 0;
    public static readonly UTF8Encoding Encoding = new UTF8Encoding();

    public static BinaryWriter Writer
    {
        get
        {
            MemoryWriter.BaseStream.SetLength(0L);
            return MemoryWriter;
        }
    }
}

