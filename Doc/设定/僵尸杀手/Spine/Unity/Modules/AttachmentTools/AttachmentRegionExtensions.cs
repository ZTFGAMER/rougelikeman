namespace Spine.Unity.Modules.AttachmentTools
{
    using Spine;
    using Spine.Unity;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class AttachmentRegionExtensions
    {
        public static AtlasRegion GetRegion(this Attachment attachment)
        {
            IHasRendererObject obj2 = attachment as IHasRendererObject;
            if (obj2 != null)
            {
                return (obj2.RendererObject as AtlasRegion);
            }
            return null;
        }

        public static AtlasRegion GetRegion(this MeshAttachment meshAttachment) => 
            (meshAttachment.RendererObject as AtlasRegion);

        public static AtlasRegion GetRegion(this RegionAttachment regionAttachment) => 
            (regionAttachment.RendererObject as AtlasRegion);

        public static void SetPositionOffset(this RegionAttachment regionAttachment, Vector2 offset)
        {
            regionAttachment.x = offset.x;
            regionAttachment.y = offset.y;
        }

        public static void SetPositionOffset(this RegionAttachment regionAttachment, float x, float y)
        {
            regionAttachment.x = x;
            regionAttachment.y = y;
        }

        public static void SetRegion(this Attachment attachment, AtlasRegion region, bool updateOffset = true)
        {
            RegionAttachment attachment2 = attachment as RegionAttachment;
            if (attachment2 != null)
            {
                attachment2.SetRegion(region, updateOffset);
            }
            MeshAttachment attachment3 = attachment as MeshAttachment;
            if (attachment3 != null)
            {
                attachment3.SetRegion(region, updateOffset);
            }
        }

        public static void SetRegion(this MeshAttachment attachment, AtlasRegion region, bool updateUVs = true)
        {
            if (region == null)
            {
                throw new ArgumentNullException("region");
            }
            attachment.RendererObject = region;
            attachment.RegionU = region.u;
            attachment.RegionV = region.v;
            attachment.RegionU2 = region.u2;
            attachment.RegionV2 = region.v2;
            attachment.RegionRotate = region.rotate;
            attachment.regionOffsetX = region.offsetX;
            attachment.regionOffsetY = region.offsetY;
            attachment.regionWidth = region.width;
            attachment.regionHeight = region.height;
            attachment.regionOriginalWidth = region.originalWidth;
            attachment.regionOriginalHeight = region.originalHeight;
            if (updateUVs)
            {
                attachment.UpdateUVs();
            }
        }

        public static void SetRegion(this RegionAttachment attachment, AtlasRegion region, bool updateOffset = true)
        {
            if (region == null)
            {
                throw new ArgumentNullException("region");
            }
            attachment.RendererObject = region;
            attachment.SetUVs(region.u, region.v, region.u2, region.v2, region.rotate);
            attachment.regionOffsetX = region.offsetX;
            attachment.regionOffsetY = region.offsetY;
            attachment.regionWidth = region.width;
            attachment.regionHeight = region.height;
            attachment.regionOriginalWidth = region.originalWidth;
            attachment.regionOriginalHeight = region.originalHeight;
            if (updateOffset)
            {
                attachment.UpdateOffset();
            }
        }

        public static void SetRotation(this RegionAttachment regionAttachment, float rotation)
        {
            regionAttachment.rotation = rotation;
        }

        public static void SetScale(this RegionAttachment regionAttachment, Vector2 scale)
        {
            regionAttachment.scaleX = scale.x;
            regionAttachment.scaleY = scale.y;
        }

        public static void SetScale(this RegionAttachment regionAttachment, float x, float y)
        {
            regionAttachment.scaleX = x;
            regionAttachment.scaleY = y;
        }

        public static RegionAttachment ToRegionAttachment(this Sprite sprite, AtlasPage page, float rotation = 0f)
        {
            if (sprite == null)
            {
                throw new ArgumentNullException("sprite");
            }
            if (page == null)
            {
                throw new ArgumentNullException("page");
            }
            AtlasRegion region = sprite.ToAtlasRegion(page);
            float scale = 1f / sprite.pixelsPerUnit;
            return region.ToRegionAttachment(sprite.name, scale, rotation);
        }

        public static RegionAttachment ToRegionAttachment(this Sprite sprite, Material material, float rotation = 0f) => 
            sprite.ToRegionAttachment(material.ToSpineAtlasPage(), rotation);

        public static RegionAttachment ToRegionAttachment(this AtlasRegion region, string attachmentName, float scale = 0.01f, float rotation = 0f)
        {
            if (string.IsNullOrEmpty(attachmentName))
            {
                throw new ArgumentException("attachmentName can't be null or empty.", "attachmentName");
            }
            if (region == null)
            {
                throw new ArgumentNullException("region");
            }
            RegionAttachment attachment = new RegionAttachment(attachmentName) {
                RendererObject = region
            };
            attachment.SetUVs(region.u, region.v, region.u2, region.v2, region.rotate);
            attachment.regionOffsetX = region.offsetX;
            attachment.regionOffsetY = region.offsetY;
            attachment.regionWidth = region.width;
            attachment.regionHeight = region.height;
            attachment.regionOriginalWidth = region.originalWidth;
            attachment.regionOriginalHeight = region.originalHeight;
            attachment.Path = region.name;
            attachment.scaleX = 1f;
            attachment.scaleY = 1f;
            attachment.rotation = rotation;
            attachment.r = 1f;
            attachment.g = 1f;
            attachment.b = 1f;
            attachment.a = 1f;
            attachment.width = attachment.regionOriginalWidth * scale;
            attachment.height = attachment.regionOriginalHeight * scale;
            attachment.SetColor(Color.white);
            attachment.UpdateOffset();
            return attachment;
        }

        public static RegionAttachment ToRegionAttachmentPMAClone(this Sprite sprite, Material materialPropertySource, TextureFormat textureFormat = 4, bool mipmaps = false, float rotation = 0f) => 
            sprite.ToRegionAttachmentPMAClone(materialPropertySource.shader, textureFormat, mipmaps, materialPropertySource, rotation);

        public static RegionAttachment ToRegionAttachmentPMAClone(this Sprite sprite, Shader shader, TextureFormat textureFormat = 4, bool mipmaps = false, Material materialPropertySource = null, float rotation = 0f)
        {
            if (sprite == null)
            {
                throw new ArgumentNullException("sprite");
            }
            if (shader == null)
            {
                throw new ArgumentNullException("shader");
            }
            AtlasRegion region = sprite.ToAtlasRegionPMAClone(shader, textureFormat, mipmaps, materialPropertySource);
            float scale = 1f / sprite.pixelsPerUnit;
            return region.ToRegionAttachment(sprite.name, scale, rotation);
        }
    }
}

