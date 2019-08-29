using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SpineSkeletonFlipClip : PlayableAsset, ITimelineClipAsset
{
    public SpineSkeletonFlipBehaviour template = new SpineSkeletonFlipBehaviour();

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) => 
        ((Playable) ScriptPlayable<SpineSkeletonFlipBehaviour>.Create(graph, this.template, 0));

    public ClipCaps clipCaps =>
        ClipCaps.None;
}

