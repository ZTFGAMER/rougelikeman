namespace LitJson
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate TValue ImporterFunc<TJson, TValue>(TJson input);
}

