namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using Spine.Unity.Modules.AttachmentTools;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MixAndMatch : MonoBehaviour
    {
        [SpineSkin("", "", true, false)]
        public string templateAttachmentsSkin = "base";
        public Material sourceMaterial;
        [Header("Visor")]
        public Sprite visorSprite;
        [SpineSlot("", "", false, true, false)]
        public string visorSlot;
        [SpineAttachment(true, false, false, "visorSlot", "", "baseSkinName", true, false)]
        public string visorKey = "goggles";
        [Header("Gun")]
        public Sprite gunSprite;
        [SpineSlot("", "", false, true, false)]
        public string gunSlot;
        [SpineAttachment(true, false, false, "gunSlot", "", "baseSkinName", true, false)]
        public string gunKey = "gun";
        [Header("Runtime Repack")]
        public bool repack = true;
        public BoundingBoxFollower bbFollower;
        [Header("Do not assign")]
        public Texture2D runtimeAtlas;
        public Material runtimeMaterial;
        private Skin customSkin;

        private void Apply()
        {
            SkeletonAnimation component = base.GetComponent<SkeletonAnimation>();
            Skeleton skeleton = component.Skeleton;
            if (this.customSkin == null)
            {
            }
            this.customSkin = new Skin("custom skin");
            Skin skin = skeleton.Data.FindSkin(this.templateAttachmentsSkin);
            int slotIndex = skeleton.FindSlotIndex(this.visorSlot);
            Attachment attachment = skin.GetAttachment(slotIndex, this.visorKey).GetRemappedClone(this.visorSprite, this.sourceMaterial, true, true, false);
            this.customSkin.SetAttachment(slotIndex, this.visorKey, attachment);
            int num2 = skeleton.FindSlotIndex(this.gunSlot);
            Attachment attachment4 = skin.GetAttachment(num2, this.gunKey).GetRemappedClone(this.gunSprite, this.sourceMaterial, true, true, false);
            if (attachment4 != null)
            {
                this.customSkin.SetAttachment(num2, this.gunKey, attachment4);
            }
            if (this.repack)
            {
                Skin destination = new Skin("repacked skin");
                destination.Append(skeleton.Data.DefaultSkin);
                destination.Append(this.customSkin);
                destination = destination.GetRepackedSkin("repacked skin", this.sourceMaterial, out this.runtimeMaterial, out this.runtimeAtlas, 0x400, 2, TextureFormat.RGBA32, false, true);
                skeleton.SetSkin(destination);
                if (this.bbFollower != null)
                {
                    this.bbFollower.Initialize(true);
                }
            }
            else
            {
                skeleton.SetSkin(this.customSkin);
            }
            skeleton.SetSlotsToSetupPose();
            component.Update(0f);
            Resources.UnloadUnusedAssets();
        }

        private void OnValidate()
        {
            if (this.sourceMaterial == null)
            {
                SkeletonAnimation component = base.GetComponent<SkeletonAnimation>();
                if (component != null)
                {
                    this.sourceMaterial = component.SkeletonDataAsset.atlasAssets[0].materials[0];
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator Start() => 
            new <Start>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal MixAndMatch $this;
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
                        this.$current = new WaitForSeconds(1f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.Apply();
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

