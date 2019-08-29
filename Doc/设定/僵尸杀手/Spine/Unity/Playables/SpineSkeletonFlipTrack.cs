namespace Spine.Unity.Playables
{
    using System;
    using UnityEngine;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;

    [TrackColor(0.855f, 0.8623f, 0.87f), TrackClipType(typeof(SpineSkeletonFlipClip)), TrackBindingType(typeof(SpinePlayableHandleBase))]
    public class SpineSkeletonFlipTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) => 
            ((Playable) ScriptPlayable<SpineSkeletonFlipMixerBehaviour>.Create(graph, inputCount));

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            base.GatherProperties(director, driver);
        }
    }
}

