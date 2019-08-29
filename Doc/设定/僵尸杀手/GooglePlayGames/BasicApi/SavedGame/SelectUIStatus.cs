namespace GooglePlayGames.BasicApi.SavedGame
{
    using System;

    public enum SelectUIStatus
    {
        SavedGameSelected = 1,
        UserClosedUI = 2,
        InternalError = -1,
        TimeoutError = -2,
        AuthenticationError = -3,
        BadInputError = -4
    }
}

