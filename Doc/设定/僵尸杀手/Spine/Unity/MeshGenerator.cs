namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class MeshGenerator
    {
        public Settings settings = Settings.Default;
        private const float BoundsMinDefault = float.PositiveInfinity;
        private const float BoundsMaxDefault = float.NegativeInfinity;
        [NonSerialized]
        private readonly ExposedList<Vector3> vertexBuffer = new ExposedList<Vector3>(4);
        [NonSerialized]
        private readonly ExposedList<Vector2> uvBuffer = new ExposedList<Vector2>(4);
        [NonSerialized]
        private readonly ExposedList<Color32> colorBuffer = new ExposedList<Color32>(4);
        [NonSerialized]
        private readonly ExposedList<ExposedList<int>> submeshes;
        [NonSerialized]
        private Vector2 meshBoundsMin;
        [NonSerialized]
        private Vector2 meshBoundsMax;
        [NonSerialized]
        private float meshBoundsThickness;
        [NonSerialized]
        private int submeshIndex;
        [NonSerialized]
        private SkeletonClipping clipper;
        [NonSerialized]
        private float[] tempVerts;
        [NonSerialized]
        private int[] regionTriangles;
        [NonSerialized]
        private Vector3[] normals;
        [NonSerialized]
        private Vector4[] tangents;
        [NonSerialized]
        private Vector2[] tempTanBuffer;
        [NonSerialized]
        private ExposedList<Vector2> uv2;
        [NonSerialized]
        private ExposedList<Vector2> uv3;
        private static List<Vector3> AttachmentVerts = new List<Vector3>();
        private static List<Vector2> AttachmentUVs = new List<Vector2>();
        private static List<Color32> AttachmentColors32 = new List<Color32>();
        private static List<int> AttachmentIndices = new List<int>();

        public MeshGenerator()
        {
            ExposedList<ExposedList<int>> list = new ExposedList<ExposedList<int>> {
                new ExposedList<int>(6)
            };
            this.submeshes = list;
            this.clipper = new SkeletonClipping();
            this.tempVerts = new float[8];
            this.regionTriangles = new int[] { 0, 1, 2, 2, 3, 0 };
        }

        private void AddAttachmentTintBlack(float r2, float g2, float b2, int vertexCount)
        {
            Vector2 vector = new Vector2(r2, g2);
            Vector2 vector2 = new Vector2(b2, 1f);
            int count = this.vertexBuffer.Count;
            int newSize = count + vertexCount;
            if (this.uv2 == null)
            {
                this.uv2 = new ExposedList<Vector2>();
                this.uv3 = new ExposedList<Vector2>();
            }
            if (newSize > this.uv2.Items.Length)
            {
                Array.Resize<Vector2>(ref this.uv2.Items, newSize);
                Array.Resize<Vector2>(ref this.uv3.Items, newSize);
            }
            this.uv2.Count = this.uv3.Count = newSize;
            Vector2[] items = this.uv2.Items;
            Vector2[] vectorArray2 = this.uv3.Items;
            for (int i = 0; i < vertexCount; i++)
            {
                items[count + i] = vector;
                vectorArray2[count + i] = vector2;
            }
        }

        public void AddSubmesh(SubmeshInstruction instruction, bool updateTriangles = true)
        {
            Settings settings = this.settings;
            if ((this.submeshes.Count - 1) < this.submeshIndex)
            {
                this.submeshes.Resize(this.submeshIndex + 1);
                if (this.submeshes.Items[this.submeshIndex] == null)
                {
                    this.submeshes.Items[this.submeshIndex] = new ExposedList<int>();
                }
            }
            ExposedList<int> list = this.submeshes.Items[this.submeshIndex];
            list.Clear(false);
            Skeleton skeleton = instruction.skeleton;
            Slot[] items = skeleton.drawOrder.Items;
            Color32 color = new Color32();
            float num = skeleton.a * 255f;
            float r = skeleton.r;
            float g = skeleton.g;
            float b = skeleton.b;
            Vector2 meshBoundsMin = this.meshBoundsMin;
            Vector2 meshBoundsMax = this.meshBoundsMax;
            float zSpacing = settings.zSpacing;
            bool pmaVertexColors = settings.pmaVertexColors;
            bool tintBlack = settings.tintBlack;
            bool flag3 = settings.useClipping && instruction.hasClipping;
            if (flag3 && (instruction.preActiveClippingSlotSource >= 0))
            {
                Slot slot = items[instruction.preActiveClippingSlotSource];
                this.clipper.ClipStart(slot, slot.attachment as ClippingAttachment);
            }
            for (int i = instruction.startSlot; i < instruction.endSlot; i++)
            {
                float[] uvs;
                int[] regionTriangles;
                int num8;
                int num9;
                Slot slot = items[i];
                Attachment attachment = slot.attachment;
                float num7 = zSpacing * i;
                float[] tempVerts = this.tempVerts;
                Color color3 = new Color();
                RegionAttachment attachment2 = attachment as RegionAttachment;
                if (attachment2 != null)
                {
                    attachment2.ComputeWorldVertices(slot.bone, tempVerts, 0, 2);
                    uvs = attachment2.uvs;
                    regionTriangles = this.regionTriangles;
                    color3.r = attachment2.r;
                    color3.g = attachment2.g;
                    color3.b = attachment2.b;
                    color3.a = attachment2.a;
                    num8 = 4;
                    num9 = 6;
                }
                else
                {
                    MeshAttachment attachment3 = attachment as MeshAttachment;
                    if (attachment3 != null)
                    {
                        int worldVerticesLength = attachment3.worldVerticesLength;
                        if (tempVerts.Length < worldVerticesLength)
                        {
                            tempVerts = new float[worldVerticesLength];
                            this.tempVerts = tempVerts;
                        }
                        attachment3.ComputeWorldVertices(slot, 0, worldVerticesLength, tempVerts, 0, 2);
                        uvs = attachment3.uvs;
                        regionTriangles = attachment3.triangles;
                        color3.r = attachment3.r;
                        color3.g = attachment3.g;
                        color3.b = attachment3.b;
                        color3.a = attachment3.a;
                        num8 = worldVerticesLength >> 1;
                        num9 = attachment3.triangles.Length;
                    }
                    else
                    {
                        if (flag3)
                        {
                            ClippingAttachment clip = attachment as ClippingAttachment;
                            if (clip != null)
                            {
                                this.clipper.ClipStart(slot, clip);
                                continue;
                            }
                        }
                        this.clipper.ClipEnd(slot);
                        continue;
                    }
                }
                if (pmaVertexColors)
                {
                    color.a = (byte) ((num * slot.a) * color3.a);
                    color.r = (byte) (((r * slot.r) * color3.r) * color.a);
                    color.g = (byte) (((g * slot.g) * color3.g) * color.a);
                    color.b = (byte) (((b * slot.b) * color3.b) * color.a);
                    if (slot.data.blendMode == BlendMode.Additive)
                    {
                        color.a = 0;
                    }
                }
                else
                {
                    color.a = (byte) ((num * slot.a) * color3.a);
                    color.r = (byte) (((r * slot.r) * color3.r) * 255f);
                    color.g = (byte) (((g * slot.g) * color3.g) * 255f);
                    color.b = (byte) (((b * slot.b) * color3.b) * 255f);
                }
                if (flag3 && this.clipper.IsClipping)
                {
                    this.clipper.ClipTriangles(tempVerts, num8 << 1, regionTriangles, num9, uvs);
                    tempVerts = this.clipper.clippedVertices.Items;
                    num8 = this.clipper.clippedVertices.Count >> 1;
                    regionTriangles = this.clipper.clippedTriangles.Items;
                    num9 = this.clipper.clippedTriangles.Count;
                    uvs = this.clipper.clippedUVs.Items;
                }
                if ((num8 != 0) && (num9 != 0))
                {
                    if (tintBlack)
                    {
                        this.AddAttachmentTintBlack(slot.r2, slot.g2, slot.b2, num8);
                    }
                    int num11 = this.vertexBuffer.Count;
                    int newSize = num11 + num8;
                    if (newSize > this.vertexBuffer.Items.Length)
                    {
                        Array.Resize<Vector3>(ref this.vertexBuffer.Items, newSize);
                        Array.Resize<Vector2>(ref this.uvBuffer.Items, newSize);
                        Array.Resize<Color32>(ref this.colorBuffer.Items, newSize);
                    }
                    this.vertexBuffer.Count = this.uvBuffer.Count = this.colorBuffer.Count = newSize;
                    Vector3[] vectorArray = this.vertexBuffer.Items;
                    Vector2[] vectorArray2 = this.uvBuffer.Items;
                    Color32[] colorArray = this.colorBuffer.Items;
                    if (num11 == 0)
                    {
                        for (int j = 0; j < num8; j++)
                        {
                            int index = num11 + j;
                            int num16 = j << 1;
                            float num17 = tempVerts[num16];
                            float num18 = tempVerts[num16 + 1];
                            vectorArray[index].x = num17;
                            vectorArray[index].y = num18;
                            vectorArray[index].z = num7;
                            vectorArray2[index].x = uvs[num16];
                            vectorArray2[index].y = uvs[num16 + 1];
                            colorArray[index] = color;
                            if (num17 < meshBoundsMin.x)
                            {
                                meshBoundsMin.x = num17;
                            }
                            if (num17 > meshBoundsMax.x)
                            {
                                meshBoundsMax.x = num17;
                            }
                            if (num18 < meshBoundsMin.y)
                            {
                                meshBoundsMin.y = num18;
                            }
                            if (num18 > meshBoundsMax.y)
                            {
                                meshBoundsMax.y = num18;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < num8; j++)
                        {
                            int index = num11 + j;
                            int num21 = j << 1;
                            float num22 = tempVerts[num21];
                            float num23 = tempVerts[num21 + 1];
                            vectorArray[index].x = num22;
                            vectorArray[index].y = num23;
                            vectorArray[index].z = num7;
                            vectorArray2[index].x = uvs[num21];
                            vectorArray2[index].y = uvs[num21 + 1];
                            colorArray[index] = color;
                            if (num22 < meshBoundsMin.x)
                            {
                                meshBoundsMin.x = num22;
                            }
                            else if (num22 > meshBoundsMax.x)
                            {
                                meshBoundsMax.x = num22;
                            }
                            if (num23 < meshBoundsMin.y)
                            {
                                meshBoundsMin.y = num23;
                            }
                            else if (num23 > meshBoundsMax.y)
                            {
                                meshBoundsMax.y = num23;
                            }
                        }
                    }
                    if (updateTriangles)
                    {
                        int num24 = list.Count;
                        int num25 = num24 + num9;
                        if (num25 > list.Items.Length)
                        {
                            Array.Resize<int>(ref list.Items, num25);
                        }
                        list.Count = num25;
                        int[] numArray4 = list.Items;
                        for (int j = 0; j < num9; j++)
                        {
                            numArray4[num24 + j] = regionTriangles[j] + num11;
                        }
                    }
                }
                this.clipper.ClipEnd(slot);
            }
            this.clipper.ClipEnd();
            this.meshBoundsMin = meshBoundsMin;
            this.meshBoundsMax = meshBoundsMax;
            this.meshBoundsThickness = instruction.endSlot * zSpacing;
            int[] numArray5 = list.Items;
            int count = list.Count;
            int length = numArray5.Length;
            while (count < length)
            {
                numArray5[count] = 0;
                count++;
            }
            this.submeshIndex++;
        }

        public void Begin()
        {
            this.vertexBuffer.Clear(false);
            this.colorBuffer.Clear(false);
            this.uvBuffer.Clear(false);
            this.clipper.ClipEnd();
            this.meshBoundsMin.x = float.PositiveInfinity;
            this.meshBoundsMin.y = float.PositiveInfinity;
            this.meshBoundsMax.x = float.NegativeInfinity;
            this.meshBoundsMax.y = float.NegativeInfinity;
            this.meshBoundsThickness = 0f;
            this.submeshIndex = 0;
            this.submeshes.Count = 1;
        }

        public void BuildMesh(SkeletonRendererInstruction instruction, bool updateTriangles)
        {
            SubmeshInstruction[] items = instruction.submeshInstructions.Items;
            int index = 0;
            int count = instruction.submeshInstructions.Count;
            while (index < count)
            {
                this.AddSubmesh(items[index], updateTriangles);
                index++;
            }
        }

        public void BuildMeshWithArrays(SkeletonRendererInstruction instruction, bool updateTriangles)
        {
            Settings settings = this.settings;
            int rawVertexCount = instruction.rawVertexCount;
            if (rawVertexCount > this.vertexBuffer.Items.Length)
            {
                Array.Resize<Vector3>(ref this.vertexBuffer.Items, rawVertexCount);
                Array.Resize<Vector2>(ref this.uvBuffer.Items, rawVertexCount);
                Array.Resize<Color32>(ref this.colorBuffer.Items, rawVertexCount);
            }
            this.vertexBuffer.Count = this.uvBuffer.Count = this.colorBuffer.Count = rawVertexCount;
            Color32 color = new Color32();
            int index = 0;
            float[] tempVerts = this.tempVerts;
            Vector3 meshBoundsMin = (Vector3) this.meshBoundsMin;
            Vector3 meshBoundsMax = (Vector3) this.meshBoundsMax;
            Vector3[] items = this.vertexBuffer.Items;
            Vector2[] vectorArray2 = this.uvBuffer.Items;
            Color32[] colorArray = this.colorBuffer.Items;
            int num4 = 0;
            int num5 = 0;
            int count = instruction.submeshInstructions.Count;
            while (num5 < count)
            {
                SubmeshInstruction instruction2 = instruction.submeshInstructions.Items[num5];
                Skeleton skeleton = instruction2.skeleton;
                Slot[] slotArray = skeleton.drawOrder.Items;
                float num7 = skeleton.a * 255f;
                float r = skeleton.r;
                float g = skeleton.g;
                float b = skeleton.b;
                int endSlot = instruction2.endSlot;
                int startSlot = instruction2.startSlot;
                num4 = endSlot;
                if (settings.tintBlack)
                {
                    Vector2 vector4;
                    int num13 = index;
                    vector4.y = 1f;
                    if (this.uv2 == null)
                    {
                        this.uv2 = new ExposedList<Vector2>();
                        this.uv3 = new ExposedList<Vector2>();
                    }
                    if (rawVertexCount > this.uv2.Items.Length)
                    {
                        Array.Resize<Vector2>(ref this.uv2.Items, rawVertexCount);
                        Array.Resize<Vector2>(ref this.uv3.Items, rawVertexCount);
                    }
                    this.uv2.Count = this.uv3.Count = rawVertexCount;
                    Vector2[] vectorArray3 = this.uv2.Items;
                    Vector2[] vectorArray4 = this.uv3.Items;
                    for (int j = startSlot; j < endSlot; j++)
                    {
                        Vector2 vector3;
                        Slot slot = slotArray[j];
                        Attachment attachment = slot.attachment;
                        vector3.x = slot.r2;
                        vector3.y = slot.g2;
                        vector4.x = slot.b2;
                        if (attachment is RegionAttachment)
                        {
                            vectorArray3[num13] = vector3;
                            vectorArray3[num13 + 1] = vector3;
                            vectorArray3[num13 + 2] = vector3;
                            vectorArray3[num13 + 3] = vector3;
                            vectorArray4[num13] = vector4;
                            vectorArray4[num13 + 1] = vector4;
                            vectorArray4[num13 + 2] = vector4;
                            vectorArray4[num13 + 3] = vector4;
                            num13 += 4;
                        }
                        else
                        {
                            MeshAttachment attachment3 = attachment as MeshAttachment;
                            if (attachment3 != null)
                            {
                                int worldVerticesLength = attachment3.worldVerticesLength;
                                for (int k = 0; k < worldVerticesLength; k += 2)
                                {
                                    vectorArray3[num13] = vector3;
                                    vectorArray4[num13] = vector4;
                                    num13++;
                                }
                            }
                        }
                    }
                }
                for (int i = startSlot; i < endSlot; i++)
                {
                    Slot slot = slotArray[i];
                    Attachment attachment = slot.attachment;
                    float num18 = i * settings.zSpacing;
                    RegionAttachment attachment5 = attachment as RegionAttachment;
                    if (attachment5 != null)
                    {
                        attachment5.ComputeWorldVertices(slot.bone, tempVerts, 0, 2);
                        float num19 = tempVerts[0];
                        float num20 = tempVerts[1];
                        float num21 = tempVerts[2];
                        float num22 = tempVerts[3];
                        float num23 = tempVerts[4];
                        float num24 = tempVerts[5];
                        float num25 = tempVerts[6];
                        float num26 = tempVerts[7];
                        items[index].x = num19;
                        items[index].y = num20;
                        items[index].z = num18;
                        items[index + 1].x = num25;
                        items[index + 1].y = num26;
                        items[index + 1].z = num18;
                        items[index + 2].x = num21;
                        items[index + 2].y = num22;
                        items[index + 2].z = num18;
                        items[index + 3].x = num23;
                        items[index + 3].y = num24;
                        items[index + 3].z = num18;
                        if (settings.pmaVertexColors)
                        {
                            color.a = (byte) ((num7 * slot.a) * attachment5.a);
                            color.r = (byte) (((r * slot.r) * attachment5.r) * color.a);
                            color.g = (byte) (((g * slot.g) * attachment5.g) * color.a);
                            color.b = (byte) (((b * slot.b) * attachment5.b) * color.a);
                            if (slot.data.blendMode == BlendMode.Additive)
                            {
                                color.a = 0;
                            }
                        }
                        else
                        {
                            color.a = (byte) ((num7 * slot.a) * attachment5.a);
                            color.r = (byte) (((r * slot.r) * attachment5.r) * 255f);
                            color.g = (byte) (((g * slot.g) * attachment5.g) * 255f);
                            color.b = (byte) (((b * slot.b) * attachment5.b) * 255f);
                        }
                        colorArray[index] = color;
                        colorArray[index + 1] = color;
                        colorArray[index + 2] = color;
                        colorArray[index + 3] = color;
                        float[] uvs = attachment5.uvs;
                        vectorArray2[index].x = uvs[0];
                        vectorArray2[index].y = uvs[1];
                        vectorArray2[index + 1].x = uvs[6];
                        vectorArray2[index + 1].y = uvs[7];
                        vectorArray2[index + 2].x = uvs[2];
                        vectorArray2[index + 2].y = uvs[3];
                        vectorArray2[index + 3].x = uvs[4];
                        vectorArray2[index + 3].y = uvs[5];
                        if (num19 < meshBoundsMin.x)
                        {
                            meshBoundsMin.x = num19;
                        }
                        if (num19 > meshBoundsMax.x)
                        {
                            meshBoundsMax.x = num19;
                        }
                        if (num21 < meshBoundsMin.x)
                        {
                            meshBoundsMin.x = num21;
                        }
                        else if (num21 > meshBoundsMax.x)
                        {
                            meshBoundsMax.x = num21;
                        }
                        if (num23 < meshBoundsMin.x)
                        {
                            meshBoundsMin.x = num23;
                        }
                        else if (num23 > meshBoundsMax.x)
                        {
                            meshBoundsMax.x = num23;
                        }
                        if (num25 < meshBoundsMin.x)
                        {
                            meshBoundsMin.x = num25;
                        }
                        else if (num25 > meshBoundsMax.x)
                        {
                            meshBoundsMax.x = num25;
                        }
                        if (num20 < meshBoundsMin.y)
                        {
                            meshBoundsMin.y = num20;
                        }
                        if (num20 > meshBoundsMax.y)
                        {
                            meshBoundsMax.y = num20;
                        }
                        if (num22 < meshBoundsMin.y)
                        {
                            meshBoundsMin.y = num22;
                        }
                        else if (num22 > meshBoundsMax.y)
                        {
                            meshBoundsMax.y = num22;
                        }
                        if (num24 < meshBoundsMin.y)
                        {
                            meshBoundsMin.y = num24;
                        }
                        else if (num24 > meshBoundsMax.y)
                        {
                            meshBoundsMax.y = num24;
                        }
                        if (num26 < meshBoundsMin.y)
                        {
                            meshBoundsMin.y = num26;
                        }
                        else if (num26 > meshBoundsMax.y)
                        {
                            meshBoundsMax.y = num26;
                        }
                        index += 4;
                    }
                    else
                    {
                        MeshAttachment attachment6 = attachment as MeshAttachment;
                        if (attachment6 != null)
                        {
                            int worldVerticesLength = attachment6.worldVerticesLength;
                            if (tempVerts.Length < worldVerticesLength)
                            {
                                this.tempVerts = tempVerts = new float[worldVerticesLength];
                            }
                            attachment6.ComputeWorldVertices(slot, tempVerts);
                            if (settings.pmaVertexColors)
                            {
                                color.a = (byte) ((num7 * slot.a) * attachment6.a);
                                color.r = (byte) (((r * slot.r) * attachment6.r) * color.a);
                                color.g = (byte) (((g * slot.g) * attachment6.g) * color.a);
                                color.b = (byte) (((b * slot.b) * attachment6.b) * color.a);
                                if (slot.data.blendMode == BlendMode.Additive)
                                {
                                    color.a = 0;
                                }
                            }
                            else
                            {
                                color.a = (byte) ((num7 * slot.a) * attachment6.a);
                                color.r = (byte) (((r * slot.r) * attachment6.r) * 255f);
                                color.g = (byte) (((g * slot.g) * attachment6.g) * 255f);
                                color.b = (byte) (((b * slot.b) * attachment6.b) * 255f);
                            }
                            float[] uvs = attachment6.uvs;
                            if (index == 0)
                            {
                                float num28 = tempVerts[0];
                                float num29 = tempVerts[1];
                                if (num28 < meshBoundsMin.x)
                                {
                                    meshBoundsMin.x = num28;
                                }
                                if (num28 > meshBoundsMax.x)
                                {
                                    meshBoundsMax.x = num28;
                                }
                                if (num29 < meshBoundsMin.y)
                                {
                                    meshBoundsMin.y = num29;
                                }
                                if (num29 > meshBoundsMax.y)
                                {
                                    meshBoundsMax.y = num29;
                                }
                            }
                            for (int j = 0; j < worldVerticesLength; j += 2)
                            {
                                float num31 = tempVerts[j];
                                float num32 = tempVerts[j + 1];
                                items[index].x = num31;
                                items[index].y = num32;
                                items[index].z = num18;
                                colorArray[index] = color;
                                vectorArray2[index].x = uvs[j];
                                vectorArray2[index].y = uvs[j + 1];
                                if (num31 < meshBoundsMin.x)
                                {
                                    meshBoundsMin.x = num31;
                                }
                                else if (num31 > meshBoundsMax.x)
                                {
                                    meshBoundsMax.x = num31;
                                }
                                if (num32 < meshBoundsMin.y)
                                {
                                    meshBoundsMin.y = num32;
                                }
                                else if (num32 > meshBoundsMax.y)
                                {
                                    meshBoundsMax.y = num32;
                                }
                                index++;
                            }
                        }
                    }
                }
                num5++;
            }
            this.meshBoundsMin = meshBoundsMin;
            this.meshBoundsMax = meshBoundsMax;
            this.meshBoundsThickness = num4 * settings.zSpacing;
            if (updateTriangles)
            {
                int newSize = instruction.submeshInstructions.Count;
                if (this.submeshes.Count < newSize)
                {
                    this.submeshes.Resize(newSize);
                    int num34 = 0;
                    int num35 = newSize;
                    while (num34 < num35)
                    {
                        ExposedList<int> list = this.submeshes.Items[num34];
                        if (list == null)
                        {
                            this.submeshes.Items[num34] = new ExposedList<int>();
                        }
                        else
                        {
                            list.Clear(false);
                        }
                        num34++;
                    }
                }
                SubmeshInstruction[] instructionArray = instruction.submeshInstructions.Items;
                int num36 = 0;
                for (int i = 0; i < newSize; i++)
                {
                    SubmeshInstruction instruction3 = instructionArray[i];
                    ExposedList<int> list2 = this.submeshes.Items[i];
                    int rawTriangleCount = instruction3.rawTriangleCount;
                    if (rawTriangleCount > list2.Items.Length)
                    {
                        Array.Resize<int>(ref list2.Items, rawTriangleCount);
                    }
                    else if (rawTriangleCount < list2.Items.Length)
                    {
                        int[] numArray4 = list2.Items;
                        int num39 = rawTriangleCount;
                        int length = numArray4.Length;
                        while (num39 < length)
                        {
                            numArray4[num39] = 0;
                            num39++;
                        }
                    }
                    list2.Count = rawTriangleCount;
                    int[] numArray5 = list2.Items;
                    int num41 = 0;
                    Slot[] slotArray2 = instruction3.skeleton.drawOrder.Items;
                    int startSlot = instruction3.startSlot;
                    int endSlot = instruction3.endSlot;
                    while (startSlot < endSlot)
                    {
                        Attachment attachment = slotArray2[startSlot].attachment;
                        if (attachment is RegionAttachment)
                        {
                            numArray5[num41] = num36;
                            numArray5[num41 + 1] = num36 + 2;
                            numArray5[num41 + 2] = num36 + 1;
                            numArray5[num41 + 3] = num36 + 2;
                            numArray5[num41 + 4] = num36 + 3;
                            numArray5[num41 + 5] = num36 + 1;
                            num41 += 6;
                            num36 += 4;
                        }
                        else
                        {
                            MeshAttachment attachment8 = attachment as MeshAttachment;
                            if (attachment8 != null)
                            {
                                int[] triangles = attachment8.triangles;
                                int num44 = 0;
                                int length = triangles.Length;
                                while (num44 < length)
                                {
                                    numArray5[num41] = num36 + triangles[num44];
                                    num44++;
                                    num41++;
                                }
                                num36 += attachment8.worldVerticesLength >> 1;
                            }
                        }
                        startSlot++;
                    }
                }
            }
        }

        public void FillLateVertexData(Mesh mesh)
        {
            if (this.settings.calculateTangents)
            {
                int count = this.vertexBuffer.Count;
                ExposedList<int>[] items = this.submeshes.Items;
                int num2 = this.submeshes.Count;
                Vector3[] vertices = this.vertexBuffer.Items;
                Vector2[] uvs = this.uvBuffer.Items;
                SolveTangents2DEnsureSize(ref this.tangents, ref this.tempTanBuffer, count);
                for (int i = 0; i < num2; i++)
                {
                    int[] triangles = items[i].Items;
                    int triangleCount = items[i].Count;
                    SolveTangents2DTriangles(this.tempTanBuffer, triangles, triangleCount, vertices, uvs, count);
                }
                SolveTangents2DBuffer(this.tangents, this.tempTanBuffer, count);
                mesh.tangents = this.tangents;
            }
        }

        public static void FillMeshLocal(Mesh mesh, RegionAttachment regionAttachment)
        {
            if ((mesh != null) && (regionAttachment != null))
            {
                AttachmentVerts.Clear();
                float[] offset = regionAttachment.Offset;
                AttachmentVerts.Add(new Vector3(offset[0], offset[1]));
                AttachmentVerts.Add(new Vector3(offset[2], offset[3]));
                AttachmentVerts.Add(new Vector3(offset[4], offset[5]));
                AttachmentVerts.Add(new Vector3(offset[6], offset[7]));
                AttachmentUVs.Clear();
                float[] uVs = regionAttachment.UVs;
                AttachmentUVs.Add(new Vector2(uVs[2], uVs[3]));
                AttachmentUVs.Add(new Vector2(uVs[4], uVs[5]));
                AttachmentUVs.Add(new Vector2(uVs[6], uVs[7]));
                AttachmentUVs.Add(new Vector2(uVs[0], uVs[1]));
                AttachmentColors32.Clear();
                Color32 item = new Color(regionAttachment.r, regionAttachment.g, regionAttachment.b, regionAttachment.a);
                for (int i = 0; i < 4; i++)
                {
                    AttachmentColors32.Add(item);
                }
                AttachmentIndices.Clear();
                AttachmentIndices.AddRange(new int[] { 0, 2, 1, 0, 3, 2 });
                mesh.Clear();
                mesh.name = regionAttachment.Name;
                mesh.SetVertices(AttachmentVerts);
                mesh.SetUVs(0, AttachmentUVs);
                mesh.SetColors(AttachmentColors32);
                mesh.SetTriangles(AttachmentIndices, 0);
                mesh.RecalculateBounds();
                AttachmentVerts.Clear();
                AttachmentUVs.Clear();
                AttachmentColors32.Clear();
                AttachmentIndices.Clear();
            }
        }

        public static void FillMeshLocal(Mesh mesh, MeshAttachment meshAttachment, SkeletonData skeletonData)
        {
            if ((mesh != null) && (meshAttachment != null))
            {
                int num = meshAttachment.WorldVerticesLength / 2;
                AttachmentVerts.Clear();
                if (meshAttachment.IsWeighted())
                {
                    int worldVerticesLength = meshAttachment.WorldVerticesLength;
                    int[] bones = meshAttachment.bones;
                    int index = 0;
                    float[] vertices = meshAttachment.vertices;
                    int num4 = 0;
                    int num5 = 0;
                    while (num4 < worldVerticesLength)
                    {
                        float x = 0f;
                        float y = 0f;
                        int num8 = bones[index++];
                        num8 += index;
                        while (index < num8)
                        {
                            BoneMatrix matrix = BoneMatrix.CalculateSetupWorld(skeletonData.bones.Items[bones[index]]);
                            float num9 = vertices[num5];
                            float num10 = vertices[num5 + 1];
                            float num11 = vertices[num5 + 2];
                            x += (((num9 * matrix.a) + (num10 * matrix.b)) + matrix.x) * num11;
                            y += (((num9 * matrix.c) + (num10 * matrix.d)) + matrix.y) * num11;
                            index++;
                            num5 += 3;
                        }
                        AttachmentVerts.Add(new Vector3(x, y));
                        num4 += 2;
                    }
                }
                else
                {
                    float[] vertices = meshAttachment.Vertices;
                    Vector3 vector = new Vector3();
                    for (int j = 0; j < num; j++)
                    {
                        int index = j * 2;
                        vector.x = vertices[index];
                        vector.y = vertices[index + 1];
                        AttachmentVerts.Add(vector);
                    }
                }
                float[] uvs = meshAttachment.uvs;
                Vector2 item = new Vector2();
                Color32 color = new Color(meshAttachment.r, meshAttachment.g, meshAttachment.b, meshAttachment.a);
                AttachmentUVs.Clear();
                AttachmentColors32.Clear();
                for (int i = 0; i < num; i++)
                {
                    int index = i * 2;
                    item.x = uvs[index];
                    item.y = uvs[index + 1];
                    AttachmentUVs.Add(item);
                    AttachmentColors32.Add(color);
                }
                AttachmentIndices.Clear();
                AttachmentIndices.AddRange(meshAttachment.triangles);
                mesh.Clear();
                mesh.name = meshAttachment.Name;
                mesh.SetVertices(AttachmentVerts);
                mesh.SetUVs(0, AttachmentUVs);
                mesh.SetColors(AttachmentColors32);
                mesh.SetTriangles(AttachmentIndices, 0);
                mesh.RecalculateBounds();
                AttachmentVerts.Clear();
                AttachmentUVs.Clear();
                AttachmentColors32.Clear();
                AttachmentIndices.Clear();
            }
        }

        public void FillTriangles(Mesh mesh)
        {
            int count = this.submeshes.Count;
            ExposedList<int>[] items = this.submeshes.Items;
            mesh.subMeshCount = count;
            for (int i = 0; i < count; i++)
            {
                mesh.SetTriangles(items[i].Items, i, false);
            }
        }

        public void FillTrianglesSingle(Mesh mesh)
        {
            mesh.SetTriangles(this.submeshes.Items[0].Items, 0, false);
        }

        public void FillVertexData(Mesh mesh)
        {
            Vector3[] items = this.vertexBuffer.Items;
            Vector2[] vectorArray2 = this.uvBuffer.Items;
            Color32[] colorArray = this.colorBuffer.Items;
            int count = this.vertexBuffer.Count;
            int length = this.vertexBuffer.Items.Length;
            Vector3 zero = Vector3.zero;
            for (int i = count; i < length; i++)
            {
                items[i] = zero;
            }
            mesh.vertices = items;
            mesh.uv = vectorArray2;
            mesh.colors32 = colorArray;
            if (float.IsInfinity(this.meshBoundsMin.x))
            {
                mesh.bounds = new Bounds();
            }
            else
            {
                Vector2 vector2 = (this.meshBoundsMax - this.meshBoundsMin) * 0.5f;
                Bounds bounds2 = new Bounds {
                    center = (Vector3) (this.meshBoundsMin + vector2),
                    extents = new Vector3(vector2.x, vector2.y, this.meshBoundsThickness * 0.5f)
                };
                mesh.bounds = bounds2;
            }
            int newSize = this.vertexBuffer.Count;
            if (this.settings.addNormals)
            {
                int num5 = 0;
                if (this.normals == null)
                {
                    this.normals = new Vector3[newSize];
                }
                else
                {
                    num5 = this.normals.Length;
                }
                if (num5 < newSize)
                {
                    Array.Resize<Vector3>(ref this.normals, newSize);
                    Vector3[] normals = this.normals;
                    for (int j = num5; j < newSize; j++)
                    {
                        normals[j] = Vector3.back;
                    }
                }
                mesh.normals = this.normals;
            }
            if (this.settings.tintBlack && (this.uv2 != null))
            {
                mesh.uv2 = this.uv2.Items;
                mesh.uv3 = this.uv3.Items;
            }
        }

        public static void GenerateSingleSubmeshInstruction(SkeletonRendererInstruction instructionOutput, Skeleton skeleton, Material material)
        {
            ExposedList<Slot> drawOrder = skeleton.drawOrder;
            int count = drawOrder.Count;
            instructionOutput.Clear();
            ExposedList<SubmeshInstruction> submeshInstructions = instructionOutput.submeshInstructions;
            submeshInstructions.Resize(1);
            instructionOutput.attachments.Resize(count);
            Attachment[] items = instructionOutput.attachments.Items;
            int num2 = 0;
            SubmeshInstruction instruction = new SubmeshInstruction {
                skeleton = skeleton,
                preActiveClippingSlotSource = -1,
                startSlot = 0,
                rawFirstVertexIndex = 0,
                material = material,
                forceSeparate = false,
                endSlot = count
            };
            bool flag = false;
            Slot[] slotArray = drawOrder.Items;
            for (int i = 0; i < count; i++)
            {
                int length;
                int num5;
                Slot slot = slotArray[i];
                Attachment attachment = slot.attachment;
                items[i] = attachment;
                if (attachment is RegionAttachment)
                {
                    num5 = 4;
                    length = 6;
                }
                else
                {
                    MeshAttachment attachment3 = attachment as MeshAttachment;
                    if (attachment3 != null)
                    {
                        num5 = attachment3.worldVerticesLength >> 1;
                        length = attachment3.triangles.Length;
                    }
                    else
                    {
                        if (attachment is ClippingAttachment)
                        {
                            instruction.hasClipping = true;
                            flag = true;
                        }
                        num5 = 0;
                        length = 0;
                    }
                }
                instruction.rawTriangleCount += length;
                instruction.rawVertexCount += num5;
                num2 += num5;
            }
            instructionOutput.hasActiveClipping = flag;
            instructionOutput.rawVertexCount = num2;
            submeshInstructions.Items[0] = instruction;
        }

        public static void GenerateSkeletonRendererInstruction(SkeletonRendererInstruction instructionOutput, Skeleton skeleton, Dictionary<Slot, Material> customSlotMaterials, List<Slot> separatorSlots, bool generateMeshOverride, bool immutableTriangles = false)
        {
            ExposedList<Slot> drawOrder = skeleton.drawOrder;
            int count = drawOrder.Count;
            instructionOutput.Clear();
            ExposedList<SubmeshInstruction> submeshInstructions = instructionOutput.submeshInstructions;
            instructionOutput.attachments.Resize(count);
            Attachment[] items = instructionOutput.attachments.Items;
            int num2 = 0;
            bool flag = false;
            SubmeshInstruction instruction = new SubmeshInstruction {
                skeleton = skeleton,
                preActiveClippingSlotSource = -1
            };
            bool flag2 = (customSlotMaterials != null) && (customSlotMaterials.Count > 0);
            int num3 = (separatorSlots != null) ? separatorSlots.Count : 0;
            bool flag3 = num3 > 0;
            int num4 = -1;
            int num5 = -1;
            SlotData endSlot = null;
            int index = 0;
            Slot[] slotArray = drawOrder.Items;
            for (int i = 0; i < count; i++)
            {
                Slot objA = slotArray[i];
                Attachment attachment = objA.attachment;
                items[i] = attachment;
                int num8 = 0;
                int length = 0;
                object rendererObject = null;
                bool flag4 = false;
                RegionAttachment attachment2 = attachment as RegionAttachment;
                if (attachment2 != null)
                {
                    rendererObject = attachment2.RendererObject;
                    num8 = 4;
                    length = 6;
                }
                else
                {
                    MeshAttachment attachment3 = attachment as MeshAttachment;
                    if (attachment3 != null)
                    {
                        rendererObject = attachment3.RendererObject;
                        num8 = attachment3.worldVerticesLength >> 1;
                        length = attachment3.triangles.Length;
                    }
                    else
                    {
                        ClippingAttachment attachment4 = attachment as ClippingAttachment;
                        if (attachment4 != null)
                        {
                            endSlot = attachment4.endSlot;
                            num4 = i;
                            instruction.hasClipping = true;
                            flag = true;
                        }
                        flag4 = true;
                    }
                }
                if (((endSlot != null) && (objA.data == endSlot)) && (i != num4))
                {
                    endSlot = null;
                    num4 = -1;
                }
                if (flag3)
                {
                    instruction.forceSeparate = false;
                    for (int j = 0; j < num3; j++)
                    {
                        if (object.ReferenceEquals(objA, separatorSlots[j]))
                        {
                            instruction.forceSeparate = true;
                            break;
                        }
                    }
                }
                if (flag4)
                {
                    if (instruction.forceSeparate && generateMeshOverride)
                    {
                        instruction.endSlot = i;
                        instruction.preActiveClippingSlotSource = num5;
                        submeshInstructions.Resize(index + 1);
                        submeshInstructions.Items[index] = instruction;
                        index++;
                        instruction.startSlot = i;
                        num5 = num4;
                        instruction.rawTriangleCount = 0;
                        instruction.rawVertexCount = 0;
                        instruction.rawFirstVertexIndex = num2;
                        instruction.hasClipping = num4 >= 0;
                    }
                }
                else
                {
                    if (flag2)
                    {
                        if (!customSlotMaterials.TryGetValue(objA, out Material rendererObject))
                        {
                            rendererObject = (Material) ((AtlasRegion) rendererObject).page.rendererObject;
                        }
                    }
                    else
                    {
                        rendererObject = (Material) ((AtlasRegion) rendererObject).page.rendererObject;
                    }
                    if (instruction.forceSeparate || ((instruction.rawVertexCount > 0) && !object.ReferenceEquals(instruction.material, rendererObject)))
                    {
                        instruction.endSlot = i;
                        instruction.preActiveClippingSlotSource = num5;
                        submeshInstructions.Resize(index + 1);
                        submeshInstructions.Items[index] = instruction;
                        index++;
                        instruction.startSlot = i;
                        num5 = num4;
                        instruction.rawTriangleCount = 0;
                        instruction.rawVertexCount = 0;
                        instruction.rawFirstVertexIndex = num2;
                        instruction.hasClipping = num4 >= 0;
                    }
                    instruction.material = rendererObject;
                    instruction.rawTriangleCount += length;
                    instruction.rawVertexCount += num8;
                    instruction.rawFirstVertexIndex = num2;
                    num2 += num8;
                }
            }
            if (instruction.rawVertexCount > 0)
            {
                instruction.endSlot = count;
                instruction.preActiveClippingSlotSource = num5;
                instruction.forceSeparate = false;
                submeshInstructions.Resize(index + 1);
                submeshInstructions.Items[index] = instruction;
            }
            instructionOutput.hasActiveClipping = flag;
            instructionOutput.rawVertexCount = num2;
            instructionOutput.immutableTriangles = immutableTriangles;
        }

        public void ScaleVertexData(float scale)
        {
            Vector3[] items = this.vertexBuffer.Items;
            int index = 0;
            int count = this.vertexBuffer.Count;
            while (index < count)
            {
                items[index] *= scale;
                index++;
            }
            this.meshBoundsMin *= scale;
            this.meshBoundsMax *= scale;
            this.meshBoundsThickness *= scale;
        }

        internal static void SolveTangents2DBuffer(Vector4[] tangents, Vector2[] tempTanBuffer, int vertexCount)
        {
            Vector4 vector;
            vector.z = 0f;
            for (int i = 0; i < vertexCount; i++)
            {
                Vector2 vector2 = tempTanBuffer[i];
                float num2 = Mathf.Sqrt((vector2.x * vector2.x) + (vector2.y * vector2.y));
                if (num2 > 1E-05)
                {
                    float num3 = 1f / num2;
                    vector2.x *= num3;
                    vector2.y *= num3;
                }
                Vector2 vector3 = tempTanBuffer[vertexCount + i];
                vector.x = vector2.x;
                vector.y = vector2.y;
                vector.w = ((vector2.y * vector3.x) <= (vector2.x * vector3.y)) ? ((float) (-1)) : ((float) 1);
                tangents[i] = vector;
            }
        }

        internal static void SolveTangents2DEnsureSize(ref Vector4[] tangentBuffer, ref Vector2[] tempTanBuffer, int vertexCount)
        {
            if ((tangentBuffer == null) || (tangentBuffer.Length < vertexCount))
            {
                tangentBuffer = new Vector4[vertexCount];
            }
            if ((tempTanBuffer == null) || (tempTanBuffer.Length < (vertexCount * 2)))
            {
                tempTanBuffer = new Vector2[vertexCount * 2];
            }
        }

        internal static void SolveTangents2DTriangles(Vector2[] tempTanBuffer, int[] triangles, int triangleCount, Vector3[] vertices, Vector2[] uvs, int vertexCount)
        {
            for (int i = 0; i < triangleCount; i += 3)
            {
                Vector2 vector;
                Vector2 vector2;
                Vector2 vector9;
                int index = triangles[i];
                int num3 = triangles[i + 1];
                int num4 = triangles[i + 2];
                Vector3 vector3 = vertices[index];
                Vector3 vector4 = vertices[num3];
                Vector3 vector5 = vertices[num4];
                Vector2 vector6 = uvs[index];
                Vector2 vector7 = uvs[num3];
                Vector2 vector8 = uvs[num4];
                float num5 = vector4.x - vector3.x;
                float num6 = vector5.x - vector3.x;
                float num7 = vector4.y - vector3.y;
                float num8 = vector5.y - vector3.y;
                float num9 = vector7.x - vector6.x;
                float num10 = vector8.x - vector6.x;
                float num11 = vector7.y - vector6.y;
                float num12 = vector8.y - vector6.y;
                float num13 = (num9 * num12) - (num10 * num11);
                float num14 = (num13 != 0f) ? (1f / num13) : 0f;
                vector.x = ((num12 * num5) - (num11 * num6)) * num14;
                vector.y = ((num12 * num7) - (num11 * num8)) * num14;
                tempTanBuffer[num4] = vector9 = vector;
                tempTanBuffer[index] = tempTanBuffer[num3] = vector9;
                vector2.x = ((num9 * num6) - (num10 * num5)) * num14;
                vector2.y = ((num9 * num8) - (num10 * num7)) * num14;
                tempTanBuffer[vertexCount + num4] = vector9 = vector2;
                tempTanBuffer[vertexCount + index] = tempTanBuffer[vertexCount + num3] = vector9;
            }
        }

        public void TrimExcess()
        {
            this.vertexBuffer.TrimExcess();
            this.uvBuffer.TrimExcess();
            this.colorBuffer.TrimExcess();
            if (this.uv2 != null)
            {
                this.uv2.TrimExcess();
            }
            if (this.uv3 != null)
            {
                this.uv3.TrimExcess();
            }
            int count = this.vertexBuffer.Count;
            if (this.normals != null)
            {
                Array.Resize<Vector3>(ref this.normals, count);
            }
            if (this.tangents != null)
            {
                Array.Resize<Vector4>(ref this.tangents, count);
            }
        }

        public static void TryReplaceMaterials(ExposedList<SubmeshInstruction> workingSubmeshInstructions, Dictionary<Material, Material> customMaterialOverride)
        {
            SubmeshInstruction[] items = workingSubmeshInstructions.Items;
            for (int i = 0; i < workingSubmeshInstructions.Count; i++)
            {
                Material key = items[i].material;
                if (customMaterialOverride.TryGetValue(key, out Material material2))
                {
                    items[i].material = material2;
                }
            }
        }

        public int VertexCount =>
            this.vertexBuffer.Count;

        public MeshGeneratorBuffers Buffers =>
            new MeshGeneratorBuffers { 
                vertexCount=this.VertexCount,
                vertexBuffer=this.vertexBuffer.Items,
                uvBuffer=this.uvBuffer.Items,
                colorBuffer=this.colorBuffer.Items,
                meshGenerator=this
            };

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Settings
        {
            public bool useClipping;
            [Space, Range(-0.1f, 0f)]
            public float zSpacing;
            [Space, Header("Vertex Data")]
            public bool pmaVertexColors;
            public bool tintBlack;
            public bool calculateTangents;
            public bool addNormals;
            public bool immutableTriangles;
            public static MeshGenerator.Settings Default =>
                new MeshGenerator.Settings { 
                    pmaVertexColors=true,
                    zSpacing=0f,
                    useClipping=true,
                    tintBlack=false,
                    calculateTangents=false,
                    addNormals=false,
                    immutableTriangles=false
                };
        }
    }
}

