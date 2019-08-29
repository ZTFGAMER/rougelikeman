namespace GooglePlayGames.BasicApi.SavedGame
{
    using System;

    public interface ISavedGameMetadata
    {
        bool IsOpen { get; }

        string Filename { get; }

        string Description { get; }

        string CoverImageURL { get; }

        TimeSpan TotalTimePlayed { get; }

        DateTime LastModifiedTimestamp { get; }
    }
}

