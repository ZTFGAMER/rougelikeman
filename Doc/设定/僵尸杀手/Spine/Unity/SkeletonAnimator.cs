namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class SkeletonAnimator : SkeletonRenderer, ISkeletonAnimation
    {
        [SerializeField]
        protected MecanimTranslator translator;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        protected event UpdateBonesDelegate _UpdateComplete;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        protected event UpdateBonesDelegate _UpdateLocal;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        protected event UpdateBonesDelegate _UpdateWorld;

        public event UpdateBonesDelegate UpdateComplete
        {
            add
            {
                this._UpdateComplete += value;
            }
            remove
            {
                this._UpdateComplete -= value;
            }
        }

        public event UpdateBonesDelegate UpdateLocal
        {
            add
            {
                this._UpdateLocal += value;
            }
            remove
            {
                this._UpdateLocal -= value;
            }
        }

        public event UpdateBonesDelegate UpdateWorld
        {
            add
            {
                this._UpdateWorld += value;
            }
            remove
            {
                this._UpdateWorld -= value;
            }
        }

        public override void Initialize(bool overwrite)
        {
            if (!base.valid || overwrite)
            {
                base.Initialize(overwrite);
                if (base.valid)
                {
                    if (this.translator == null)
                    {
                        this.translator = new MecanimTranslator();
                    }
                    this.translator.Initialize(base.GetComponent<Animator>(), base.skeletonDataAsset);
                }
            }
        }

        public void Update()
        {
            if (base.valid)
            {
                this.translator.Apply(base.skeleton);
                if (this._UpdateLocal != null)
                {
                    this._UpdateLocal(this);
                }
                base.skeleton.UpdateWorldTransform();
                if (this._UpdateWorld != null)
                {
                    this._UpdateWorld(this);
                    base.skeleton.UpdateWorldTransform();
                }
                if (this._UpdateComplete != null)
                {
                    this._UpdateComplete(this);
                }
            }
        }

        public MecanimTranslator Translator =>
            this.translator;

        [Serializable]
        public class MecanimTranslator
        {
            public bool autoReset = true;
            public MixMode[] layerMixModes = new MixMode[0];
            private readonly Dictionary<int, Spine.Animation> animationTable = new Dictionary<int, Spine.Animation>(IntEqualityComparer.Instance);
            private readonly Dictionary<AnimationClip, int> clipNameHashCodeTable = new Dictionary<AnimationClip, int>(AnimationClipEqualityComparer.Instance);
            private readonly List<Spine.Animation> previousAnimations = new List<Spine.Animation>();
            private readonly List<AnimatorClipInfo> clipInfoCache = new List<AnimatorClipInfo>();
            private readonly List<AnimatorClipInfo> nextClipInfoCache = new List<AnimatorClipInfo>();
            private UnityEngine.Animator animator;

            private static float AnimationTime(float normalizedTime, float clipLength, bool reversed)
            {
                if (reversed)
                {
                    normalizedTime = ((1f - normalizedTime) + ((int) normalizedTime)) + ((int) normalizedTime);
                }
                return (normalizedTime * clipLength);
            }

            private static float AnimationTime(float normalizedTime, float clipLength, bool loop, bool reversed)
            {
                if (reversed)
                {
                    normalizedTime = ((1f - normalizedTime) + ((int) normalizedTime)) + ((int) normalizedTime);
                }
                float num = normalizedTime * clipLength;
                if (loop)
                {
                    return num;
                }
                return (((clipLength - num) >= 0.03333334f) ? num : clipLength);
            }

            public void Apply(Skeleton skeleton)
            {
                if (this.layerMixModes.Length < this.animator.layerCount)
                {
                    Array.Resize<MixMode>(ref this.layerMixModes, this.animator.layerCount);
                }
                if (this.autoReset)
                {
                    List<Spine.Animation> previousAnimations = this.previousAnimations;
                    int num = 0;
                    int count = previousAnimations.Count;
                    while (num < count)
                    {
                        previousAnimations[num].SetKeyedItemsToSetupPose(skeleton);
                        num++;
                    }
                    previousAnimations.Clear();
                    int num3 = 0;
                    int num4 = this.animator.layerCount;
                    while (num3 < num4)
                    {
                        float num5 = (num3 != 0) ? this.animator.GetLayerWeight(num3) : 1f;
                        if (num5 > 0f)
                        {
                            bool flag = this.animator.GetNextAnimatorStateInfo(num3).fullPathHash != 0;
                            this.GetAnimatorClipInfos(num3, out int num6, out int num7, out IList<AnimatorClipInfo> list2, out IList<AnimatorClipInfo> list3);
                            for (int i = 0; i < num6; i++)
                            {
                                AnimatorClipInfo info2 = list2[i];
                                float num9 = info2.weight * num5;
                                if (num9 != 0f)
                                {
                                    previousAnimations.Add(this.GetAnimation(info2.clip));
                                }
                            }
                            if (flag)
                            {
                                for (int j = 0; j < num7; j++)
                                {
                                    AnimatorClipInfo info3 = list3[j];
                                    float num11 = info3.weight * num5;
                                    if (num11 != 0f)
                                    {
                                        previousAnimations.Add(this.GetAnimation(info3.clip));
                                    }
                                }
                            }
                        }
                        num3++;
                    }
                }
                int layerIndex = 0;
                int layerCount = this.animator.layerCount;
                while (layerIndex < layerCount)
                {
                    float num14 = (layerIndex != 0) ? this.animator.GetLayerWeight(layerIndex) : 1f;
                    AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(layerIndex);
                    AnimatorStateInfo nextAnimatorStateInfo = this.animator.GetNextAnimatorStateInfo(layerIndex);
                    bool flag2 = nextAnimatorStateInfo.fullPathHash != 0;
                    this.GetAnimatorClipInfos(layerIndex, out int num15, out int num16, out IList<AnimatorClipInfo> list4, out IList<AnimatorClipInfo> list5);
                    MixMode mode = this.layerMixModes[layerIndex];
                    if (mode == MixMode.AlwaysMix)
                    {
                        for (int i = 0; i < num15; i++)
                        {
                            AnimatorClipInfo info6 = list4[i];
                            float alpha = info6.weight * num14;
                            if (alpha != 0f)
                            {
                                float time = AnimationTime(currentAnimatorStateInfo.normalizedTime, info6.clip.length, currentAnimatorStateInfo.loop, currentAnimatorStateInfo.speed < 0f);
                                this.GetAnimation(info6.clip).Apply(skeleton, 0f, time, currentAnimatorStateInfo.loop, null, alpha, MixPose.Current, MixDirection.In);
                            }
                        }
                        if (flag2)
                        {
                            for (int j = 0; j < num16; j++)
                            {
                                AnimatorClipInfo info7 = list5[j];
                                float alpha = info7.weight * num14;
                                if (alpha != 0f)
                                {
                                    float time = AnimationTime(nextAnimatorStateInfo.normalizedTime, info7.clip.length, nextAnimatorStateInfo.speed < 0f);
                                    this.GetAnimation(info7.clip).Apply(skeleton, 0f, time, nextAnimatorStateInfo.loop, null, alpha, MixPose.Current, MixDirection.In);
                                }
                            }
                        }
                    }
                    else
                    {
                        int num21 = 0;
                        while (num21 < num15)
                        {
                            AnimatorClipInfo info8 = list4[num21];
                            float num22 = info8.weight * num14;
                            if (num22 != 0f)
                            {
                                float time = AnimationTime(currentAnimatorStateInfo.normalizedTime, info8.clip.length, currentAnimatorStateInfo.loop, currentAnimatorStateInfo.speed < 0f);
                                this.GetAnimation(info8.clip).Apply(skeleton, 0f, time, currentAnimatorStateInfo.loop, null, 1f, MixPose.Current, MixDirection.In);
                                break;
                            }
                            num21++;
                        }
                        while (num21 < num15)
                        {
                            AnimatorClipInfo info9 = list4[num21];
                            float alpha = info9.weight * num14;
                            if (alpha != 0f)
                            {
                                float time = AnimationTime(currentAnimatorStateInfo.normalizedTime, info9.clip.length, currentAnimatorStateInfo.loop, currentAnimatorStateInfo.speed < 0f);
                                this.GetAnimation(info9.clip).Apply(skeleton, 0f, time, currentAnimatorStateInfo.loop, null, alpha, MixPose.Current, MixDirection.In);
                            }
                            num21++;
                        }
                        num21 = 0;
                        if (flag2)
                        {
                            if (mode == MixMode.SpineStyle)
                            {
                                while (num21 < num16)
                                {
                                    AnimatorClipInfo info10 = list5[num21];
                                    float num24 = info10.weight * num14;
                                    if (num24 != 0f)
                                    {
                                        float time = AnimationTime(nextAnimatorStateInfo.normalizedTime, info10.clip.length, nextAnimatorStateInfo.speed < 0f);
                                        this.GetAnimation(info10.clip).Apply(skeleton, 0f, time, nextAnimatorStateInfo.loop, null, 1f, MixPose.Current, MixDirection.In);
                                        break;
                                    }
                                    num21++;
                                }
                            }
                            while (num21 < num16)
                            {
                                AnimatorClipInfo info11 = list5[num21];
                                float alpha = info11.weight * num14;
                                if (alpha != 0f)
                                {
                                    float time = AnimationTime(nextAnimatorStateInfo.normalizedTime, info11.clip.length, nextAnimatorStateInfo.speed < 0f);
                                    this.GetAnimation(info11.clip).Apply(skeleton, 0f, time, nextAnimatorStateInfo.loop, null, alpha, MixPose.Current, MixDirection.In);
                                }
                                num21++;
                            }
                        }
                    }
                    layerIndex++;
                }
            }

            private Spine.Animation GetAnimation(AnimationClip clip)
            {
                if (!this.clipNameHashCodeTable.TryGetValue(clip, out int hashCode))
                {
                    hashCode = clip.name.GetHashCode();
                    this.clipNameHashCodeTable.Add(clip, hashCode);
                }
                this.animationTable.TryGetValue(hashCode, out Spine.Animation animation);
                return animation;
            }

            private void GetAnimatorClipInfos(int layer, out int clipInfoCount, out int nextClipInfoCount, out IList<AnimatorClipInfo> clipInfo, out IList<AnimatorClipInfo> nextClipInfo)
            {
                clipInfoCount = this.animator.GetCurrentAnimatorClipInfoCount(layer);
                nextClipInfoCount = this.animator.GetNextAnimatorClipInfoCount(layer);
                if (this.clipInfoCache.Capacity < clipInfoCount)
                {
                    this.clipInfoCache.Capacity = clipInfoCount;
                }
                if (this.nextClipInfoCache.Capacity < nextClipInfoCount)
                {
                    this.nextClipInfoCache.Capacity = nextClipInfoCount;
                }
                this.animator.GetCurrentAnimatorClipInfo(layer, this.clipInfoCache);
                this.animator.GetNextAnimatorClipInfo(layer, this.nextClipInfoCache);
                clipInfo = this.clipInfoCache;
                nextClipInfo = this.nextClipInfoCache;
            }

            public void Initialize(UnityEngine.Animator animator, SkeletonDataAsset skeletonDataAsset)
            {
                this.animator = animator;
                this.previousAnimations.Clear();
                this.animationTable.Clear();
                foreach (Spine.Animation animation in skeletonDataAsset.GetSkeletonData(true).Animations)
                {
                    this.animationTable.Add(animation.Name.GetHashCode(), animation);
                }
                this.clipNameHashCodeTable.Clear();
                this.clipInfoCache.Clear();
                this.nextClipInfoCache.Clear();
            }

            public UnityEngine.Animator Animator =>
                this.animator;

            private class AnimationClipEqualityComparer : IEqualityComparer<AnimationClip>
            {
                internal static readonly IEqualityComparer<AnimationClip> Instance = new SkeletonAnimator.MecanimTranslator.AnimationClipEqualityComparer();

                public bool Equals(AnimationClip x, AnimationClip y) => 
                    (x.GetInstanceID() == y.GetInstanceID());

                public int GetHashCode(AnimationClip o) => 
                    o.GetInstanceID();
            }

            private class IntEqualityComparer : IEqualityComparer<int>
            {
                internal static readonly IEqualityComparer<int> Instance = new SkeletonAnimator.MecanimTranslator.IntEqualityComparer();

                public bool Equals(int x, int y) => 
                    (x == y);

                public int GetHashCode(int o) => 
                    o;
            }

            public enum MixMode
            {
                AlwaysMix,
                MixNext,
                SpineStyle
            }
        }
    }
}

