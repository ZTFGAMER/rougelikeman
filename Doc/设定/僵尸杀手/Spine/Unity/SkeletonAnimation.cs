namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Spine/SkeletonAnimation"), HelpURL("http://esotericsoftware.com/spine-unity-documentation#Controlling-Animation")]
    public class SkeletonAnimation : SkeletonRenderer, ISkeletonAnimation, IAnimationStateComponent
    {
        public Spine.AnimationState state;
        [SerializeField, SpineAnimation("", "", true, false)]
        private string _animationName;
        public bool loop;
        public float timeScale = 1f;

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

        public static SkeletonAnimation AddToGameObject(GameObject gameObject, SkeletonDataAsset skeletonDataAsset) => 
            SkeletonRenderer.AddSpineComponent<SkeletonAnimation>(gameObject, skeletonDataAsset);

        public override void ClearState()
        {
            base.ClearState();
            if (this.state != null)
            {
                this.state.ClearTracks();
            }
        }

        public override void Initialize(bool overwrite)
        {
            if (!base.valid || overwrite)
            {
                base.Initialize(overwrite);
                if (base.valid)
                {
                    this.state = new Spine.AnimationState(base.skeletonDataAsset.GetAnimationStateData());
                    if (!string.IsNullOrEmpty(this._animationName) && (this.TrySetAnimation(this._animationName, this.loop) != null))
                    {
                        this.Update(0f);
                    }
                }
            }
        }

        public static SkeletonAnimation NewSkeletonAnimationGameObject(SkeletonDataAsset skeletonDataAsset) => 
            SkeletonRenderer.NewSpineGameObject<SkeletonAnimation>(skeletonDataAsset);

        private TrackEntry TrySetAnimation(string animationName, bool animationLoop)
        {
            Spine.Animation animation = base.skeletonDataAsset.GetSkeletonData(false).FindAnimation(animationName);
            if (animation != null)
            {
                return this.state.SetAnimation(0, animation, animationLoop);
            }
            return null;
        }

        private void Update()
        {
            this.Update(Time.deltaTime);
        }

        public void Update(float deltaTime)
        {
            if (base.valid)
            {
                deltaTime *= this.timeScale;
                base.skeleton.Update(deltaTime);
                this.state.Update(deltaTime);
                this.state.Apply(base.skeleton);
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

        public Spine.AnimationState AnimationState =>
            this.state;

        public string AnimationName
        {
            get
            {
                if (!base.valid)
                {
                    return this._animationName;
                }
                TrackEntry current = this.state.GetCurrent(0);
                return current?.Animation.Name;
            }
            set
            {
                if (this._animationName != value)
                {
                    this._animationName = value;
                    if (string.IsNullOrEmpty(value))
                    {
                        this.state.ClearTrack(0);
                    }
                    else
                    {
                        this.TrySetAnimation(value, this.loop);
                    }
                }
            }
        }
    }
}

