namespace Spine
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class AnimationStateData
    {
        internal Spine.SkeletonData skeletonData;
        private readonly Dictionary<AnimationPair, float> animationToMixTime = new Dictionary<AnimationPair, float>(AnimationPairComparer.Instance);
        internal float defaultMix;

        public AnimationStateData(Spine.SkeletonData skeletonData)
        {
            if (skeletonData == null)
            {
                throw new ArgumentException("skeletonData cannot be null.", "skeletonData");
            }
            this.skeletonData = skeletonData;
        }

        public float GetMix(Animation from, Animation to)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from", "from cannot be null.");
            }
            if (to == null)
            {
                throw new ArgumentNullException("to", "to cannot be null.");
            }
            AnimationPair key = new AnimationPair(from, to);
            if (this.animationToMixTime.TryGetValue(key, out float num))
            {
                return num;
            }
            return this.defaultMix;
        }

        public void SetMix(Animation from, Animation to, float duration)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from", "from cannot be null.");
            }
            if (to == null)
            {
                throw new ArgumentNullException("to", "to cannot be null.");
            }
            AnimationPair key = new AnimationPair(from, to);
            this.animationToMixTime.Remove(key);
            this.animationToMixTime.Add(key, duration);
        }

        public void SetMix(string fromName, string toName, float duration)
        {
            Animation from = this.skeletonData.FindAnimation(fromName);
            if (from == null)
            {
                throw new ArgumentException("Animation not found: " + fromName, "fromName");
            }
            Animation to = this.skeletonData.FindAnimation(toName);
            if (to == null)
            {
                throw new ArgumentException("Animation not found: " + toName, "toName");
            }
            this.SetMix(from, to, duration);
        }

        public Spine.SkeletonData SkeletonData =>
            this.skeletonData;

        public float DefaultMix
        {
            get => 
                this.defaultMix;
            set => 
                (this.defaultMix = value);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AnimationPair
        {
            public readonly Animation a1;
            public readonly Animation a2;
            public AnimationPair(Animation a1, Animation a2)
            {
                this.a1 = a1;
                this.a2 = a2;
            }

            public override string ToString() => 
                (this.a1.name + "->" + this.a2.name);
        }

        public class AnimationPairComparer : IEqualityComparer<AnimationStateData.AnimationPair>
        {
            public static readonly AnimationStateData.AnimationPairComparer Instance = new AnimationStateData.AnimationPairComparer();

            bool IEqualityComparer<AnimationStateData.AnimationPair>.Equals(AnimationStateData.AnimationPair x, AnimationStateData.AnimationPair y) => 
                (object.ReferenceEquals(x.a1, y.a1) && object.ReferenceEquals(x.a2, y.a2));

            int IEqualityComparer<AnimationStateData.AnimationPair>.GetHashCode(AnimationStateData.AnimationPair obj)
            {
                int hashCode = obj.a1.GetHashCode();
                return (((hashCode << 5) + hashCode) ^ obj.a2.GetHashCode());
            }
        }
    }
}

