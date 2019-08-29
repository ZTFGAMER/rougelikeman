using System;
using UnityEngine;

public class UniWebViewLogger
{
    private static UniWebViewLogger instance;
    private Level level;

    private UniWebViewLogger(Level level)
    {
        this.level = level;
    }

    public void Critical(string message)
    {
        this.Log(Level.Critical, message);
    }

    public void Debug(string message)
    {
        this.Log(Level.Debug, message);
    }

    public void Info(string message)
    {
        this.Log(Level.Info, message);
    }

    private void Log(Level level, string message)
    {
        if (level >= this.LogLevel)
        {
            string str = "<UniWebView> " + message;
            if (level == Level.Critical)
            {
                UnityEngine.Debug.LogError(str);
            }
            else
            {
                UnityEngine.Debug.Log(str);
            }
        }
    }

    public void Verbose(string message)
    {
        this.Log(Level.Verbose, message);
    }

    public Level LogLevel
    {
        get => 
            this.level;
        set
        {
            this.Log(Level.Off, "Setting UniWebView logger level to: " + value);
            this.level = value;
            UniWebViewInterface.SetLogLevel((int) value);
        }
    }

    public static UniWebViewLogger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UniWebViewLogger(Level.Critical);
            }
            return instance;
        }
    }

    public enum Level
    {
        Verbose = 0,
        Debug = 10,
        Info = 20,
        Critical = 80,
        Off = 0x63
    }
}

