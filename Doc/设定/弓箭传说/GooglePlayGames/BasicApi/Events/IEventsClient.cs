namespace GooglePlayGames.BasicApi.Events
{
    using GooglePlayGames.BasicApi;
    using System;

    public interface IEventsClient
    {
        void FetchAllEvents(DataSource source, Action<ResponseStatus, List<IEvent>> callback);
        void FetchEvent(DataSource source, string eventId, Action<ResponseStatus, IEvent> callback);
        void IncrementEvent(string eventId, uint stepsToIncrement);
    }
}

