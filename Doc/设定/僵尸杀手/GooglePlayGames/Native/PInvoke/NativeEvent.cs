namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.Events;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeEvent : BaseReferenceHolder, IEvent
    {
        internal NativeEvent(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            Event.Event_Dispose(selfPointer);
        }

        public override string ToString()
        {
            if (base.IsDisposed())
            {
                return "[NativeEvent: DELETED]";
            }
            return $"[NativeEvent: Id={this.Id}, Name={this.Name}, Description={this.Description}, ImageUrl={this.ImageUrl}, CurrentCount={this.CurrentCount}, Visibility={this.Visibility}]";
        }

        public string Id =>
            PInvokeUtilities.OutParamsToString((out_string, out_size) => Event.Event_Id(base.SelfPtr(), out_string, out_size));

        public string Name =>
            PInvokeUtilities.OutParamsToString((out_string, out_size) => Event.Event_Name(base.SelfPtr(), out_string, out_size));

        public string Description =>
            PInvokeUtilities.OutParamsToString((out_string, out_size) => Event.Event_Description(base.SelfPtr(), out_string, out_size));

        public string ImageUrl =>
            PInvokeUtilities.OutParamsToString((out_string, out_size) => Event.Event_ImageUrl(base.SelfPtr(), out_string, out_size));

        public ulong CurrentCount =>
            Event.Event_Count(base.SelfPtr());

        public GooglePlayGames.BasicApi.Events.EventVisibility Visibility
        {
            get
            {
                Types.EventVisibility visibility = Event.Event_Visibility(base.SelfPtr());
                if (visibility != Types.EventVisibility.HIDDEN)
                {
                    if (visibility != Types.EventVisibility.REVEALED)
                    {
                        throw new InvalidOperationException("Unknown visibility: " + visibility);
                    }
                    return GooglePlayGames.BasicApi.Events.EventVisibility.Revealed;
                }
                return GooglePlayGames.BasicApi.Events.EventVisibility.Hidden;
            }
        }
    }
}

