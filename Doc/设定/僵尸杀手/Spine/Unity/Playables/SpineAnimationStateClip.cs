namespace Spine.Unity.Playables
{
    using System;
    using UnityEngine;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;

    [Serializable]
    public class SpineAnimationStateClip : PlayableAsset, ITimelineClipAsset
    {
        public SpineAnimationStateBehaviour template = new SpineAnimationStateBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<SpineAnimationStateBehaviour> playable = ScriptPlayable<SpineAnimationStateBehaviour>.Create(graph, this.template, 0);
            playable.GetBehaviour();
            return (Playable) playable;
        }

        public ClipCaps clipCaps =>
            ClipCaps.None;
    }
}

