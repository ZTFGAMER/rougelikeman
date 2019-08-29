namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class ParticipantResults : BaseReferenceHolder
    {
        internal ParticipantResults(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.ParticipantResults.ParticipantResults_Dispose(selfPointer);
        }

        internal bool HasResultsForParticipant(string participantId) => 
            GooglePlayGames.Native.Cwrapper.ParticipantResults.ParticipantResults_HasResultsForParticipant(base.SelfPtr(), participantId);

        internal uint PlacingForParticipant(string participantId) => 
            GooglePlayGames.Native.Cwrapper.ParticipantResults.ParticipantResults_PlaceForParticipant(base.SelfPtr(), participantId);

        internal Types.MatchResult ResultsForParticipant(string participantId) => 
            GooglePlayGames.Native.Cwrapper.ParticipantResults.ParticipantResults_MatchResultForParticipant(base.SelfPtr(), participantId);

        internal GooglePlayGames.Native.PInvoke.ParticipantResults WithResult(string participantId, uint placing, Types.MatchResult result) => 
            new GooglePlayGames.Native.PInvoke.ParticipantResults(GooglePlayGames.Native.Cwrapper.ParticipantResults.ParticipantResults_WithResult(base.SelfPtr(), participantId, placing, result));
    }
}

