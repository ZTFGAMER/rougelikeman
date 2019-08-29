using System;
using System.Security.Cryptography;

public class NetEncrypt
{
    public static string DesDecrypt(string text, string password) => 
        text.UnAesStr(password, password);

    public static string Encrypt_UTF8(string text, string password) => 
        text.AesStr(password, password);

    public static void EncryptTest()
    {
        string text = "test string";
        string privateKey = string.Empty;
        string publicKey = string.Empty;
        RSAGenerateKey(ref privateKey, ref publicKey);
        Debugger.Log("加密前：" + text);
        Debugger.Log("解密后：" + DesDecrypt(Encrypt_UTF8(text, publicKey), publicKey));
    }

    public static void RSAGenerateKey(ref string privateKey, ref string publicKey)
    {
        RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
        privateKey = provider.ToXmlString(true);
        publicKey = provider.ToXmlString(false);
    }
}

