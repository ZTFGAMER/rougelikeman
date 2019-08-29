namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class SkeletonAnimationMulti : MonoBehaviour
    {
        private const int MainTrackIndex = 0;
        public bool initialFlipX;
        public bool initialFlipY;
        public string initialAnimation;
        public bool initialLoop;
        [Space]
        public List<SkeletonDataAsset> skeletonDataAssets = new List<SkeletonDataAsset>();
        [Header("Settings")]
        public MeshGenerator.Settings meshGeneratorSettings = MeshGenerator.Settings.Default;
        private readonly List<SkeletonAnimation> skeletonAnimations = new List<SkeletonAnimation>();
        private readonly Dictionary<string, Spine.Animation> animationNameTable = new Dictionary<string, Spine.Animation>();
        private readonly Dictionary<Spine.Animation, SkeletonAnimation> animationSkeletonTable = new Dictionary<Spine.Animation, SkeletonAnimation>();
        private SkeletonAnimation currentSkeletonAnimation;

        private void Awake()
        {
            this.Initialize(false);
        }

        private void Clear()
        {
            foreach (SkeletonAnimation animation in this.skeletonAnimations)
            {
                UnityEngine.Object.Destroy(animation.gameObject);
            }
            this.skeletonAnimations.Clear();
            this.animationNameTable.Clear();
            this.animationSkeletonTable.Clear();
        }

        public void ClearAnimation()
        {
            this.currentSkeletonAnimation.state.ClearTrack(0);
        }

        public Spine.Animation FindAnimation(string animationName)
        {
            this.animationNameTable.TryGetValue(animationName, out Spine.Animation animation);
            return animation;
        }

        public TrackEntry GetCurrent() => 
            this.currentSkeletonAnimation.state.GetCurrent(0);

        public void Initialize(bool overwrite)
        {
            if ((this.skeletonAnimations.Count == 0) || overwrite)
            {
                this.Clear();
                MeshGenerator.Settings meshGeneratorSettings = this.meshGeneratorSettings;
                Transform parent = base.transform;
                foreach (SkeletonDataAsset asset in this.skeletonDataAssets)
                {
                    SkeletonAnimation item = SkeletonAnimation.NewSkeletonAnimationGameObject(asset);
                    item.transform.SetParent(parent, false);
                    item.SetMeshSettings(meshGeneratorSettings);
                    item.initialFlipX = this.initialFlipX;
                    item.initialFlipY = this.initialFlipY;
                    Skeleton skeleton = item.skeleton;
                    skeleton.FlipX = this.initialFlipX;
                    skeleton.FlipY = this.initialFlipY;
                    item.Initialize(false);
                    this.skeletonAnimations.Add(item);
                }
                Dictionary<string, Spine.Animation> animationNameTable = this.animationNameTable;
                Dictionary<Spine.Animation, SkeletonAnimation> animationSkeletonTable = this.animationSkeletonTable;
                foreach (SkeletonAnimation animation2 in this.skeletonAnimations)
                {
                    foreach (Spine.Animation animation3 in animation2.Skeleton.Data.Animations)
                    {
                        animationNameTable[animation3.Name] = animation3;
                        animationSkeletonTable[animation3] = animation2;
                    }
                }
                this.SetActiveSkeleton(this.skeletonAnimations[0]);
                this.SetAnimation(this.initialAnimation, this.initialLoop);
            }
        }

        private void SetActiveSkeleton(SkeletonAnimation skeletonAnimation)
        {
            foreach (SkeletonAnimation animation in this.skeletonAnimations)
            {
                animation.gameObject.SetActive(animation == skeletonAnimation);
            }
            this.currentSkeletonAnimation = skeletonAnimation;
        }

        public TrackEntry SetAnimation(Spine.Animation animation, bool loop)
        {
            if (animation != null)
            {
                this.animationSkeletonTable.TryGetValue(animation, out SkeletonAnimation animation2);
                if (animation2 != null)
                {
                    this.SetActiveSkeleton(animation2);
                    animation2.skeleton.SetToSetupPose();
                    return animation2.state.SetAnimation(0, animation, loop);
                }
            }
            return null;
        }

        public TrackEntry SetAnimation(string animationName, bool loop) => 
            this.SetAnimation(this.FindAnimation(animationName), loop);

        public void SetEmptyAnimation(float mixDuration)
        {
            this.currentSkeletonAnimation.state.SetEmptyAnimation(0, mixDuration);
        }

        public Dictionary<Spine.Animation, SkeletonAnimation> AnimationSkeletonTable =>
            this.animationSkeletonTable;

        public Dictionary<string, Spine.Animation> AnimationNameTable =>
            this.animationNameTable;

        public SkeletonAnimation CurrentSkeletonAnimation =>
            this.currentSkeletonAnimation;
    }
}

