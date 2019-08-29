namespace LitJson
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void ExporterFunc<T>(T obj, JsonWriter writer);
}

