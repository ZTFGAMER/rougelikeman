using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

public static class PassWordHelper
{
    private const string PublicRsaKey = "pubKey";
    private const string PrivateRsaKey = "priKey";

    public static byte[] AesByte(this byte[] data, string keyVal, string ivVal)
    {
        byte[] buffer3;
        byte[] destinationArray = new byte[0x20];
        Array.Copy(Encoding.UTF8.GetBytes(keyVal.PadRight(destinationArray.Length)), destinationArray, destinationArray.Length);
        byte[] buffer2 = new byte[0x10];
        Array.Copy(Encoding.UTF8.GetBytes(ivVal.PadRight(buffer2.Length)), buffer2, buffer2.Length);
        Rijndael rijndael = Rijndael.Create();
        try
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateEncryptor(destinationArray, buffer2), CryptoStreamMode.Write))
                {
                    stream2.Write(data, 0, data.Length);
                    stream2.FlushFinalBlock();
                    return stream.ToArray();
                }
            }
        }
        catch
        {
            buffer3 = null;
        }
        return buffer3;
    }

    public static string AesStr(this string source, string keyVal, string ivVal)
    {
        string str;
        Encoding encoding = Encoding.UTF8;
        byte[] rgbKey = keyVal.FormatByte(encoding);
        byte[] rgbIV = ivVal.FormatByte(encoding);
        byte[] bytes = encoding.GetBytes(source);
        Rijndael rijndael = Rijndael.Create();
        using (MemoryStream stream = new MemoryStream())
        {
            using (CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
            {
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                str = Convert.ToBase64String(stream.ToArray());
            }
        }
        rijndael.Clear();
        return str;
    }

    public static string Base64(this string source)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(source);
        return Convert.ToBase64String(bytes, 0, bytes.Length);
    }

    private static string Bytes2Str(this IEnumerable<byte> source, string formatStr = "{0:X2}")
    {
        StringBuilder builder = new StringBuilder();
        foreach (byte num in source)
        {
            builder.AppendFormat(formatStr, num);
        }
        return builder.ToString();
    }

    public static string Des(this string source, string keyVal, string ivVal)
    {
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider {
                Key = Encoding.ASCII.GetBytes((keyVal.Length <= 8) ? keyVal : keyVal.Substring(0, 8)),
                IV = Encoding.ASCII.GetBytes((ivVal.Length <= 8) ? ivVal : ivVal.Substring(0, 8))
            };
            return BitConverter.ToString(provider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
        }
        catch
        {
            return "转换出错！";
        }
    }

    public static string Des3(this string source, string keyVal)
    {
        string str;
        try
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider {
                Key = keyVal.FormatByte(Encoding.UTF8),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(source);
                try
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        stream2.Write(bytes, 0, bytes.Length);
                        stream2.FlushFinalBlock();
                    }
                    str = stream.ToArray().Bytes2Str("{0:X2}");
                }
                catch
                {
                    str = source;
                }
            }
        }
        catch
        {
            str = "TripleDES加密出现错误";
        }
        return str;
    }

    private static byte[] FormatByte(this string strVal, Encoding encoding) => 
        encoding.GetBytes(strVal.Base64().Substring(0, 0x10).ToUpper());

    private static string HashAlgorithmBase(HashAlgorithm hashAlgorithmObj, string source, Encoding encoding)
    {
        byte[] bytes = encoding.GetBytes(source);
        return hashAlgorithmObj.ComputeHash(bytes).Bytes2Str("{0:X2}");
    }

    public static string HmacMd5(this string source, string keyVal)
    {
        if (source.IsEmpty())
        {
            return null;
        }
        Encoding encoding = Encoding.UTF8;
        HMACMD5 hashAlgorithmObj = new HMACMD5(encoding.GetBytes(keyVal));
        return HashAlgorithmBase(hashAlgorithmObj, source, encoding);
    }

    public static string HmacRipeMd160(this string source, string keyVal)
    {
        if (source.IsEmpty())
        {
            return null;
        }
        Encoding encoding = Encoding.UTF8;
        HMACRIPEMD160 hashAlgorithmObj = new HMACRIPEMD160(encoding.GetBytes(keyVal));
        return HashAlgorithmBase(hashAlgorithmObj, source, encoding);
    }

    public static string HmacSha1(this string source, string keyVal)
    {
        if (source.IsEmpty())
        {
            return null;
        }
        Encoding encoding = Encoding.UTF8;
        HMACSHA1 hashAlgorithmObj = new HMACSHA1(encoding.GetBytes(keyVal));
        return HashAlgorithmBase(hashAlgorithmObj, source, encoding);
    }

    public static string HmacSha256(this string source, string keyVal)
    {
        if (source.IsEmpty())
        {
            return null;
        }
        Encoding encoding = Encoding.UTF8;
        HMACSHA256 hashAlgorithmObj = new HMACSHA256(encoding.GetBytes(keyVal));
        return HashAlgorithmBase(hashAlgorithmObj, source, encoding);
    }

    public static string HmacSha384(this string source, string keyVal)
    {
        if (source.IsEmpty())
        {
            return null;
        }
        Encoding encoding = Encoding.UTF8;
        HMACSHA384 hashAlgorithmObj = new HMACSHA384(encoding.GetBytes(keyVal));
        return HashAlgorithmBase(hashAlgorithmObj, source, encoding);
    }

    public static string HmacSha512(this string source, string keyVal)
    {
        if (source.IsEmpty())
        {
            return null;
        }
        Encoding encoding = Encoding.UTF8;
        HMACSHA512 hashAlgorithmObj = new HMACSHA512(encoding.GetBytes(keyVal));
        return HashAlgorithmBase(hashAlgorithmObj, source, encoding);
    }

    public static bool IsEmpty(this string value) => 
        string.IsNullOrEmpty(value);

    public static string Md532(this string source)
    {
        if (source.IsEmpty())
        {
            return null;
        }
        Encoding encoding = Encoding.UTF8;
        return HashAlgorithmBase(MD5.Create(), source, encoding);
    }

    public static string Md532Salt(this string source, string salt) => 
        (!salt.IsEmpty() ? (source + "『" + salt + "』").Md532() : source.Md532());

    public static string Rsa(this string source)
    {
        RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
        provider.FromXmlString("pubKey");
        return Convert.ToBase64String(provider.Encrypt(Encoding.UTF8.GetBytes(source), true));
    }

    public static string Sha1(this string source)
    {
        if (source.IsEmpty())
        {
            return null;
        }
        Encoding encoding = Encoding.UTF8;
        SHA1 hashAlgorithmObj = new SHA1CryptoServiceProvider();
        return HashAlgorithmBase(hashAlgorithmObj, source, encoding);
    }

    public static string Sha256(this string source)
    {
        if (source.IsEmpty())
        {
            return null;
        }
        Encoding encoding = Encoding.UTF8;
        SHA256 hashAlgorithmObj = new SHA256Managed();
        return HashAlgorithmBase(hashAlgorithmObj, source, encoding);
    }

    public static string Sha512(this string source)
    {
        if (source.IsEmpty())
        {
            return null;
        }
        Encoding encoding = Encoding.UTF8;
        SHA512 hashAlgorithmObj = new SHA512Managed();
        return HashAlgorithmBase(hashAlgorithmObj, source, encoding);
    }

    private static byte[] Str2Bytes(this string source)
    {
        source = source.Replace(" ", string.Empty);
        byte[] buffer = new byte[source.Length / 2];
        for (int i = 0; i < source.Length; i += 2)
        {
            buffer[i / 2] = Convert.ToByte(source.Substring(i, 2), 0x10);
        }
        return buffer;
    }

    public static byte[] UnAesByte(this byte[] data, string keyVal, string ivVal)
    {
        byte[] buffer3;
        byte[] destinationArray = new byte[0x20];
        Array.Copy(Encoding.UTF8.GetBytes(keyVal.PadRight(destinationArray.Length)), destinationArray, destinationArray.Length);
        byte[] buffer2 = new byte[0x10];
        Array.Copy(Encoding.UTF8.GetBytes(ivVal.PadRight(buffer2.Length)), buffer2, buffer2.Length);
        Rijndael rijndael = Rijndael.Create();
        try
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateDecryptor(destinationArray, buffer2), CryptoStreamMode.Read))
                {
                    using (MemoryStream stream3 = new MemoryStream())
                    {
                        int num;
                        byte[] buffer = new byte[0x400];
                        while ((num = stream2.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            stream3.Write(buffer, 0, num);
                        }
                        return stream3.ToArray();
                    }
                }
            }
        }
        catch
        {
            buffer3 = null;
        }
        return buffer3;
    }

    public static string UnAesStr(this string source, string keyVal, string ivVal)
    {
        string str;
        Encoding encoding = Encoding.UTF8;
        byte[] rgbKey = keyVal.FormatByte(encoding);
        byte[] rgbIV = ivVal.FormatByte(encoding);
        byte[] buffer = Convert.FromBase64String(source);
        Rijndael rijndael = Rijndael.Create();
        using (MemoryStream stream = new MemoryStream())
        {
            using (CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
            {
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                str = encoding.GetString(stream.ToArray());
            }
        }
        rijndael.Clear();
        return str;
    }

    public static string UnBase64(this string source)
    {
        byte[] bytes = Convert.FromBase64String(source);
        return Encoding.UTF8.GetString(bytes);
    }

    public static string UnDes(this string source, string keyVal, string ivVal)
    {
        try
        {
            string[] strArray = source.Split("-".ToCharArray());
            byte[] inputBuffer = new byte[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                inputBuffer[i] = byte.Parse(strArray[i], NumberStyles.HexNumber);
            }
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider {
                Key = Encoding.ASCII.GetBytes((keyVal.Length <= 8) ? keyVal : keyVal.Substring(0, 8)),
                IV = Encoding.ASCII.GetBytes((ivVal.Length <= 8) ? ivVal : ivVal.Substring(0, 8))
            };
            byte[] bytes = provider.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return "解密出错！";
        }
    }

    public static string UnDes3(this string source, string keyVal)
    {
        string str;
        try
        {
            byte[] buffer = source.Str2Bytes();
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider {
                Key = keyVal.FormatByte(Encoding.UTF8),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    stream2.Write(buffer, 0, buffer.Length);
                    stream2.FlushFinalBlock();
                    stream2.Close();
                    stream.Close();
                    str = Encoding.UTF8.GetString(stream.ToArray());
                }
            }
        }
        catch
        {
            str = "TripleDES解密出现错误";
        }
        return str;
    }

    public static string UnRsa(this string source)
    {
        RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
        provider.FromXmlString("priKey");
        byte[] bytes = provider.Decrypt(Convert.FromBase64String(source), true);
        return Encoding.UTF8.GetString(bytes);
    }
}

