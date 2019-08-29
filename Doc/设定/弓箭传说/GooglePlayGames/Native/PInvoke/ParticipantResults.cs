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
            ParticipantResults.ParticipantResults_Dispose(selfPointer);
        }

        internal bool HasResultsForParticipant(string participantId) => 
            ParticipantResults.ParticipantResults_HasResultsForParticipant(base.SelfPtr(), participantId);

        internal uint PlacingForParticipant(string participantId) => 
            ParticipantResults.ParticipantResults_PlaceForParticipant(base.SelfPtr(), participantId);

        internal Types.MatchResult ResultsForParticipant(string participantId) => 
            ParticipantResults.ParticipantResults_MatchResultForParticipant(base.SelfPtr(), participantId);

        internal ParticipantResults WithResult(string participantId, uint placing, Types.MatchResult result) => 
            new ParticipantResults(ParticipantResults.ParticipantResults_WithResult(base.SelfPtr(), participantId, placing, result));
    }
}

