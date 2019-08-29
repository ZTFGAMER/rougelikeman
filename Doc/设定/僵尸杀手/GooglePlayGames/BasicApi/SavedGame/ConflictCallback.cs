namespace GooglePlayGames.BasicApi.SavedGame
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void ConflictCallback(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData);
}

