namespace Spine
{
    using System;

    public interface AttachmentLoader
    {
        BoundingBoxAttachment NewBoundingBoxAttachment(Skin skin, string name);
        ClippingAttachment NewClippingAttachment(Skin skin, string name);
        MeshAttachment NewMeshAttachment(Skin skin, string name, string path);
        PathAttachment NewPathAttachment(Skin skin, string name);
        PointAttachment NewPointAttachment(Skin skin, string name);
        RegionAttachment NewRegionAttachment(Skin skin, string name, string path);
    }
}

