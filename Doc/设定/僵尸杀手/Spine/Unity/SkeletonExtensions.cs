namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class SkeletonExtensions
    {
        private const float ByteToFloat = 0.003921569f;

        public static Color GetColor(this MeshAttachment a) => 
            new Color(a.r, a.g, a.b, a.a);

        public static Color GetColor(this RegionAttachment a) => 
            new Color(a.r, a.g, a.b, a.a);

        public static Color GetColor(this Skeleton s) => 
            new Color(s.r, s.g, s.b, s.a);

        public static Color GetColor(this Slot s) => 
            new Color(s.r, s.g, s.b, s.a);

        public static Color GetColorTintBlack(this Slot s) => 
            new Color(s.r2, s.g2, s.b2, 1f);

        public static Vector2 GetLocalPosition(this Bone bone) => 
            new Vector2(bone.x, bone.y);

        public static Quaternion GetLocalQuaternion(this Bone bone)
        {
            float f = (bone.rotation * 0.01745329f) * 0.5f;
            return new Quaternion(0f, 0f, Mathf.Sin(f), Mathf.Cos(f));
        }

        public static Vector2[] GetLocalVertices(this VertexAttachment va, Slot slot, Vector2[] buffer)
        {
            int worldVerticesLength = va.worldVerticesLength;
            int num2 = worldVerticesLength >> 1;
            if (buffer == null)
            {
            }
            buffer = new Vector2[num2];
            if (buffer.Length < num2)
            {
                throw new ArgumentException($"Vector2 buffer too small. {va.Name} requires an array of size {worldVerticesLength}. Use the attachment's .WorldVerticesLength to get the correct size.", "buffer");
            }
            if (va.bones == null)
            {
                float[] vertices = va.vertices;
                for (int j = 0; j < num2; j++)
                {
                    int index = j * 2;
                    buffer[j] = new Vector2(vertices[index], vertices[index + 1]);
                }
                return buffer;
            }
            float[] worldVertices = new float[worldVerticesLength];
            va.ComputeWorldVertices(slot, worldVertices);
            Bone bone = slot.bone;
            float worldX = bone.worldX;
            float worldY = bone.worldY;
            bone.GetWorldToLocalMatrix(out float num5, out float num6, out float num7, out float num8);
            for (int i = 0; i < num2; i++)
            {
                int index = i * 2;
                float num13 = worldVertices[index] - worldX;
                float num14 = worldVertices[index + 1] - worldY;
                buffer[i] = new Vector2((num13 * num5) + (num14 * num6), (num13 * num7) + (num14 * num8));
            }
            return buffer;
        }

        public static Material GetMaterial(this Attachment a)
        {
            object rendererObject = null;
            IHasRendererObject obj3 = a as IHasRendererObject;
            if (obj3 != null)
            {
                rendererObject = obj3.RendererObject;
            }
            if (rendererObject == null)
            {
                return null;
            }
            return (Material) ((AtlasRegion) rendererObject).page.rendererObject;
        }

        public static Matrix4x4 GetMatrix4x4(this Bone bone) => 
            new Matrix4x4 { 
                m00 = bone.a,
                m01 = bone.b,
                m03 = bone.worldX,
                m10 = bone.c,
                m11 = bone.d,
                m13 = bone.worldY,
                m33 = 1f
            };

        public static Quaternion GetQuaternion(this Bone bone)
        {
            float f = Mathf.Atan2(bone.c, bone.a) * 0.5f;
            return new Quaternion(0f, 0f, Mathf.Sin(f), Mathf.Cos(f));
        }

        public static Vector2 GetSkeletonSpacePosition(this Bone bone) => 
            new Vector2(bone.worldX, bone.worldY);

        public static Vector2 GetSkeletonSpacePosition(this Bone bone, Vector2 boneLocal)
        {
            Vector2 vector;
            bone.LocalToWorld(boneLocal.x, boneLocal.y, out vector.x, out vector.y);
            return vector;
        }

        public static Vector3 GetWorldPosition(this Bone bone, Transform spineGameObjectTransform) => 
            spineGameObjectTransform.TransformPoint(new Vector3(bone.worldX, bone.worldY));

        public static Vector3 GetWorldPosition(this Bone bone, Transform spineGameObjectTransform, float positionScale) => 
            spineGameObjectTransform.TransformPoint(new Vector3(bone.worldX * positionScale, bone.worldY * positionScale));

        public static Vector3 GetWorldPosition(this PointAttachment attachment, Bone bone, Transform spineGameObjectTransform)
        {
            Vector3 vector;
            vector.z = 0f;
            attachment.ComputeWorldPosition(bone, out vector.x, out vector.y);
            return spineGameObjectTransform.TransformPoint(vector);
        }

        public static Vector3 GetWorldPosition(this PointAttachment attachment, Slot slot, Transform spineGameObjectTransform)
        {
            Vector3 vector;
            vector.z = 0f;
            attachment.ComputeWorldPosition(slot.bone, out vector.x, out vector.y);
            return spineGameObjectTransform.TransformPoint(vector);
        }

        public static void GetWorldToLocalMatrix(this Bone bone, out float ia, out float ib, out float ic, out float id)
        {
            float a = bone.a;
            float b = bone.b;
            float c = bone.c;
            float d = bone.d;
            float num5 = 1f / ((a * d) - (b * c));
            ia = num5 * d;
            ib = num5 * -b;
            ic = num5 * -c;
            id = num5 * a;
        }

        public static Vector2[] GetWorldVertices(this VertexAttachment a, Slot slot, Vector2[] buffer)
        {
            int worldVerticesLength = a.worldVerticesLength;
            int num2 = worldVerticesLength >> 1;
            if (buffer == null)
            {
            }
            buffer = new Vector2[num2];
            if (buffer.Length < num2)
            {
                throw new ArgumentException($"Vector2 buffer too small. {a.Name} requires an array of size {worldVerticesLength}. Use the attachment's .WorldVerticesLength to get the correct size.", "buffer");
            }
            float[] worldVertices = new float[worldVerticesLength];
            a.ComputeWorldVertices(slot, worldVertices);
            int index = 0;
            int num4 = worldVerticesLength >> 1;
            while (index < num4)
            {
                int num5 = index * 2;
                buffer[index] = new Vector2(worldVertices[num5], worldVertices[num5 + 1]);
                index++;
            }
            return buffer;
        }

        public static void SetColor(this MeshAttachment attachment, Color color)
        {
            attachment.A = color.a;
            attachment.R = color.r;
            attachment.G = color.g;
            attachment.B = color.b;
        }

        public static void SetColor(this MeshAttachment attachment, Color32 color)
        {
            attachment.A = color.a * 0.003921569f;
            attachment.R = color.r * 0.003921569f;
            attachment.G = color.g * 0.003921569f;
            attachment.B = color.b * 0.003921569f;
        }

        public static void SetColor(this RegionAttachment attachment, Color color)
        {
            attachment.A = color.a;
            attachment.R = color.r;
            attachment.G = color.g;
            attachment.B = color.b;
        }

        public static void SetColor(this RegionAttachment attachment, Color32 color)
        {
            attachment.A = color.a * 0.003921569f;
            attachment.R = color.r * 0.003921569f;
            attachment.G = color.g * 0.003921569f;
            attachment.B = color.b * 0.003921569f;
        }

        public static void SetColor(this Skeleton skeleton, Color color)
        {
            skeleton.A = color.a;
            skeleton.R = color.r;
            skeleton.G = color.g;
            skeleton.B = color.b;
        }

        public static void SetColor(this Skeleton skeleton, Color32 color)
        {
            skeleton.A = color.a * 0.003921569f;
            skeleton.R = color.r * 0.003921569f;
            skeleton.G = color.g * 0.003921569f;
            skeleton.B = color.b * 0.003921569f;
        }

        public static void SetColor(this Slot slot, Color color)
        {
            slot.A = color.a;
            slot.R = color.r;
            slot.G = color.g;
            slot.B = color.b;
        }

        public static void SetColor(this Slot slot, Color32 color)
        {
            slot.A = color.a * 0.003921569f;
            slot.R = color.r * 0.003921569f;
            slot.G = color.g * 0.003921569f;
            slot.B = color.b * 0.003921569f;
        }

        public static void SetPosition(this Bone bone, Vector2 position)
        {
            bone.X = position.x;
            bone.Y = position.y;
        }

        public static void SetPosition(this Bone bone, Vector3 position)
        {
            bone.X = position.x;
            bone.Y = position.y;
        }

        public static Vector2 SetPositionSkeletonSpace(this Bone bone, Vector2 skeletonSpacePosition)
        {
            if (bone.parent == null)
            {
                bone.SetPosition(skeletonSpacePosition);
                return skeletonSpacePosition;
            }
            Vector2 position = bone.parent.WorldToLocal(skeletonSpacePosition);
            bone.SetPosition(position);
            return position;
        }

        public static Vector2 WorldToLocal(this Bone bone, Vector2 worldPosition)
        {
            Vector2 vector;
            bone.WorldToLocal(worldPosition.x, worldPosition.y, out vector.x, out vector.y);
            return vector;
        }
    }
}

