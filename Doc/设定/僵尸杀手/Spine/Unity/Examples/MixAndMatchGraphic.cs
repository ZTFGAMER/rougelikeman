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

    public class MixAndMatchGraphic : MonoBehaviour
    {
        [SpineSkin("", "", true, false)]
        public string baseSkinName = "base";
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
        [Header("Runtime Repack Required!!")]
        public bool repack = true;
        [Header("Do not assign")]
        public Texture2D runtimeAtlas;
        public Material runtimeMaterial;
        private Skin customSkin;

        [ContextMenu("Apply")]
        private void Apply()
        {
            SkeletonGraphic component = base.GetComponent<SkeletonGraphic>();
            Skeleton skeleton = component.Skeleton;
            if (this.customSkin == null)
            {
            }
            this.customSkin = new Skin("custom skin");
            Skin skin = skeleton.Data.FindSkin(this.baseSkinName);
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
            }
            else
            {
                skeleton.SetSkin(this.customSkin);
            }
            skeleton.SetToSetupPose();
            component.Update(0f);
            component.OverrideTexture = this.runtimeAtlas;
            Resources.UnloadUnusedAssets();
        }

        private void OnValidate()
        {
            if (this.sourceMaterial == null)
            {
                SkeletonGraphic component = base.GetComponent<SkeletonGraphic>();
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
            internal MixAndMatchGraphic $this;
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

