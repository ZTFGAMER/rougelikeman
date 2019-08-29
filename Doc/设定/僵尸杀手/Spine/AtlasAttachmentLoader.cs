namespace Spine
{
    using System;

    public class AtlasAttachmentLoader : AttachmentLoader
    {
        private Atlas[] atlasArray;

        public AtlasAttachmentLoader(params Atlas[] atlasArray)
        {
            if (atlasArray == null)
            {
                throw new ArgumentNullException("atlas array cannot be null.");
            }
            this.atlasArray = atlasArray;
        }

        public AtlasRegion FindRegion(string name)
        {
            for (int i = 0; i < this.atlasArray.Length; i++)
            {
                AtlasRegion region = this.atlasArray[i].FindRegion(name);
                if (region != null)
                {
                    return region;
                }
            }
            return null;
        }

        public BoundingBoxAttachment NewBoundingBoxAttachment(Skin skin, string name) => 
            new BoundingBoxAttachment(name);

        public ClippingAttachment NewClippingAttachment(Skin skin, string name) => 
            new ClippingAttachment(name);

        public MeshAttachment NewMeshAttachment(Skin skin, string name, string path)
        {
            AtlasRegion region = this.FindRegion(path);
            if (region == null)
            {
                throw new ArgumentException($"Region not found in atlas: {path} (region attachment: {name})");
            }
            return new MeshAttachment(name) { 
                RendererObject = region,
                RegionU = region.u,
                RegionV = region.v,
                RegionU2 = region.u2,
                RegionV2 = region.v2,
                RegionRotate = region.rotate,
                regionOffsetX = region.offsetX,
                regionOffsetY = region.offsetY,
                regionWidth = region.width,
                regionHeight = region.height,
                regionOriginalWidth = region.originalWidth,
                regionOriginalHeight = region.originalHeight
            };
        }

        public PathAttachment NewPathAttachment(Skin skin, string name) => 
            new PathAttachment(name);

        public PointAttachment NewPointAttachment(Skin skin, string name) => 
            new PointAttachment(name);

        public RegionAttachment NewRegionAttachment(Skin skin, string name, string path)
        {
            AtlasRegion region = this.FindRegion(path);
            if (region == null)
            {
                throw new ArgumentException($"Region not found in atlas: {path} (region attachment: {name})");
            }
            RegionAttachment attachment = new RegionAttachment(name) {
                RendererObject = region
            };
            attachment.SetUVs(region.u, region.v, region.u2, region.v2, region.rotate);
            attachment.regionOffsetX = region.offsetX;
            attachment.regionOffsetY = region.offsetY;
            attachment.regionWidth = region.width;
            attachment.regionHeight = region.height;
            attachment.regionOriginalWidth = region.originalWidth;
            attachment.regionOriginalHeight = region.originalHeight;
            return attachment;
        }
    }
}

