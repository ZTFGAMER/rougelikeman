namespace Spine.Unity.Modules.AttachmentTools
{
    using Spine;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class AttachmentCloneExtensions
    {
        private static void CloneVertexAttachment(VertexAttachment src, VertexAttachment dest)
        {
            dest.worldVerticesLength = src.worldVerticesLength;
            if (src.bones != null)
            {
                dest.bones = src.bones.Clone() as int[];
            }
            if (src.vertices != null)
            {
                dest.vertices = src.vertices.Clone() as float[];
            }
        }

        public static BoundingBoxAttachment GetClone(this BoundingBoxAttachment o)
        {
            BoundingBoxAttachment dest = new BoundingBoxAttachment(o.Name);
            CloneVertexAttachment(o, dest);
            return dest;
        }

        public static ClippingAttachment GetClone(this ClippingAttachment o)
        {
            ClippingAttachment dest = new ClippingAttachment(o.Name) {
                endSlot = o.endSlot
            };
            CloneVertexAttachment(o, dest);
            return dest;
        }

        public static MeshAttachment GetClone(this MeshAttachment o)
        {
            MeshAttachment dest = new MeshAttachment(o.Name) {
                r = o.r,
                g = o.g,
                b = o.b,
                a = o.a,
                inheritDeform = o.inheritDeform,
                Path = o.Path,
                RendererObject = o.RendererObject,
                regionOffsetX = o.regionOffsetX,
                regionOffsetY = o.regionOffsetY,
                regionWidth = o.regionWidth,
                regionHeight = o.regionHeight,
                regionOriginalWidth = o.regionOriginalWidth,
                regionOriginalHeight = o.regionOriginalHeight,
                RegionU = o.RegionU,
                RegionV = o.RegionV,
                RegionU2 = o.RegionU2,
                RegionV2 = o.RegionV2,
                RegionRotate = o.RegionRotate,
                uvs = o.uvs.Clone() as float[]
            };
            if (o.ParentMesh != null)
            {
                dest.ParentMesh = o.ParentMesh;
                return dest;
            }
            CloneVertexAttachment(o, dest);
            dest.regionUVs = o.regionUVs.Clone() as float[];
            dest.triangles = o.triangles.Clone() as int[];
            dest.hulllength = o.hulllength;
            dest.Edges = (o.Edges != null) ? (o.Edges.Clone() as int[]) : null;
            dest.Width = o.Width;
            dest.Height = o.Height;
            return dest;
        }

        public static PathAttachment GetClone(this PathAttachment o)
        {
            PathAttachment dest = new PathAttachment(o.Name) {
                lengths = o.lengths.Clone() as float[],
                closed = o.closed,
                constantSpeed = o.constantSpeed
            };
            CloneVertexAttachment(o, dest);
            return dest;
        }

        public static PointAttachment GetClone(this PointAttachment o) => 
            new PointAttachment(o.Name) { 
                rotation = o.rotation,
                x = o.x,
                y = o.y
            };

        public static RegionAttachment GetClone(this RegionAttachment o) => 
            new RegionAttachment(o.Name + "clone") { 
                x = o.x,
                y = o.y,
                rotation = o.rotation,
                scaleX = o.scaleX,
                scaleY = o.scaleY,
                width = o.width,
                height = o.height,
                r = o.r,
                g = o.g,
                b = o.b,
                a = o.a,
                Path = o.Path,
                RendererObject = o.RendererObject,
                regionOffsetX = o.regionOffsetX,
                regionOffsetY = o.regionOffsetY,
                regionWidth = o.regionWidth,
                regionHeight = o.regionHeight,
                regionOriginalWidth = o.regionOriginalWidth,
                regionOriginalHeight = o.regionOriginalHeight,
                uvs = o.uvs.Clone() as float[],
                offset = o.offset.Clone() as float[]
            };

        public static Attachment GetClone(this Attachment o, bool cloneMeshesAsLinked)
        {
            RegionAttachment attachment = o as RegionAttachment;
            if (attachment != null)
            {
                return attachment.GetClone();
            }
            MeshAttachment attachment2 = o as MeshAttachment;
            if (attachment2 != null)
            {
                return (!cloneMeshesAsLinked ? attachment2.GetClone() : attachment2.GetLinkedClone(true));
            }
            BoundingBoxAttachment attachment3 = o as BoundingBoxAttachment;
            if (attachment3 != null)
            {
                return attachment3.GetClone();
            }
            PathAttachment attachment4 = o as PathAttachment;
            if (attachment4 != null)
            {
                return attachment4.GetClone();
            }
            PointAttachment attachment5 = o as PointAttachment;
            if (attachment5 != null)
            {
                return attachment5.GetClone();
            }
            ClippingAttachment attachment6 = o as ClippingAttachment;
            if (attachment6 != null)
            {
                return attachment6.GetClone();
            }
            return null;
        }

        public static MeshAttachment GetLinkedClone(this MeshAttachment o, bool inheritDeform = true) => 
            o.GetLinkedMesh(o.Name, (o.RendererObject as AtlasRegion), inheritDeform);

        public static MeshAttachment GetLinkedMesh(this MeshAttachment o, string newLinkedMeshName, AtlasRegion region, bool inheritDeform = true)
        {
            if (region == null)
            {
                throw new ArgumentNullException("region");
            }
            if (o.ParentMesh != null)
            {
                o = o.ParentMesh;
            }
            MeshAttachment attachment = new MeshAttachment(newLinkedMeshName);
            attachment.SetRegion(region, false);
            attachment.Path = newLinkedMeshName;
            attachment.r = 1f;
            attachment.g = 1f;
            attachment.b = 1f;
            attachment.a = 1f;
            attachment.inheritDeform = inheritDeform;
            attachment.ParentMesh = o;
            attachment.UpdateUVs();
            return attachment;
        }

        public static MeshAttachment GetLinkedMesh(this MeshAttachment o, Sprite sprite, Material materialPropertySource, bool inheritDeform = true) => 
            o.GetLinkedMesh(sprite, materialPropertySource.shader, inheritDeform, materialPropertySource);

        public static MeshAttachment GetLinkedMesh(this MeshAttachment o, Sprite sprite, Shader shader, bool inheritDeform = true, Material materialPropertySource = null)
        {
            Material material = new Material(shader);
            if (materialPropertySource != null)
            {
                material.CopyPropertiesFromMaterial(materialPropertySource);
                material.shaderKeywords = materialPropertySource.shaderKeywords;
            }
            return o.GetLinkedMesh(sprite.name, sprite.ToAtlasRegion(false), inheritDeform);
        }

        public static Attachment GetRemappedClone(this Attachment o, AtlasRegion atlasRegion, bool cloneMeshAsLinked = true, bool useOriginalRegionSize = false, float scale = 0.01f)
        {
            RegionAttachment attachment = o as RegionAttachment;
            if (attachment != null)
            {
                RegionAttachment clone = attachment.GetClone();
                clone.SetRegion(atlasRegion, false);
                if (!useOriginalRegionSize)
                {
                    clone.width = atlasRegion.width * scale;
                    clone.height = atlasRegion.height * scale;
                }
                clone.UpdateOffset();
                return clone;
            }
            MeshAttachment attachment3 = o as MeshAttachment;
            if (attachment3 != null)
            {
                MeshAttachment attachment4 = !cloneMeshAsLinked ? attachment3.GetClone() : attachment3.GetLinkedClone(cloneMeshAsLinked);
                attachment4.SetRegion(atlasRegion, true);
                return attachment4;
            }
            return o.GetClone(true);
        }

        public static Attachment GetRemappedClone(this Attachment o, Sprite sprite, Material sourceMaterial, bool premultiplyAlpha = true, bool cloneMeshAsLinked = true, bool useOriginalRegionSize = false)
        {
            AtlasRegion atlasRegion = !premultiplyAlpha ? sprite.ToAtlasRegion(false) : sprite.ToAtlasRegionPMAClone(sourceMaterial, TextureFormat.RGBA32, false);
            return o.GetRemappedClone(atlasRegion, cloneMeshAsLinked, useOriginalRegionSize, (1f / sprite.pixelsPerUnit));
        }
    }
}

