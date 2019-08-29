namespace Spine.Unity.Examples
{
    using Spine.Unity;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DataAssetsFromExportsExample : MonoBehaviour
    {
        public TextAsset skeletonJson;
        public TextAsset atlasText;
        public Texture2D[] textures;
        public Material materialPropertySource;
        private AtlasAsset runtimeAtlasAsset;
        private SkeletonDataAsset runtimeSkeletonDataAsset;
        private SkeletonAnimation runtimeSkeletonAnimation;

        private void CreateRuntimeAssetsAndGameObject()
        {
            this.runtimeAtlasAsset = AtlasAsset.CreateRuntimeInstance(this.atlasText, this.textures, this.materialPropertySource, true);
            this.runtimeSkeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(this.skeletonJson, this.runtimeAtlasAsset, true, 0.01f);
        }

        [DebuggerHidden]
        private IEnumerator Start() => 
            new <Start>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal DataAssetsFromExportsExample $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$this.CreateRuntimeAssetsAndGameObject();
                        this.$this.runtimeSkeletonDataAsset.GetSkeletonData(false);
                        this.$current = new WaitForSeconds(0.5f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.runtimeSkeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(this.$this.runtimeSkeletonDataAsset);
                        this.$this.runtimeSkeletonAnimation.Initialize(false);
                        this.$this.runtimeSkeletonAnimation.Skeleton.SetSkin("base");
                        this.$this.runtimeSkeletonAnimation.Skeleton.SetSlotsToSetupPose();
                        this.$this.runtimeSkeletonAnimation.AnimationState.SetAnimation(0, "run", true);
                        this.$this.runtimeSkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = 10;
                        this.$this.runtimeSkeletonAnimation.transform.Translate(Vector3.down * 2f);
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

