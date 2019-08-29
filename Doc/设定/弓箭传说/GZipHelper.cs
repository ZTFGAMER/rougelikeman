using ICSharpCode.SharpZipLib.GZip;
using System;
using System.IO;

public class GZipHelper
{
    public static byte[] Depress(byte[] press)
    {
        GZipInputStream stream = new GZipInputStream(new MemoryStream(press));
        MemoryStream stream2 = new MemoryStream();
        int count = 0;
        byte[] buffer = new byte[0x1000];
        while ((count = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            stream2.Write(buffer, 0, count);
        }
        return stream2.ToArray();
    }

    public static byte[] Press(byte[] binary)
    {
        MemoryStream stream = new MemoryStream();
        GZipOutputStream stream2 = new GZipOutputStream(stream);
        stream2.Write(binary, 0, binary.Length);
        stream2.Close();
        return stream.ToArray();
    }
}

