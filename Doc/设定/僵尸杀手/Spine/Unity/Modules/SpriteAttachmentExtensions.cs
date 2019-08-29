namespace Spine.Unity.Modules
{
    using Spine;
    using Spine.Unity.Modules.AttachmentTools;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class SpriteAttachmentExtensions
    {
        [Obsolete]
        public static RegionAttachment AddUnitySprite(this SkeletonData skeletonData, string slotName, Sprite sprite, string skinName = "", string shaderName = "Spine/Skeleton", bool applyPMA = true, float rotation = 0f) => 
            skeletonData.AddUnitySprite(slotName, sprite, skinName, Shader.Find(shaderName), applyPMA, rotation);

        [Obsolete]
        public static RegionAttachment AddUnitySprite(this SkeletonData skeletonData, string slotName, Sprite sprite, string skinName, Shader shader, bool applyPMA, float rotation = 0f)
        {
            RegionAttachment attachment = !applyPMA ? sprite.ToRegionAttachment(new Material(shader), rotation) : sprite.ToRegionAttachmentPMAClone(shader, TextureFormat.RGBA32, false, null, rotation);
            int slotIndex = skeletonData.FindSlotIndex(slotName);
            Skin defaultSkin = skeletonData.DefaultSkin;
            if (skinName != string.Empty)
            {
                defaultSkin = skeletonData.FindSkin(skinName);
            }
            defaultSkin.AddAttachment(slotIndex, attachment.Name, attachment);
            return attachment;
        }

        [Obsolete]
        public static RegionAttachment AttachUnitySprite(this Skeleton skeleton, string slotName, Sprite sprite, string shaderName = "Spine/Skeleton", bool applyPMA = true, float rotation = 0f) => 
            skeleton.AttachUnitySprite(slotName, sprite, Shader.Find(shaderName), applyPMA, rotation);

        [Obsolete]
        public static RegionAttachment AttachUnitySprite(this Skeleton skeleton, string slotName, Sprite sprite, Shader shader, bool applyPMA, float rotation = 0f)
        {
            RegionAttachment attachment = !applyPMA ? sprite.ToRegionAttachment(new Material(shader), rotation) : sprite.ToRegionAttachmentPMAClone(shader, TextureFormat.RGBA32, false, null, rotation);
            skeleton.FindSlot(slotName).Attachment = attachment;
            return attachment;
        }
    }
}

