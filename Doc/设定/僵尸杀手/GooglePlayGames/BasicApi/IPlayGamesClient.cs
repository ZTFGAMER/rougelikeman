namespace GooglePlayGames.BasicApi
{
    using GooglePlayGames.BasicApi.Events;
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.BasicApi.SavedGame;
    using GooglePlayGames.BasicApi.Video;
    using System;
    using UnityEngine.SocialPlatforms;

    public interface IPlayGamesClient
    {
        void Authenticate(Action<bool, string> callback, bool silent);
        Achievement GetAchievement(string achievementId);
        void GetAnotherServerAuthCode(bool reAuthenticateIfNeeded, Action<string> callback);
        IntPtr GetApiClient();
        IEventsClient GetEventsClient();
        IUserProfile[] GetFriends();
        string GetIdToken();
        void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback);
        IRealTimeMultiplayerClient GetRtmpClient();
        ISavedGameClient GetSavedGameClient();
        string GetServerAuthCode();
        ITurnBasedMultiplayerClient GetTbmpClient();
        string GetUserDisplayName();
        string GetUserEmail();
        string GetUserId();
        string GetUserImageUrl();
        IVideoClient GetVideoClient();
        void IncrementAchievement(string achievementId, int steps, Action<bool> successOrFailureCalllback);
        bool IsAuthenticated();
        int LeaderboardMaxResults();
        void LoadAchievements(Action<Achievement[]> callback);
        void LoadFriends(Action<bool> callback);
        void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback);
        void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback);
        void LoadUsers(string[] userIds, Action<IUserProfile[]> callback);
        void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate);
        void RevealAchievement(string achievementId, Action<bool> successOrFailureCalllback);
        void SetGravityForPopups(Gravity gravity);
        void SetStepsAtLeast(string achId, int steps, Action<bool> callback);
        void ShowAchievementsUI(Action<UIStatus> callback);
        void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback);
        void SignOut();
        void SubmitScore(string leaderboardId, long score, Action<bool> successOrFailureCalllback);
        void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> successOrFailureCalllback);
        void UnlockAchievement(string achievementId, Action<bool> successOrFailureCalllback);
    }
}

