using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct UniWebViewMessage
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string <RawMessage>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string <Scheme>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string <Path>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Dictionary<string, string> <Args>k__BackingField;
    public UniWebViewMessage(string rawMessage)
    {
        this = new UniWebViewMessage();
        UniWebViewLogger.Instance.Debug("Try to parse raw message: " + rawMessage);
        this.RawMessage = WWW.UnEscapeURL(rawMessage);
        string[] separator = new string[] { "://" };
        string[] strArray = rawMessage.Split(separator, StringSplitOptions.None);
        if (strArray.Length >= 2)
        {
            this.Scheme = strArray[0];
            UniWebViewLogger.Instance.Debug("Get scheme: " + this.Scheme);
            string str = string.Empty;
            for (int i = 1; i < strArray.Length; i++)
            {
                str = str + strArray[i];
            }
            UniWebViewLogger.Instance.Verbose("Build path and args string: " + str);
            char[] chArray1 = new char[] { "?"[0] };
            string[] strArray2 = str.Split(chArray1);
            char[] trimChars = new char[] { '/' };
            this.Path = WWW.UnEscapeURL(strArray2[0].TrimEnd(trimChars));
            this.Args = new Dictionary<string, string>();
            if (strArray2.Length > 1)
            {
                char[] chArray3 = new char[] { "&"[0] };
                foreach (string str2 in strArray2[1].Split(chArray3))
                {
                    char[] chArray4 = new char[] { "="[0] };
                    string[] strArray4 = str2.Split(chArray4);
                    if (strArray4.Length > 1)
                    {
                        string key = WWW.UnEscapeURL(strArray4[0]);
                        if (this.Args.ContainsKey(key))
                        {
                            string str4 = this.Args[key];
                            this.Args[key] = str4 + "," + WWW.UnEscapeURL(strArray4[1]);
                        }
                        else
                        {
                            this.Args[key] = WWW.UnEscapeURL(strArray4[1]);
                        }
                        UniWebViewLogger.Instance.Debug("Get arg, key: " + key + " value: " + this.Args[key]);
                    }
                }
            }
        }
        else
        {
            UniWebViewLogger.Instance.Critical("Bad url scheme. Can not be parsed to UniWebViewMessage: " + rawMessage);
        }
    }

    public string RawMessage { get; private set; }
    public string Scheme { get; private set; }
    public string Path { get; private set; }
    public Dictionary<string, string> Args { get; private set; }
}

