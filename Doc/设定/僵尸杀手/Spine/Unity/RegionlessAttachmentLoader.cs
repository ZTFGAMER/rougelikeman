namespace Spine.Unity
{
    using Spine;
    using System;
    using UnityEngine;

    public class RegionlessAttachmentLoader : AttachmentLoader
    {
        private static AtlasRegion emptyRegion;

        public BoundingBoxAttachment NewBoundingBoxAttachment(Skin skin, string name) => 
            new BoundingBoxAttachment(name);

        public ClippingAttachment NewClippingAttachment(Skin skin, string name) => 
            new ClippingAttachment(name);

        public MeshAttachment NewMeshAttachment(Skin skin, string name, string path) => 
            new MeshAttachment(name) { RendererObject = EmptyRegion };

        public PathAttachment NewPathAttachment(Skin skin, string name) => 
            new PathAttachment(name);

        public PointAttachment NewPointAttachment(Skin skin, string name) => 
            new PointAttachment(name);

        public RegionAttachment NewRegionAttachment(Skin skin, string name, string path) => 
            new RegionAttachment(name) { RendererObject = EmptyRegion };

        private static AtlasRegion EmptyRegion
        {
            get
            {
                if (emptyRegion == null)
                {
                    AtlasRegion region = new AtlasRegion {
                        name = "Empty AtlasRegion"
                    };
                    AtlasPage page = new AtlasPage {
                        name = "Empty AtlasPage"
                    };
                    Material material = new Material(Shader.Find("Spine/Special/HiddenPass")) {
                        name = "NoRender Material"
                    };
                    page.rendererObject = material;
                    region.page = page;
                    emptyRegion = region;
                }
                return emptyRegion;
            }
        }
    }
}

