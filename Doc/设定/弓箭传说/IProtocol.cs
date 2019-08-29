using System;
using System.IO;

public interface IProtocol
{
    void ReadFromStream(BinaryReader reader);
    void WriteToStream(BinaryWriter writer);

    ushort GetMsgType { get; }
}

