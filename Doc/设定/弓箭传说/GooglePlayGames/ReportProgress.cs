namespace GooglePlayGames
{
    using System;
    using System.Runtime.CompilerServices;

    internal delegate void ReportProgress(string id, double progress, Action<bool> callback);
}

