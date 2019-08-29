using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

public static class SerializeHelper
{
    public static byte[] ConvertToByte(string str) => 
        Encoding.UTF8.GetBytes(str);

    public static byte[] ConvertToByte(string str, Encoding encoding) => 
        encoding.GetBytes(str);

    public static string ConvertToString(byte[] data) => 
        Encoding.UTF8.GetString(data, 0, data.Length);

    public static string ConvertToString(byte[] data, Encoding encoding) => 
        encoding.GetString(data, 0, data.Length);

    public static object DeserializeWithBinary(byte[] data)
    {
        MemoryStream serializationStream = new MemoryStream();
        serializationStream.Write(data, 0, data.Length);
        serializationStream.Position = 0L;
        object obj2 = new BinaryFormatter().Deserialize(serializationStream);
        serializationStream.Close();
        return obj2;
    }

    public static T DeserializeWithBinary<T>(byte[] data) => 
        ((T) DeserializeWithBinary(data));

    public static T DeserializeWithXml<T>(byte[] data)
    {
        MemoryStream stream = new MemoryStream();
        stream.Write(data, 0, data.Length);
        stream.Position = 0L;
        object obj2 = new XmlSerializer(typeof(T)).Deserialize(stream);
        stream.Close();
        return (T) obj2;
    }

    public static byte[] SerializeToBinary(object obj)
    {
        MemoryStream serializationStream = new MemoryStream();
        new BinaryFormatter().Serialize(serializationStream, obj);
        byte[] buffer = serializationStream.ToArray();
        serializationStream.Close();
        return buffer;
    }

    public static byte[] SerializeToXml(object obj)
    {
        MemoryStream stream = new MemoryStream();
        new XmlSerializer(obj.GetType()).Serialize((Stream) stream, obj);
        byte[] buffer = stream.ToArray();
        stream.Close();
        return buffer;
    }
}

