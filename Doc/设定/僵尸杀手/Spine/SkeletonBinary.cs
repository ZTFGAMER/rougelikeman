namespace Spine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class SkeletonBinary
    {
        public const int BONE_ROTATE = 0;
        public const int BONE_TRANSLATE = 1;
        public const int BONE_SCALE = 2;
        public const int BONE_SHEAR = 3;
        public const int SLOT_ATTACHMENT = 0;
        public const int SLOT_COLOR = 1;
        public const int SLOT_TWO_COLOR = 2;
        public const int PATH_POSITION = 0;
        public const int PATH_SPACING = 1;
        public const int PATH_MIX = 2;
        public const int CURVE_LINEAR = 0;
        public const int CURVE_STEPPED = 1;
        public const int CURVE_BEZIER = 2;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Scale>k__BackingField;
        private AttachmentLoader attachmentLoader;
        private byte[] buffer;
        private List<SkeletonJson.LinkedMesh> linkedMeshes;
        public static readonly TransformMode[] TransformModeValues = new TransformMode[] { TransformMode.Normal };

        public SkeletonBinary(params Atlas[] atlasArray) : this(new AtlasAttachmentLoader(atlasArray))
        {
        }

        public SkeletonBinary(AttachmentLoader attachmentLoader)
        {
            this.buffer = new byte[0x20];
            this.linkedMeshes = new List<SkeletonJson.LinkedMesh>();
            if (attachmentLoader == null)
            {
                throw new ArgumentNullException("attachmentLoader");
            }
            this.attachmentLoader = attachmentLoader;
            this.Scale = 1f;
        }

        public static string GetVersionString(Stream input)
        {
            string str;
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            try
            {
                int length = ReadVarint(input, true);
                if (length > 1)
                {
                    input.Position += length - 1;
                }
                length = ReadVarint(input, true);
                if (length <= 1)
                {
                    throw new ArgumentException("Stream does not contain a valid binary Skeleton Data.", "input");
                }
                length--;
                byte[] buffer = new byte[length];
                ReadFully(input, buffer, 0, length);
                str = Encoding.UTF8.GetString(buffer, 0, length);
            }
            catch (Exception exception)
            {
                throw new ArgumentException("Stream does not contain a valid binary Skeleton Data.\n" + exception, "input");
            }
            return str;
        }

        private void ReadAnimation(string name, Stream input, SkeletonData skeletonData)
        {
            ExposedList<Timeline> timelines = new ExposedList<Timeline>();
            float scale = this.Scale;
            float num2 = 0f;
            int num3 = 0;
            int num4 = ReadVarint(input, true);
            while (num3 < num4)
            {
                int num5 = ReadVarint(input, true);
                int num6 = 0;
                int num7 = ReadVarint(input, true);
                while (num6 < num7)
                {
                    int num8 = input.ReadByte();
                    int num9 = ReadVarint(input, true);
                    switch (num8)
                    {
                        case 0:
                        {
                            AttachmentTimeline item = new AttachmentTimeline(num9) {
                                slotIndex = num5
                            };
                            for (int i = 0; i < num9; i++)
                            {
                                item.SetFrame(i, this.ReadFloat(input), this.ReadString(input));
                            }
                            timelines.Add(item);
                            num2 = Math.Max(num2, item.frames[num9 - 1]);
                            break;
                        }
                        case 1:
                        {
                            ColorTimeline timeline2 = new ColorTimeline(num9) {
                                slotIndex = num5
                            };
                            for (int i = 0; i < num9; i++)
                            {
                                float time = this.ReadFloat(input);
                                int num13 = ReadInt(input);
                                float r = ((float) ((num13 & 0xff000000L) >> 0x18)) / 255f;
                                float g = ((float) ((num13 & 0xff0000) >> 0x10)) / 255f;
                                float b = ((float) ((num13 & 0xff00) >> 8)) / 255f;
                                float a = ((float) (num13 & 0xff)) / 255f;
                                timeline2.SetFrame(i, time, r, g, b, a);
                                if (i < (num9 - 1))
                                {
                                    this.ReadCurve(input, i, timeline2);
                                }
                            }
                            timelines.Add(timeline2);
                            num2 = Math.Max(num2, timeline2.frames[(timeline2.FrameCount - 1) * 5]);
                            break;
                        }
                        case 2:
                        {
                            TwoColorTimeline timeline3 = new TwoColorTimeline(num9) {
                                slotIndex = num5
                            };
                            for (int i = 0; i < num9; i++)
                            {
                                float time = this.ReadFloat(input);
                                int num20 = ReadInt(input);
                                float r = ((float) ((num20 & 0xff000000L) >> 0x18)) / 255f;
                                float g = ((float) ((num20 & 0xff0000) >> 0x10)) / 255f;
                                float b = ((float) ((num20 & 0xff00) >> 8)) / 255f;
                                float a = ((float) (num20 & 0xff)) / 255f;
                                int num25 = ReadInt(input);
                                float num26 = ((float) ((num25 & 0xff0000) >> 0x10)) / 255f;
                                float num27 = ((float) ((num25 & 0xff00) >> 8)) / 255f;
                                float num28 = ((float) (num25 & 0xff)) / 255f;
                                timeline3.SetFrame(i, time, r, g, b, a, num26, num27, num28);
                                if (i < (num9 - 1))
                                {
                                    this.ReadCurve(input, i, timeline3);
                                }
                            }
                            timelines.Add(timeline3);
                            num2 = Math.Max(num2, timeline3.frames[(timeline3.FrameCount - 1) * 8]);
                            break;
                        }
                    }
                    num6++;
                }
                num3++;
            }
            int num29 = 0;
            int num30 = ReadVarint(input, true);
            while (num29 < num30)
            {
                int num31 = ReadVarint(input, true);
                int num32 = 0;
                int num33 = ReadVarint(input, true);
                while (num32 < num33)
                {
                    RotateTimeline timeline4;
                    int num36;
                    TranslateTimeline timeline5;
                    float num37;
                    int num34 = input.ReadByte();
                    int num35 = ReadVarint(input, true);
                    switch (num34)
                    {
                        case 0:
                            timeline4 = new RotateTimeline(num35) {
                                boneIndex = num31
                            };
                            num36 = 0;
                            goto Label_0380;

                        case 1:
                        case 2:
                        case 3:
                            num37 = 1f;
                            if (num34 != 2)
                            {
                                goto Label_03C8;
                            }
                            timeline5 = new ScaleTimeline(num35);
                            goto Label_03EA;

                        default:
                            goto Label_0466;
                    }
                Label_034D:
                    timeline4.SetFrame(num36, this.ReadFloat(input), this.ReadFloat(input));
                    if (num36 < (num35 - 1))
                    {
                        this.ReadCurve(input, num36, timeline4);
                    }
                    num36++;
                Label_0380:
                    if (num36 < num35)
                    {
                        goto Label_034D;
                    }
                    timelines.Add(timeline4);
                    num2 = Math.Max(num2, timeline4.frames[(num35 - 1) * 2]);
                    goto Label_0466;
                Label_03C8:
                    if (num34 == 3)
                    {
                        timeline5 = new ShearTimeline(num35);
                    }
                    else
                    {
                        timeline5 = new TranslateTimeline(num35);
                        num37 = scale;
                    }
                Label_03EA:
                    timeline5.boneIndex = num31;
                    for (int i = 0; i < num35; i++)
                    {
                        timeline5.SetFrame(i, this.ReadFloat(input), this.ReadFloat(input) * num37, this.ReadFloat(input) * num37);
                        if (i < (num35 - 1))
                        {
                            this.ReadCurve(input, i, timeline5);
                        }
                    }
                    timelines.Add(timeline5);
                    num2 = Math.Max(num2, timeline5.frames[(num35 - 1) * 3]);
                Label_0466:
                    num32++;
                }
                num29++;
            }
            int num39 = 0;
            int num40 = ReadVarint(input, true);
            while (num39 < num40)
            {
                int num41 = ReadVarint(input, true);
                int num42 = ReadVarint(input, true);
                IkConstraintTimeline timeline = new IkConstraintTimeline(num42) {
                    ikConstraintIndex = num41
                };
                for (int i = 0; i < num42; i++)
                {
                    timeline.SetFrame(i, this.ReadFloat(input), this.ReadFloat(input), (int) ReadSByte(input));
                    if (i < (num42 - 1))
                    {
                        this.ReadCurve(input, i, timeline);
                    }
                }
                timelines.Add(timeline);
                num2 = Math.Max(num2, timeline.frames[(num42 - 1) * 3]);
                num39++;
            }
            int num44 = 0;
            int num45 = ReadVarint(input, true);
            while (num44 < num45)
            {
                int num46 = ReadVarint(input, true);
                int num47 = ReadVarint(input, true);
                TransformConstraintTimeline timeline = new TransformConstraintTimeline(num47) {
                    transformConstraintIndex = num46
                };
                for (int i = 0; i < num47; i++)
                {
                    timeline.SetFrame(i, this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input));
                    if (i < (num47 - 1))
                    {
                        this.ReadCurve(input, i, timeline);
                    }
                }
                timelines.Add(timeline);
                num2 = Math.Max(num2, timeline.frames[(num47 - 1) * 5]);
                num44++;
            }
            int num49 = 0;
            int num50 = ReadVarint(input, true);
            while (num49 < num50)
            {
                int index = ReadVarint(input, true);
                PathConstraintData data = skeletonData.pathConstraints.Items[index];
                int num52 = 0;
                int num53 = ReadVarint(input, true);
                while (num52 < num53)
                {
                    int num54 = ReadSByte(input);
                    int num55 = ReadVarint(input, true);
                    switch (num54)
                    {
                        case 0:
                        case 1:
                        {
                            PathConstraintPositionTimeline timeline8;
                            float num56 = 1f;
                            if (num54 == 1)
                            {
                                timeline8 = new PathConstraintSpacingTimeline(num55);
                                if ((data.spacingMode == SpacingMode.Length) || (data.spacingMode == SpacingMode.Fixed))
                                {
                                    num56 = scale;
                                }
                            }
                            else
                            {
                                timeline8 = new PathConstraintPositionTimeline(num55);
                                if (data.positionMode == PositionMode.Fixed)
                                {
                                    num56 = scale;
                                }
                            }
                            timeline8.pathConstraintIndex = index;
                            for (int i = 0; i < num55; i++)
                            {
                                timeline8.SetFrame(i, this.ReadFloat(input), this.ReadFloat(input) * num56);
                                if (i < (num55 - 1))
                                {
                                    this.ReadCurve(input, i, timeline8);
                                }
                            }
                            timelines.Add(timeline8);
                            num2 = Math.Max(num2, timeline8.frames[(num55 - 1) * 2]);
                            break;
                        }
                        case 2:
                        {
                            PathConstraintMixTimeline timeline = new PathConstraintMixTimeline(num55) {
                                pathConstraintIndex = index
                            };
                            for (int i = 0; i < num55; i++)
                            {
                                timeline.SetFrame(i, this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input));
                                if (i < (num55 - 1))
                                {
                                    this.ReadCurve(input, i, timeline);
                                }
                            }
                            timelines.Add(timeline);
                            num2 = Math.Max(num2, timeline.frames[(num55 - 1) * 3]);
                            break;
                        }
                    }
                    num52++;
                }
                num49++;
            }
            int num59 = 0;
            int num60 = ReadVarint(input, true);
            while (num59 < num60)
            {
                Skin skin = skeletonData.skins.Items[ReadVarint(input, true)];
                int num61 = 0;
                int num62 = ReadVarint(input, true);
                while (num61 < num62)
                {
                    int slotIndex = ReadVarint(input, true);
                    int num64 = 0;
                    int num65 = ReadVarint(input, true);
                    while (num64 < num65)
                    {
                        VertexAttachment attachment = (VertexAttachment) skin.GetAttachment(slotIndex, this.ReadString(input));
                        bool flag = attachment.bones != null;
                        float[] vertices = attachment.vertices;
                        int num66 = !flag ? vertices.Length : ((vertices.Length / 3) * 2);
                        int num67 = ReadVarint(input, true);
                        DeformTimeline timeline = new DeformTimeline(num67) {
                            slotIndex = slotIndex,
                            attachment = attachment
                        };
                        for (int i = 0; i < num67; i++)
                        {
                            float[] numArray2;
                            float time = this.ReadFloat(input);
                            int num70 = ReadVarint(input, true);
                            if (num70 == 0)
                            {
                                numArray2 = !flag ? vertices : new float[num66];
                            }
                            else
                            {
                                numArray2 = new float[num66];
                                int num71 = ReadVarint(input, true);
                                num70 += num71;
                                if (scale == 1f)
                                {
                                    for (int j = num71; j < num70; j++)
                                    {
                                        numArray2[j] = this.ReadFloat(input);
                                    }
                                }
                                else
                                {
                                    for (int j = num71; j < num70; j++)
                                    {
                                        numArray2[j] = this.ReadFloat(input) * scale;
                                    }
                                }
                                if (!flag)
                                {
                                    int index = 0;
                                    int length = numArray2.Length;
                                    while (index < length)
                                    {
                                        numArray2[index] += vertices[index];
                                        index++;
                                    }
                                }
                            }
                            timeline.SetFrame(i, time, numArray2);
                            if (i < (num67 - 1))
                            {
                                this.ReadCurve(input, i, timeline);
                            }
                        }
                        timelines.Add(timeline);
                        num2 = Math.Max(num2, timeline.frames[num67 - 1]);
                        num64++;
                    }
                    num61++;
                }
                num59++;
            }
            int frameCount = ReadVarint(input, true);
            if (frameCount > 0)
            {
                DrawOrderTimeline item = new DrawOrderTimeline(frameCount);
                int count = skeletonData.slots.Count;
                for (int i = 0; i < frameCount; i++)
                {
                    float time = this.ReadFloat(input);
                    int num80 = ReadVarint(input, true);
                    int[] drawOrder = new int[count];
                    for (int j = count - 1; j >= 0; j--)
                    {
                        drawOrder[j] = -1;
                    }
                    int[] numArray4 = new int[count - num80];
                    int num82 = 0;
                    int num83 = 0;
                    for (int k = 0; k < num80; k++)
                    {
                        int num85 = ReadVarint(input, true);
                        while (num82 != num85)
                        {
                            numArray4[num83++] = num82++;
                        }
                        drawOrder[num82 + ReadVarint(input, true)] = num82++;
                    }
                    while (num82 < count)
                    {
                        numArray4[num83++] = num82++;
                    }
                    for (int m = count - 1; m >= 0; m--)
                    {
                        if (drawOrder[m] == -1)
                        {
                            drawOrder[m] = numArray4[--num83];
                        }
                    }
                    item.SetFrame(i, time, drawOrder);
                }
                timelines.Add(item);
                num2 = Math.Max(num2, item.frames[frameCount - 1]);
            }
            int num87 = ReadVarint(input, true);
            if (num87 > 0)
            {
                EventTimeline item = new EventTimeline(num87);
                for (int i = 0; i < num87; i++)
                {
                    float time = this.ReadFloat(input);
                    EventData data = skeletonData.events.Items[ReadVarint(input, true)];
                    Event e = new Event(time, data) {
                        Int = ReadVarint(input, false),
                        Float = this.ReadFloat(input),
                        String = !ReadBoolean(input) ? data.String : this.ReadString(input)
                    };
                    item.SetFrame(i, e);
                }
                timelines.Add(item);
                num2 = Math.Max(num2, item.frames[num87 - 1]);
            }
            timelines.TrimExcess();
            skeletonData.animations.Add(new Animation(name, timelines, num2));
        }

        private Attachment ReadAttachment(Stream input, SkeletonData skeletonData, Skin skin, int slotIndex, string attachmentName, bool nonessential)
        {
            float scale = this.Scale;
            string name = this.ReadString(input);
            if (name == null)
            {
                name = attachmentName;
            }
            switch (input.ReadByte())
            {
                case 0:
                {
                    string path = this.ReadString(input);
                    float num2 = this.ReadFloat(input);
                    float num3 = this.ReadFloat(input);
                    float num4 = this.ReadFloat(input);
                    float num5 = this.ReadFloat(input);
                    float num6 = this.ReadFloat(input);
                    float num7 = this.ReadFloat(input);
                    float num8 = this.ReadFloat(input);
                    int num9 = ReadInt(input);
                    if (path == null)
                    {
                        path = name;
                    }
                    RegionAttachment attachment = this.attachmentLoader.NewRegionAttachment(skin, name, path);
                    if (attachment == null)
                    {
                        return null;
                    }
                    attachment.Path = path;
                    attachment.x = num3 * scale;
                    attachment.y = num4 * scale;
                    attachment.scaleX = num5;
                    attachment.scaleY = num6;
                    attachment.rotation = num2;
                    attachment.width = num7 * scale;
                    attachment.height = num8 * scale;
                    attachment.r = ((float) ((num9 & 0xff000000L) >> 0x18)) / 255f;
                    attachment.g = ((float) ((num9 & 0xff0000) >> 0x10)) / 255f;
                    attachment.b = ((float) ((num9 & 0xff00) >> 8)) / 255f;
                    attachment.a = ((float) (num9 & 0xff)) / 255f;
                    attachment.UpdateOffset();
                    return attachment;
                }
                case 1:
                {
                    int vertexCount = ReadVarint(input, true);
                    Vertices vertices = this.ReadVertices(input, vertexCount);
                    if (nonessential)
                    {
                        ReadInt(input);
                    }
                    BoundingBoxAttachment attachment2 = this.attachmentLoader.NewBoundingBoxAttachment(skin, name);
                    if (attachment2 == null)
                    {
                        return null;
                    }
                    attachment2.worldVerticesLength = vertexCount << 1;
                    attachment2.vertices = vertices.vertices;
                    attachment2.bones = vertices.bones;
                    return attachment2;
                }
                case 2:
                {
                    string path = this.ReadString(input);
                    int num11 = ReadInt(input);
                    int vertexCount = ReadVarint(input, true);
                    float[] numArray = this.ReadFloatArray(input, vertexCount << 1, 1f);
                    int[] numArray2 = this.ReadShortArray(input);
                    Vertices vertices2 = this.ReadVertices(input, vertexCount);
                    int num13 = ReadVarint(input, true);
                    int[] numArray3 = null;
                    float num14 = 0f;
                    float num15 = 0f;
                    if (nonessential)
                    {
                        numArray3 = this.ReadShortArray(input);
                        num14 = this.ReadFloat(input);
                        num15 = this.ReadFloat(input);
                    }
                    if (path == null)
                    {
                        path = name;
                    }
                    MeshAttachment attachment3 = this.attachmentLoader.NewMeshAttachment(skin, name, path);
                    if (attachment3 == null)
                    {
                        return null;
                    }
                    attachment3.Path = path;
                    attachment3.r = ((float) ((num11 & 0xff000000L) >> 0x18)) / 255f;
                    attachment3.g = ((float) ((num11 & 0xff0000) >> 0x10)) / 255f;
                    attachment3.b = ((float) ((num11 & 0xff00) >> 8)) / 255f;
                    attachment3.a = ((float) (num11 & 0xff)) / 255f;
                    attachment3.bones = vertices2.bones;
                    attachment3.vertices = vertices2.vertices;
                    attachment3.WorldVerticesLength = vertexCount << 1;
                    attachment3.triangles = numArray2;
                    attachment3.regionUVs = numArray;
                    attachment3.UpdateUVs();
                    attachment3.HullLength = num13 << 1;
                    if (nonessential)
                    {
                        attachment3.Edges = numArray3;
                        attachment3.Width = num14 * scale;
                        attachment3.Height = num15 * scale;
                    }
                    return attachment3;
                }
                case 3:
                {
                    string path = this.ReadString(input);
                    int num16 = ReadInt(input);
                    string str5 = this.ReadString(input);
                    string parent = this.ReadString(input);
                    bool flag = ReadBoolean(input);
                    float num17 = 0f;
                    float num18 = 0f;
                    if (nonessential)
                    {
                        num17 = this.ReadFloat(input);
                        num18 = this.ReadFloat(input);
                    }
                    if (path == null)
                    {
                        path = name;
                    }
                    MeshAttachment mesh = this.attachmentLoader.NewMeshAttachment(skin, name, path);
                    if (mesh == null)
                    {
                        return null;
                    }
                    mesh.Path = path;
                    mesh.r = ((float) ((num16 & 0xff000000L) >> 0x18)) / 255f;
                    mesh.g = ((float) ((num16 & 0xff0000) >> 0x10)) / 255f;
                    mesh.b = ((float) ((num16 & 0xff00) >> 8)) / 255f;
                    mesh.a = ((float) (num16 & 0xff)) / 255f;
                    mesh.inheritDeform = flag;
                    if (nonessential)
                    {
                        mesh.Width = num17 * scale;
                        mesh.Height = num18 * scale;
                    }
                    this.linkedMeshes.Add(new SkeletonJson.LinkedMesh(mesh, str5, slotIndex, parent));
                    return mesh;
                }
                case 4:
                {
                    bool flag2 = ReadBoolean(input);
                    bool flag3 = ReadBoolean(input);
                    int vertexCount = ReadVarint(input, true);
                    Vertices vertices3 = this.ReadVertices(input, vertexCount);
                    float[] numArray4 = new float[vertexCount / 3];
                    int index = 0;
                    int length = numArray4.Length;
                    while (index < length)
                    {
                        numArray4[index] = this.ReadFloat(input) * scale;
                        index++;
                    }
                    if (nonessential)
                    {
                        ReadInt(input);
                    }
                    PathAttachment attachment5 = this.attachmentLoader.NewPathAttachment(skin, name);
                    if (attachment5 == null)
                    {
                        return null;
                    }
                    attachment5.closed = flag2;
                    attachment5.constantSpeed = flag3;
                    attachment5.worldVerticesLength = vertexCount << 1;
                    attachment5.vertices = vertices3.vertices;
                    attachment5.bones = vertices3.bones;
                    attachment5.lengths = numArray4;
                    return attachment5;
                }
                case 5:
                {
                    float num22 = this.ReadFloat(input);
                    float num23 = this.ReadFloat(input);
                    float num24 = this.ReadFloat(input);
                    if (nonessential)
                    {
                        ReadInt(input);
                    }
                    PointAttachment attachment6 = this.attachmentLoader.NewPointAttachment(skin, name);
                    if (attachment6 == null)
                    {
                        return null;
                    }
                    attachment6.x = num23 * scale;
                    attachment6.y = num24 * scale;
                    attachment6.rotation = num22;
                    return attachment6;
                }
                case 6:
                {
                    int index = ReadVarint(input, true);
                    int vertexCount = ReadVarint(input, true);
                    Vertices vertices4 = this.ReadVertices(input, vertexCount);
                    if (nonessential)
                    {
                        ReadInt(input);
                    }
                    ClippingAttachment attachment7 = this.attachmentLoader.NewClippingAttachment(skin, name);
                    if (attachment7 == null)
                    {
                        return null;
                    }
                    attachment7.EndSlot = skeletonData.slots.Items[index];
                    attachment7.worldVerticesLength = vertexCount << 1;
                    attachment7.vertices = vertices4.vertices;
                    attachment7.bones = vertices4.bones;
                    return attachment7;
                }
            }
            return null;
        }

        private static bool ReadBoolean(Stream input) => 
            (input.ReadByte() != 0);

        private void ReadCurve(Stream input, int frameIndex, CurveTimeline timeline)
        {
            switch (input.ReadByte())
            {
                case 1:
                    timeline.SetStepped(frameIndex);
                    break;

                case 2:
                    timeline.SetCurve(frameIndex, this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input));
                    break;
            }
        }

        private float ReadFloat(Stream input)
        {
            this.buffer[3] = (byte) input.ReadByte();
            this.buffer[2] = (byte) input.ReadByte();
            this.buffer[1] = (byte) input.ReadByte();
            this.buffer[0] = (byte) input.ReadByte();
            return BitConverter.ToSingle(this.buffer, 0);
        }

        private float[] ReadFloatArray(Stream input, int n, float scale)
        {
            float[] numArray = new float[n];
            if (scale == 1f)
            {
                for (int j = 0; j < n; j++)
                {
                    numArray[j] = this.ReadFloat(input);
                }
                return numArray;
            }
            for (int i = 0; i < n; i++)
            {
                numArray[i] = this.ReadFloat(input) * scale;
            }
            return numArray;
        }

        private static void ReadFully(Stream input, byte[] buffer, int offset, int length)
        {
            while (length > 0)
            {
                int num = input.Read(buffer, offset, length);
                if (num <= 0)
                {
                    throw new EndOfStreamException();
                }
                offset += num;
                length -= num;
            }
        }

        private static int ReadInt(Stream input) => 
            ((((input.ReadByte() << 0x18) + (input.ReadByte() << 0x10)) + (input.ReadByte() << 8)) + input.ReadByte());

        private static sbyte ReadSByte(Stream input)
        {
            int num = input.ReadByte();
            if (num == -1)
            {
                throw new EndOfStreamException();
            }
            return (sbyte) num;
        }

        private int[] ReadShortArray(Stream input)
        {
            int num = ReadVarint(input, true);
            int[] numArray = new int[num];
            for (int i = 0; i < num; i++)
            {
                numArray[i] = (input.ReadByte() << 8) | input.ReadByte();
            }
            return numArray;
        }

        public SkeletonData ReadSkeletonData(Stream input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            float scale = this.Scale;
            SkeletonData skeletonData = new SkeletonData {
                hash = this.ReadString(input)
            };
            if (skeletonData.hash.Length == 0)
            {
                skeletonData.hash = null;
            }
            skeletonData.version = this.ReadString(input);
            if (skeletonData.version.Length == 0)
            {
                skeletonData.version = null;
            }
            skeletonData.width = this.ReadFloat(input);
            skeletonData.height = this.ReadFloat(input);
            bool nonessential = ReadBoolean(input);
            if (nonessential)
            {
                skeletonData.fps = this.ReadFloat(input);
                skeletonData.imagesPath = this.ReadString(input);
                if (skeletonData.imagesPath.Length == 0)
                {
                    skeletonData.imagesPath = null;
                }
            }
            int index = 0;
            int num3 = ReadVarint(input, true);
            while (index < num3)
            {
                string name = this.ReadString(input);
                BoneData parent = (index != 0) ? skeletonData.bones.Items[ReadVarint(input, true)] : null;
                BoneData data3 = new BoneData(index, name, parent) {
                    rotation = this.ReadFloat(input),
                    x = this.ReadFloat(input) * scale,
                    y = this.ReadFloat(input) * scale,
                    scaleX = this.ReadFloat(input),
                    scaleY = this.ReadFloat(input),
                    shearX = this.ReadFloat(input),
                    shearY = this.ReadFloat(input),
                    length = this.ReadFloat(input) * scale,
                    transformMode = TransformModeValues[ReadVarint(input, true)]
                };
                if (nonessential)
                {
                    ReadInt(input);
                }
                skeletonData.bones.Add(data3);
                index++;
            }
            int num4 = 0;
            int num5 = ReadVarint(input, true);
            while (num4 < num5)
            {
                string name = this.ReadString(input);
                BoneData boneData = skeletonData.bones.Items[ReadVarint(input, true)];
                SlotData data5 = new SlotData(num4, name, boneData);
                int num6 = ReadInt(input);
                data5.r = ((float) ((num6 & 0xff000000L) >> 0x18)) / 255f;
                data5.g = ((float) ((num6 & 0xff0000) >> 0x10)) / 255f;
                data5.b = ((float) ((num6 & 0xff00) >> 8)) / 255f;
                data5.a = ((float) (num6 & 0xff)) / 255f;
                int num7 = ReadInt(input);
                if (num7 != -1)
                {
                    data5.hasSecondColor = true;
                    data5.r2 = ((float) ((num7 & 0xff0000) >> 0x10)) / 255f;
                    data5.g2 = ((float) ((num7 & 0xff00) >> 8)) / 255f;
                    data5.b2 = ((float) (num7 & 0xff)) / 255f;
                }
                data5.attachmentName = this.ReadString(input);
                data5.blendMode = (BlendMode) ReadVarint(input, true);
                skeletonData.slots.Add(data5);
                num4++;
            }
            int num8 = 0;
            int num9 = ReadVarint(input, true);
            while (num8 < num9)
            {
                IkConstraintData data6 = new IkConstraintData(this.ReadString(input)) {
                    order = ReadVarint(input, true)
                };
                int num10 = 0;
                int num11 = ReadVarint(input, true);
                while (num10 < num11)
                {
                    data6.bones.Add(skeletonData.bones.Items[ReadVarint(input, true)]);
                    num10++;
                }
                data6.target = skeletonData.bones.Items[ReadVarint(input, true)];
                data6.mix = this.ReadFloat(input);
                data6.bendDirection = ReadSByte(input);
                skeletonData.ikConstraints.Add(data6);
                num8++;
            }
            int num12 = 0;
            int num13 = ReadVarint(input, true);
            while (num12 < num13)
            {
                TransformConstraintData data7 = new TransformConstraintData(this.ReadString(input)) {
                    order = ReadVarint(input, true)
                };
                int num14 = 0;
                int num15 = ReadVarint(input, true);
                while (num14 < num15)
                {
                    data7.bones.Add(skeletonData.bones.Items[ReadVarint(input, true)]);
                    num14++;
                }
                data7.target = skeletonData.bones.Items[ReadVarint(input, true)];
                data7.local = ReadBoolean(input);
                data7.relative = ReadBoolean(input);
                data7.offsetRotation = this.ReadFloat(input);
                data7.offsetX = this.ReadFloat(input) * scale;
                data7.offsetY = this.ReadFloat(input) * scale;
                data7.offsetScaleX = this.ReadFloat(input);
                data7.offsetScaleY = this.ReadFloat(input);
                data7.offsetShearY = this.ReadFloat(input);
                data7.rotateMix = this.ReadFloat(input);
                data7.translateMix = this.ReadFloat(input);
                data7.scaleMix = this.ReadFloat(input);
                data7.shearMix = this.ReadFloat(input);
                skeletonData.transformConstraints.Add(data7);
                num12++;
            }
            int num16 = 0;
            int num17 = ReadVarint(input, true);
            while (num16 < num17)
            {
                PathConstraintData data8 = new PathConstraintData(this.ReadString(input)) {
                    order = ReadVarint(input, true)
                };
                int num18 = 0;
                int num19 = ReadVarint(input, true);
                while (num18 < num19)
                {
                    data8.bones.Add(skeletonData.bones.Items[ReadVarint(input, true)]);
                    num18++;
                }
                data8.target = skeletonData.slots.Items[ReadVarint(input, true)];
                data8.positionMode = (PositionMode) Enum.GetValues(typeof(PositionMode)).GetValue(ReadVarint(input, true));
                data8.spacingMode = (SpacingMode) Enum.GetValues(typeof(SpacingMode)).GetValue(ReadVarint(input, true));
                data8.rotateMode = (RotateMode) Enum.GetValues(typeof(RotateMode)).GetValue(ReadVarint(input, true));
                data8.offsetRotation = this.ReadFloat(input);
                data8.position = this.ReadFloat(input);
                if (data8.positionMode == PositionMode.Fixed)
                {
                    data8.position *= scale;
                }
                data8.spacing = this.ReadFloat(input);
                if ((data8.spacingMode == SpacingMode.Length) || (data8.spacingMode == SpacingMode.Fixed))
                {
                    data8.spacing *= scale;
                }
                data8.rotateMix = this.ReadFloat(input);
                data8.translateMix = this.ReadFloat(input);
                skeletonData.pathConstraints.Add(data8);
                num16++;
            }
            Skin item = this.ReadSkin(input, skeletonData, "default", nonessential);
            if (item != null)
            {
                skeletonData.defaultSkin = item;
                skeletonData.skins.Add(item);
            }
            int num20 = 0;
            int num21 = ReadVarint(input, true);
            while (num20 < num21)
            {
                skeletonData.skins.Add(this.ReadSkin(input, skeletonData, this.ReadString(input), nonessential));
                num20++;
            }
            int num22 = 0;
            int count = this.linkedMeshes.Count;
            while (num22 < count)
            {
                SkeletonJson.LinkedMesh mesh = this.linkedMeshes[num22];
                Skin skin2 = (mesh.skin != null) ? skeletonData.FindSkin(mesh.skin) : skeletonData.DefaultSkin;
                if (skin2 == null)
                {
                    throw new Exception("Skin not found: " + mesh.skin);
                }
                Attachment attachment = skin2.GetAttachment(mesh.slotIndex, mesh.parent);
                if (attachment == null)
                {
                    throw new Exception("Parent mesh not found: " + mesh.parent);
                }
                mesh.mesh.ParentMesh = (MeshAttachment) attachment;
                mesh.mesh.UpdateUVs();
                num22++;
            }
            this.linkedMeshes.Clear();
            int num24 = 0;
            int num25 = ReadVarint(input, true);
            while (num24 < num25)
            {
                EventData data9 = new EventData(this.ReadString(input)) {
                    Int = ReadVarint(input, false),
                    Float = this.ReadFloat(input),
                    String = this.ReadString(input)
                };
                skeletonData.events.Add(data9);
                num24++;
            }
            int num26 = 0;
            int num27 = ReadVarint(input, true);
            while (num26 < num27)
            {
                this.ReadAnimation(this.ReadString(input), input, skeletonData);
                num26++;
            }
            skeletonData.bones.TrimExcess();
            skeletonData.slots.TrimExcess();
            skeletonData.skins.TrimExcess();
            skeletonData.events.TrimExcess();
            skeletonData.animations.TrimExcess();
            skeletonData.ikConstraints.TrimExcess();
            skeletonData.pathConstraints.TrimExcess();
            return skeletonData;
        }

        public SkeletonData ReadSkeletonData(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                SkeletonData data = this.ReadSkeletonData(stream);
                data.name = Path.GetFileNameWithoutExtension(path);
                return data;
            }
        }

        private Skin ReadSkin(Stream input, SkeletonData skeletonData, string skinName, bool nonessential)
        {
            int num = ReadVarint(input, true);
            if (num == 0)
            {
                return null;
            }
            Skin skin = new Skin(skinName);
            for (int i = 0; i < num; i++)
            {
                int slotIndex = ReadVarint(input, true);
                int num4 = 0;
                int num5 = ReadVarint(input, true);
                while (num4 < num5)
                {
                    string attachmentName = this.ReadString(input);
                    Attachment attachment = this.ReadAttachment(input, skeletonData, skin, slotIndex, attachmentName, nonessential);
                    if (attachment != null)
                    {
                        skin.AddAttachment(slotIndex, attachmentName, attachment);
                    }
                    num4++;
                }
            }
            return skin;
        }

        private string ReadString(Stream input)
        {
            int length = ReadVarint(input, true);
            switch (length)
            {
                case 0:
                    return null;

                case 1:
                    return string.Empty;
            }
            length--;
            byte[] buffer = this.buffer;
            if (buffer.Length < length)
            {
                buffer = new byte[length];
            }
            ReadFully(input, buffer, 0, length);
            return Encoding.UTF8.GetString(buffer, 0, length);
        }

        private static int ReadVarint(Stream input, bool optimizePositive)
        {
            int num = input.ReadByte();
            int num2 = num & 0x7f;
            if ((num & 0x80) != 0)
            {
                num = input.ReadByte();
                num2 |= (num & 0x7f) << 7;
                if ((num & 0x80) != 0)
                {
                    num = input.ReadByte();
                    num2 |= (num & 0x7f) << 14;
                    if ((num & 0x80) != 0)
                    {
                        num = input.ReadByte();
                        num2 |= (num & 0x7f) << 0x15;
                        if ((num & 0x80) != 0)
                        {
                            num2 |= (input.ReadByte() & 0x7f) << 0x1c;
                        }
                    }
                }
            }
            return (!optimizePositive ? ((num2 >> 1) ^ -(num2 & 1)) : num2);
        }

        private Vertices ReadVertices(Stream input, int vertexCount)
        {
            float scale = this.Scale;
            int n = vertexCount << 1;
            Vertices vertices = new Vertices();
            if (!ReadBoolean(input))
            {
                vertices.vertices = this.ReadFloatArray(input, n, scale);
                return vertices;
            }
            ExposedList<float> list = new ExposedList<float>((n * 3) * 3);
            ExposedList<int> list2 = new ExposedList<int>(n * 3);
            for (int i = 0; i < vertexCount; i++)
            {
                int item = ReadVarint(input, true);
                list2.Add(item);
                for (int j = 0; j < item; j++)
                {
                    list2.Add(ReadVarint(input, true));
                    list.Add(this.ReadFloat(input) * scale);
                    list.Add(this.ReadFloat(input) * scale);
                    list.Add(this.ReadFloat(input));
                }
            }
            vertices.vertices = list.ToArray();
            vertices.bones = list2.ToArray();
            return vertices;
        }

        public float Scale { get; set; }

        internal class Vertices
        {
            public int[] bones;
            public float[] vertices;
        }
    }
}

