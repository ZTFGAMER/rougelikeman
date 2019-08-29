using System;
using System.Collections.Generic;

public abstract class PackageConfig
{
    protected PackageConfig()
    {
    }

    public abstract string Name { get; }

    public abstract string Version { get; }

    public abstract Dictionary<Platform, string> NetworkSdkVersions { get; }

    public abstract Dictionary<Platform, string> AdapterClassNames { get; }

    public string NetworkSdkVersion =>
        this.NetworkSdkVersions[Platform.ANDROID];

    public string AdapterClassName =>
        this.AdapterClassNames[Platform.ANDROID];

    public enum Platform
    {
        ANDROID,
        IOS
    }
}

