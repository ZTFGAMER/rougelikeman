namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using Spine.Unity.Modules.AttachmentTools;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class EquipSystemExample : MonoBehaviour, IHasSkeletonDataAsset
    {
        public SkeletonDataAsset skeletonDataAsset;
        public Material sourceMaterial;
        public bool applyPMA = true;
        public List<EquipHook> equippables = new List<EquipHook>();
        public EquipsVisualsComponentExample target;
        public Dictionary<EquipAssetExample, Attachment> cachedAttachments = new Dictionary<EquipAssetExample, Attachment>();

        public void Done()
        {
            this.target.OptimizeSkin();
        }

        public void Equip(EquipAssetExample asset)
        {
            <Equip>c__AnonStorey0 storey = new <Equip>c__AnonStorey0 {
                equipType = asset.equipType
            };
            EquipHook hook = this.equippables.Find(new Predicate<EquipHook>(storey.<>m__0));
            int slotIndex = this.skeletonDataAsset.GetSkeletonData(true).FindSlotIndex(hook.slot);
            Attachment attachment = this.GenerateAttachmentFromEquipAsset(asset, slotIndex, hook.templateSkin, hook.templateAttachment);
            this.target.Equip(slotIndex, hook.templateAttachment, attachment);
        }

        private Attachment GenerateAttachmentFromEquipAsset(EquipAssetExample asset, int slotIndex, string templateSkinName, string templateAttachmentName)
        {
            this.cachedAttachments.TryGetValue(asset, out Attachment attachment);
            if (attachment == null)
            {
                attachment = this.skeletonDataAsset.GetSkeletonData(true).FindSkin(templateSkinName).GetAttachment(slotIndex, templateAttachmentName).GetRemappedClone(asset.sprite, this.sourceMaterial, this.applyPMA, true, false);
                this.cachedAttachments.Add(asset, attachment);
            }
            return attachment;
        }

        SkeletonDataAsset IHasSkeletonDataAsset.SkeletonDataAsset =>
            this.skeletonDataAsset;

        [CompilerGenerated]
        private sealed class <Equip>c__AnonStorey0
        {
            internal EquipSystemExample.EquipType equipType;

            internal bool <>m__0(EquipSystemExample.EquipHook x) => 
                (x.type == this.equipType);
        }

        [Serializable]
        public class EquipHook
        {
            public EquipSystemExample.EquipType type;
            [SpineSlot("", "", false, true, false)]
            public string slot;
            [SpineSkin("", "", true, false)]
            public string templateSkin;
            [SpineAttachment(true, false, false, "", "", "templateSkin", true, false)]
            public string templateAttachment;
        }

        public enum EquipType
        {
            Gun,
            Goggles
        }
    }
}

