namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.BasicApi.Events;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    internal class NativeEventClient : IEventsClient
    {
        private readonly EventManager mEventManager;

        internal NativeEventClient(EventManager manager)
        {
            this.mEventManager = Misc.CheckNotNull<EventManager>(manager);
        }

        public void FetchAllEvents(DataSource source, Action<ResponseStatus, List<IEvent>> callback)
        {
            <FetchAllEvents>c__AnonStorey0 storey = new <FetchAllEvents>c__AnonStorey0 {
                callback = callback
            };
            Misc.CheckNotNull<Action<ResponseStatus, List<IEvent>>>(storey.callback);
            storey.callback = CallbackUtils.ToOnGameThread<ResponseStatus, List<IEvent>>(storey.callback);
            this.mEventManager.FetchAll(ConversionUtils.AsDataSource(source), new Action<EventManager.FetchAllResponse>(storey.<>m__0));
        }

        public void FetchEvent(DataSource source, string eventId, Action<ResponseStatus, IEvent> callback)
        {
            <FetchEvent>c__AnonStorey1 storey = new <FetchEvent>c__AnonStorey1 {
                callback = callback
            };
            Misc.CheckNotNull<string>(eventId);
            Misc.CheckNotNull<Action<ResponseStatus, IEvent>>(storey.callback);
            this.mEventManager.Fetch(ConversionUtils.AsDataSource(source), eventId, new Action<EventManager.FetchResponse>(storey.<>m__0));
        }

        public void IncrementEvent(string eventId, uint stepsToIncrement)
        {
            Misc.CheckNotNull<string>(eventId);
            this.mEventManager.Increment(eventId, stepsToIncrement);
        }

        [CompilerGenerated]
        private sealed class <FetchAllEvents>c__AnonStorey0
        {
            internal Action<ResponseStatus, List<IEvent>> callback;

            internal void <>m__0(EventManager.FetchAllResponse response)
            {
                ResponseStatus status = ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
                if (!response.RequestSucceeded())
                {
                    this.callback(status, new List<IEvent>());
                }
                else
                {
                    this.callback(status, response.Data().Cast<IEvent>().ToList<IEvent>());
                }
            }
        }

        [CompilerGenerated]
        private sealed class <FetchEvent>c__AnonStorey1
        {
            internal Action<ResponseStatus, IEvent> callback;

            internal void <>m__0(EventManager.FetchResponse response)
            {
                ResponseStatus status = ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
                if (!response.RequestSucceeded())
                {
                    this.callback(status, null);
                }
                else
                {
                    this.callback(status, response.Data());
                }
            }
        }
    }
}

