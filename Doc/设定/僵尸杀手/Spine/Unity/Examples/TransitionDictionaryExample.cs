namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public sealed class TransitionDictionaryExample : MonoBehaviour
    {
        [SerializeField]
        private List<SerializedEntry> transitions = new List<SerializedEntry>();
        private readonly Dictionary<AnimationStateData.AnimationPair, Spine.Animation> dictionary = new Dictionary<AnimationStateData.AnimationPair, Spine.Animation>();

        public Spine.Animation GetTransition(Spine.Animation from, Spine.Animation to)
        {
            this.dictionary.TryGetValue(new AnimationStateData.AnimationPair(from, to), out Spine.Animation animation);
            return animation;
        }

        private void Start()
        {
            this.dictionary.Clear();
            foreach (SerializedEntry entry in this.transitions)
            {
                this.dictionary.Add(new AnimationStateData.AnimationPair(entry.from.Animation, entry.to.Animation), entry.transition.Animation);
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct SerializedEntry
        {
            public AnimationReferenceAsset from;
            public AnimationReferenceAsset to;
            public AnimationReferenceAsset transition;
        }
    }
}

