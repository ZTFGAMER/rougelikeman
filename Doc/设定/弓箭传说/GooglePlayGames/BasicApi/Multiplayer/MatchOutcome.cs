namespace GooglePlayGames.BasicApi.Multiplayer
{
    using System;
    using System.Collections.Generic;

    public class MatchOutcome
    {
        public const uint PlacementUnset = 0;
        private List<string> mParticipantIds = new List<string>();
        private Dictionary<string, uint> mPlacements = new Dictionary<string, uint>();
        private Dictionary<string, ParticipantResult> mResults = new Dictionary<string, ParticipantResult>();

        public uint GetPlacementFor(string participantId) => 
            (!this.mPlacements.ContainsKey(participantId) ? 0 : this.mPlacements[participantId]);

        public ParticipantResult GetResultFor(string participantId) => 
            (!this.mResults.ContainsKey(participantId) ? ParticipantResult.Unset : this.mResults[participantId]);

        public void SetParticipantResult(string participantId, ParticipantResult result)
        {
            this.SetParticipantResult(participantId, result, 0);
        }

        public void SetParticipantResult(string participantId, uint placement)
        {
            this.SetParticipantResult(participantId, ParticipantResult.Unset, placement);
        }

        public void SetParticipantResult(string participantId, ParticipantResult result, uint placement)
        {
            if (!this.mParticipantIds.Contains(participantId))
            {
                this.mParticipantIds.Add(participantId);
            }
            this.mPlacements[participantId] = placement;
            this.mResults[participantId] = result;
        }

        public override string ToString()
        {
            string str = "[MatchOutcome";
            foreach (string str2 in this.mParticipantIds)
            {
                str = str + $" {str2}->({this.GetResultFor(str2)},{this.GetPlacementFor(str2)})";
            }
            return (str + "]");
        }

        public List<string> ParticipantIds =>
            this.mParticipantIds;

        public enum ParticipantResult
        {
            Unset = -1,
            None = 0,
            Win = 1,
            Loss = 2,
            Tie = 3
        }
    }
}

