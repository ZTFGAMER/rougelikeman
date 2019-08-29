namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using Spine.Unity.Modules.AttachmentTools;
    using System;
    using UnityEngine;

    public class EquipsVisualsComponentExample : MonoBehaviour
    {
        public SkeletonAnimation skeletonAnimation;
        [SpineSkin("", "", true, false)]
        public string templateSkinName;
        private Skin equipsSkin;
        private Skin collectedSkin;
        public Material runtimeMaterial;
        public Texture2D runtimeAtlas;

        public void Equip(int slotIndex, string attachmentName, Attachment attachment)
        {
            this.equipsSkin.AddAttachment(slotIndex, attachmentName, attachment);
            this.skeletonAnimation.Skeleton.SetSkin(this.equipsSkin);
            this.RefreshSkeletonAttachments();
        }

        public void OptimizeSkin()
        {
            if (this.collectedSkin == null)
            {
            }
            this.collectedSkin = new Skin("Collected skin");
            this.collectedSkin.Clear();
            this.collectedSkin.Append(this.skeletonAnimation.Skeleton.Data.DefaultSkin);
            this.collectedSkin.Append(this.equipsSkin);
            Skin skin = this.collectedSkin.GetRepackedSkin("Repacked skin", this.skeletonAnimation.SkeletonDataAsset.atlasAssets[0].materials[0], out this.runtimeMaterial, out this.runtimeAtlas, 0x400, 2, TextureFormat.RGBA32, false, true);
            this.collectedSkin.Clear();
            this.skeletonAnimation.Skeleton.Skin = skin;
            this.RefreshSkeletonAttachments();
        }

        private void RefreshSkeletonAttachments()
        {
            this.skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            this.skeletonAnimation.AnimationState.Apply(this.skeletonAnimation.Skeleton);
        }

        private void Start()
        {
            this.equipsSkin = new Skin("Equips");
            Skin source = this.skeletonAnimation.Skeleton.Data.FindSkin(this.templateSkinName);
            if (source != null)
            {
                this.equipsSkin.Append(source);
            }
            this.skeletonAnimation.Skeleton.Skin = this.equipsSkin;
            this.RefreshSkeletonAttachments();
        }
    }
}

