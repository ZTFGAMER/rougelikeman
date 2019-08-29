namespace Spine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class SkeletonJson
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Scale>k__BackingField;
        private AttachmentLoader attachmentLoader;
        private List<LinkedMesh> linkedMeshes;

        public SkeletonJson(params Atlas[] atlasArray) : this(new AtlasAttachmentLoader(atlasArray))
        {
        }

        public SkeletonJson(AttachmentLoader attachmentLoader)
        {
            this.linkedMeshes = new List<LinkedMesh>();
            if (attachmentLoader == null)
            {
                throw new ArgumentNullException("attachmentLoader", "attachmentLoader cannot be null.");
            }
            this.attachmentLoader = attachmentLoader;
            this.Scale = 1f;
        }

        private static bool GetBoolean(Dictionary<string, object> map, string name, bool defaultValue)
        {
            if (!map.ContainsKey(name))
            {
                return defaultValue;
            }
            return (bool) map[name];
        }

        private static float GetFloat(Dictionary<string, object> map, string name, float defaultValue)
        {
            if (!map.ContainsKey(name))
            {
                return defaultValue;
            }
            return (float) map[name];
        }

        private static float[] GetFloatArray(Dictionary<string, object> map, string name, float scale)
        {
            List<object> list = (List<object>) map[name];
            float[] numArray = new float[list.Count];
            if (scale == 1f)
            {
                int num = 0;
                int num2 = list.Count;
                while (num < num2)
                {
                    numArray[num] = (float) list[num];
                    num++;
                }
                return numArray;
            }
            int index = 0;
            int count = list.Count;
            while (index < count)
            {
                numArray[index] = ((float) list[index]) * scale;
                index++;
            }
            return numArray;
        }

        private static int GetInt(Dictionary<string, object> map, string name, int defaultValue)
        {
            if (!map.ContainsKey(name))
            {
                return defaultValue;
            }
            return (int) ((float) map[name]);
        }

        private static int[] GetIntArray(Dictionary<string, object> map, string name)
        {
            List<object> list = (List<object>) map[name];
            int[] numArray = new int[list.Count];
            int index = 0;
            int count = list.Count;
            while (index < count)
            {
                numArray[index] = (int) ((float) list[index]);
                index++;
            }
            return numArray;
        }

        private static string GetString(Dictionary<string, object> map, string name, string defaultValue)
        {
            if (!map.ContainsKey(name))
            {
                return defaultValue;
            }
            return (string) map[name];
        }

        private void ReadAnimation(Dictionary<string, object> map, string name, SkeletonData skeletonData)
        {
            float scale = this.Scale;
            ExposedList<Timeline> timelines = new ExposedList<Timeline>();
            float num2 = 0f;
            if (map.ContainsKey("slots"))
            {
                foreach (KeyValuePair<string, object> pair in (Dictionary<string, object>) map["slots"])
                {
                    string key = pair.Key;
                    int num3 = skeletonData.FindSlotIndex(key);
                    Dictionary<string, object> dictionary = (Dictionary<string, object>) pair.Value;
                    foreach (KeyValuePair<string, object> pair2 in dictionary)
                    {
                        List<object> list2 = (List<object>) pair2.Value;
                        string str2 = pair2.Key;
                        switch (str2)
                        {
                            case "attachment":
                            {
                                AttachmentTimeline item = new AttachmentTimeline(list2.Count) {
                                    slotIndex = num3
                                };
                                int num4 = 0;
                                foreach (Dictionary<string, object> dictionary2 in list2)
                                {
                                    float time = (float) dictionary2["time"];
                                    item.SetFrame(num4++, time, (string) dictionary2["name"]);
                                }
                                timelines.Add(item);
                                num2 = Math.Max(num2, item.frames[item.FrameCount - 1]);
                                break;
                            }
                            case "color":
                            {
                                ColorTimeline timeline2 = new ColorTimeline(list2.Count) {
                                    slotIndex = num3
                                };
                                int frameIndex = 0;
                                foreach (Dictionary<string, object> dictionary3 in list2)
                                {
                                    float time = (float) dictionary3["time"];
                                    string hexString = (string) dictionary3["color"];
                                    timeline2.SetFrame(frameIndex, time, ToColor(hexString, 0, 8), ToColor(hexString, 1, 8), ToColor(hexString, 2, 8), ToColor(hexString, 3, 8));
                                    ReadCurve(dictionary3, timeline2, frameIndex);
                                    frameIndex++;
                                }
                                timelines.Add(timeline2);
                                num2 = Math.Max(num2, timeline2.frames[(timeline2.FrameCount - 1) * 5]);
                                break;
                            }
                            case "twoColor":
                            {
                                TwoColorTimeline timeline3 = new TwoColorTimeline(list2.Count) {
                                    slotIndex = num3
                                };
                                int frameIndex = 0;
                                foreach (Dictionary<string, object> dictionary4 in list2)
                                {
                                    float time = (float) dictionary4["time"];
                                    string hexString = (string) dictionary4["light"];
                                    string str5 = (string) dictionary4["dark"];
                                    timeline3.SetFrame(frameIndex, time, ToColor(hexString, 0, 8), ToColor(hexString, 1, 8), ToColor(hexString, 2, 8), ToColor(hexString, 3, 8), ToColor(str5, 0, 6), ToColor(str5, 1, 6), ToColor(str5, 2, 6));
                                    ReadCurve(dictionary4, timeline3, frameIndex);
                                    frameIndex++;
                                }
                                timelines.Add(timeline3);
                                num2 = Math.Max(num2, timeline3.frames[(timeline3.FrameCount - 1) * 8]);
                                break;
                            }
                            default:
                            {
                                string[] textArray1 = new string[] { "Invalid timeline type for a slot: ", str2, " (", key, ")" };
                                throw new Exception(string.Concat(textArray1));
                            }
                        }
                    }
                }
            }
            if (map.ContainsKey("bones"))
            {
                foreach (KeyValuePair<string, object> pair3 in (Dictionary<string, object>) map["bones"])
                {
                    string key = pair3.Key;
                    int num10 = skeletonData.FindBoneIndex(key);
                    if (num10 == -1)
                    {
                        throw new Exception("Bone not found: " + key);
                    }
                    Dictionary<string, object> dictionary5 = (Dictionary<string, object>) pair3.Value;
                    foreach (KeyValuePair<string, object> pair4 in dictionary5)
                    {
                        List<object> list3 = (List<object>) pair4.Value;
                        string str7 = pair4.Key;
                        switch (str7)
                        {
                            case "rotate":
                            {
                                RotateTimeline timeline = new RotateTimeline(list3.Count) {
                                    boneIndex = num10
                                };
                                int frameIndex = 0;
                                foreach (Dictionary<string, object> dictionary6 in list3)
                                {
                                    timeline.SetFrame(frameIndex, (float) dictionary6["time"], (float) dictionary6["angle"]);
                                    ReadCurve(dictionary6, timeline, frameIndex);
                                    frameIndex++;
                                }
                                timelines.Add(timeline);
                                num2 = Math.Max(num2, timeline.frames[(timeline.FrameCount - 1) * 2]);
                                break;
                            }
                            case "translate":
                            case "scale":
                            case "shear":
                            {
                                TranslateTimeline timeline5;
                                float num12 = 1f;
                                if (str7 == "scale")
                                {
                                    timeline5 = new ScaleTimeline(list3.Count);
                                }
                                else if (str7 == "shear")
                                {
                                    timeline5 = new ShearTimeline(list3.Count);
                                }
                                else
                                {
                                    timeline5 = new TranslateTimeline(list3.Count);
                                    num12 = scale;
                                }
                                timeline5.boneIndex = num10;
                                int frameIndex = 0;
                                foreach (Dictionary<string, object> dictionary7 in list3)
                                {
                                    float time = (float) dictionary7["time"];
                                    float num15 = GetFloat(dictionary7, "x", 0f);
                                    float num16 = GetFloat(dictionary7, "y", 0f);
                                    timeline5.SetFrame(frameIndex, time, num15 * num12, num16 * num12);
                                    ReadCurve(dictionary7, timeline5, frameIndex);
                                    frameIndex++;
                                }
                                timelines.Add(timeline5);
                                num2 = Math.Max(num2, timeline5.frames[(timeline5.FrameCount - 1) * 3]);
                                break;
                            }
                            default:
                            {
                                string[] textArray2 = new string[] { "Invalid timeline type for a bone: ", str7, " (", key, ")" };
                                throw new Exception(string.Concat(textArray2));
                            }
                        }
                    }
                }
            }
            if (map.ContainsKey("ik"))
            {
                foreach (KeyValuePair<string, object> pair5 in (Dictionary<string, object>) map["ik"])
                {
                    IkConstraintData item = skeletonData.FindIkConstraint(pair5.Key);
                    List<object> list4 = (List<object>) pair5.Value;
                    IkConstraintTimeline timeline = new IkConstraintTimeline(list4.Count) {
                        ikConstraintIndex = skeletonData.ikConstraints.IndexOf(item)
                    };
                    int frameIndex = 0;
                    foreach (Dictionary<string, object> dictionary8 in list4)
                    {
                        float time = (float) dictionary8["time"];
                        float mix = GetFloat(dictionary8, "mix", 1f);
                        bool flag = GetBoolean(dictionary8, "bendPositive", true);
                        timeline.SetFrame(frameIndex, time, mix, !flag ? -1 : 1);
                        ReadCurve(dictionary8, timeline, frameIndex);
                        frameIndex++;
                    }
                    timelines.Add(timeline);
                    num2 = Math.Max(num2, timeline.frames[(timeline.FrameCount - 1) * 3]);
                }
            }
            if (map.ContainsKey("transform"))
            {
                foreach (KeyValuePair<string, object> pair6 in (Dictionary<string, object>) map["transform"])
                {
                    TransformConstraintData item = skeletonData.FindTransformConstraint(pair6.Key);
                    List<object> list5 = (List<object>) pair6.Value;
                    TransformConstraintTimeline timeline = new TransformConstraintTimeline(list5.Count) {
                        transformConstraintIndex = skeletonData.transformConstraints.IndexOf(item)
                    };
                    int frameIndex = 0;
                    foreach (Dictionary<string, object> dictionary9 in list5)
                    {
                        float time = (float) dictionary9["time"];
                        float rotateMix = GetFloat(dictionary9, "rotateMix", 1f);
                        float translateMix = GetFloat(dictionary9, "translateMix", 1f);
                        float scaleMix = GetFloat(dictionary9, "scaleMix", 1f);
                        float shearMix = GetFloat(dictionary9, "shearMix", 1f);
                        timeline.SetFrame(frameIndex, time, rotateMix, translateMix, scaleMix, shearMix);
                        ReadCurve(dictionary9, timeline, frameIndex);
                        frameIndex++;
                    }
                    timelines.Add(timeline);
                    num2 = Math.Max(num2, timeline.frames[(timeline.FrameCount - 1) * 5]);
                }
            }
            if (map.ContainsKey("paths"))
            {
                foreach (KeyValuePair<string, object> pair7 in (Dictionary<string, object>) map["paths"])
                {
                    int index = skeletonData.FindPathConstraintIndex(pair7.Key);
                    if (index == -1)
                    {
                        throw new Exception("Path constraint not found: " + pair7.Key);
                    }
                    PathConstraintData data3 = skeletonData.pathConstraints.Items[index];
                    Dictionary<string, object> dictionary10 = (Dictionary<string, object>) pair7.Value;
                    foreach (KeyValuePair<string, object> pair8 in dictionary10)
                    {
                        List<object> list6 = (List<object>) pair8.Value;
                        string key = pair8.Key;
                        switch (key)
                        {
                            case "position":
                            case "spacing":
                            {
                                PathConstraintPositionTimeline timeline8;
                                float num27 = 1f;
                                if (key == "spacing")
                                {
                                    timeline8 = new PathConstraintSpacingTimeline(list6.Count);
                                    if ((data3.spacingMode == SpacingMode.Length) || (data3.spacingMode == SpacingMode.Fixed))
                                    {
                                        num27 = scale;
                                    }
                                }
                                else
                                {
                                    timeline8 = new PathConstraintPositionTimeline(list6.Count);
                                    if (data3.positionMode == PositionMode.Fixed)
                                    {
                                        num27 = scale;
                                    }
                                }
                                timeline8.pathConstraintIndex = index;
                                int frameIndex = 0;
                                foreach (Dictionary<string, object> dictionary11 in list6)
                                {
                                    timeline8.SetFrame(frameIndex, (float) dictionary11["time"], GetFloat(dictionary11, key, 0f) * num27);
                                    ReadCurve(dictionary11, timeline8, frameIndex);
                                    frameIndex++;
                                }
                                timelines.Add(timeline8);
                                num2 = Math.Max(num2, timeline8.frames[(timeline8.FrameCount - 1) * 2]);
                                break;
                            }
                            default:
                                if (key == "mix")
                                {
                                    PathConstraintMixTimeline timeline = new PathConstraintMixTimeline(list6.Count) {
                                        pathConstraintIndex = index
                                    };
                                    int frameIndex = 0;
                                    foreach (Dictionary<string, object> dictionary12 in list6)
                                    {
                                        timeline.SetFrame(frameIndex, (float) dictionary12["time"], GetFloat(dictionary12, "rotateMix", 1f), GetFloat(dictionary12, "translateMix", 1f));
                                        ReadCurve(dictionary12, timeline, frameIndex);
                                        frameIndex++;
                                    }
                                    timelines.Add(timeline);
                                    num2 = Math.Max(num2, timeline.frames[(timeline.FrameCount - 1) * 3]);
                                }
                                break;
                        }
                    }
                }
            }
            if (map.ContainsKey("deform"))
            {
                foreach (KeyValuePair<string, object> pair9 in (Dictionary<string, object>) map["deform"])
                {
                    Skin skin = skeletonData.FindSkin(pair9.Key);
                    foreach (KeyValuePair<string, object> pair10 in (Dictionary<string, object>) pair9.Value)
                    {
                        int slotIndex = skeletonData.FindSlotIndex(pair10.Key);
                        if (slotIndex == -1)
                        {
                            throw new Exception("Slot not found: " + pair10.Key);
                        }
                        foreach (KeyValuePair<string, object> pair11 in (Dictionary<string, object>) pair10.Value)
                        {
                            List<object> list7 = (List<object>) pair11.Value;
                            VertexAttachment attachment = (VertexAttachment) skin.GetAttachment(slotIndex, pair11.Key);
                            if (attachment == null)
                            {
                                throw new Exception("Deform attachment not found: " + pair11.Key);
                            }
                            bool flag2 = attachment.bones != null;
                            float[] vertices = attachment.vertices;
                            int num31 = !flag2 ? vertices.Length : ((vertices.Length / 3) * 2);
                            DeformTimeline timeline = new DeformTimeline(list7.Count) {
                                slotIndex = slotIndex,
                                attachment = attachment
                            };
                            int frameIndex = 0;
                            foreach (Dictionary<string, object> dictionary13 in list7)
                            {
                                float[] numArray2;
                                if (!dictionary13.ContainsKey("vertices"))
                                {
                                    numArray2 = !flag2 ? vertices : new float[num31];
                                }
                                else
                                {
                                    numArray2 = new float[num31];
                                    int destinationIndex = GetInt(dictionary13, "offset", 0);
                                    float[] sourceArray = GetFloatArray(dictionary13, "vertices", 1f);
                                    Array.Copy(sourceArray, 0, numArray2, destinationIndex, sourceArray.Length);
                                    if (scale != 1f)
                                    {
                                        int index = destinationIndex;
                                        int num35 = index + sourceArray.Length;
                                        while (index < num35)
                                        {
                                            numArray2[index] *= scale;
                                            index++;
                                        }
                                    }
                                    if (!flag2)
                                    {
                                        for (int i = 0; i < num31; i++)
                                        {
                                            numArray2[i] += vertices[i];
                                        }
                                    }
                                }
                                timeline.SetFrame(frameIndex, (float) dictionary13["time"], numArray2);
                                ReadCurve(dictionary13, timeline, frameIndex);
                                frameIndex++;
                            }
                            timelines.Add(timeline);
                            num2 = Math.Max(num2, timeline.frames[timeline.FrameCount - 1]);
                        }
                    }
                }
            }
            if (map.ContainsKey("drawOrder") || map.ContainsKey("draworder"))
            {
                List<object> list8 = (List<object>) map[!map.ContainsKey("drawOrder") ? "draworder" : "drawOrder"];
                DrawOrderTimeline item = new DrawOrderTimeline(list8.Count);
                int count = skeletonData.slots.Count;
                int num38 = 0;
                foreach (Dictionary<string, object> dictionary14 in list8)
                {
                    int[] drawOrder = null;
                    if (dictionary14.ContainsKey("offsets"))
                    {
                        drawOrder = new int[count];
                        for (int i = count - 1; i >= 0; i--)
                        {
                            drawOrder[i] = -1;
                        }
                        List<object> list9 = (List<object>) dictionary14["offsets"];
                        int[] numArray5 = new int[count - list9.Count];
                        int num40 = 0;
                        int num41 = 0;
                        foreach (Dictionary<string, object> dictionary15 in list9)
                        {
                            int num42 = skeletonData.FindSlotIndex((string) dictionary15["slot"]);
                            if (num42 == -1)
                            {
                                throw new Exception("Slot not found: " + dictionary15["slot"]);
                            }
                            while (num40 != num42)
                            {
                                numArray5[num41++] = num40++;
                            }
                            int index = num40 + ((int) ((float) dictionary15["offset"]));
                            drawOrder[index] = num40++;
                        }
                        while (num40 < count)
                        {
                            numArray5[num41++] = num40++;
                        }
                        for (int j = count - 1; j >= 0; j--)
                        {
                            if (drawOrder[j] == -1)
                            {
                                drawOrder[j] = numArray5[--num41];
                            }
                        }
                    }
                    item.SetFrame(num38++, (float) dictionary14["time"], drawOrder);
                }
                timelines.Add(item);
                num2 = Math.Max(num2, item.frames[item.FrameCount - 1]);
            }
            if (map.ContainsKey("events"))
            {
                List<object> list10 = (List<object>) map["events"];
                EventTimeline item = new EventTimeline(list10.Count);
                int num45 = 0;
                foreach (Dictionary<string, object> dictionary16 in list10)
                {
                    EventData data = skeletonData.FindEvent((string) dictionary16["name"]);
                    if (data == null)
                    {
                        throw new Exception("Event not found: " + dictionary16["name"]);
                    }
                    Event e = new Event((float) dictionary16["time"], data) {
                        Int = GetInt(dictionary16, "int", data.Int),
                        Float = GetFloat(dictionary16, "float", data.Float),
                        String = GetString(dictionary16, "string", data.String)
                    };
                    item.SetFrame(num45++, e);
                }
                timelines.Add(item);
                num2 = Math.Max(num2, item.frames[item.FrameCount - 1]);
            }
            timelines.TrimExcess();
            skeletonData.animations.Add(new Animation(name, timelines, num2));
        }

        private Attachment ReadAttachment(Dictionary<string, object> map, Skin skin, int slotIndex, string name, SkeletonData skeletonData)
        {
            float scale = this.Scale;
            name = GetString(map, "name", name);
            string str = GetString(map, "type", "region");
            switch (str)
            {
                case "skinnedmesh":
                    str = "weightedmesh";
                    break;

                case "weightedmesh":
                    str = "mesh";
                    break;

                case "weightedlinkedmesh":
                    str = "linkedmesh";
                    break;
            }
            AttachmentType type = (AttachmentType) Enum.Parse(typeof(AttachmentType), str, true);
            string path = GetString(map, "path", name);
            switch (type)
            {
                case AttachmentType.Region:
                {
                    RegionAttachment attachment = this.attachmentLoader.NewRegionAttachment(skin, name, path);
                    if (attachment != null)
                    {
                        attachment.Path = path;
                        attachment.x = GetFloat(map, "x", 0f) * scale;
                        attachment.y = GetFloat(map, "y", 0f) * scale;
                        attachment.scaleX = GetFloat(map, "scaleX", 1f);
                        attachment.scaleY = GetFloat(map, "scaleY", 1f);
                        attachment.rotation = GetFloat(map, "rotation", 0f);
                        attachment.width = GetFloat(map, "width", 32f) * scale;
                        attachment.height = GetFloat(map, "height", 32f) * scale;
                        if (map.ContainsKey("color"))
                        {
                            string hexString = (string) map["color"];
                            attachment.r = ToColor(hexString, 0, 8);
                            attachment.g = ToColor(hexString, 1, 8);
                            attachment.b = ToColor(hexString, 2, 8);
                            attachment.a = ToColor(hexString, 3, 8);
                        }
                        attachment.UpdateOffset();
                        return attachment;
                    }
                    return null;
                }
                case AttachmentType.Boundingbox:
                {
                    BoundingBoxAttachment attachment2 = this.attachmentLoader.NewBoundingBoxAttachment(skin, name);
                    if (attachment2 != null)
                    {
                        this.ReadVertices(map, attachment2, GetInt(map, "vertexCount", 0) << 1);
                        return attachment2;
                    }
                    return null;
                }
                case AttachmentType.Mesh:
                case AttachmentType.Linkedmesh:
                {
                    MeshAttachment mesh = this.attachmentLoader.NewMeshAttachment(skin, name, path);
                    if (mesh != null)
                    {
                        mesh.Path = path;
                        if (map.ContainsKey("color"))
                        {
                            string hexString = (string) map["color"];
                            mesh.r = ToColor(hexString, 0, 8);
                            mesh.g = ToColor(hexString, 1, 8);
                            mesh.b = ToColor(hexString, 2, 8);
                            mesh.a = ToColor(hexString, 3, 8);
                        }
                        mesh.Width = GetFloat(map, "width", 0f) * scale;
                        mesh.Height = GetFloat(map, "height", 0f) * scale;
                        string parent = GetString(map, "parent", null);
                        if (parent != null)
                        {
                            mesh.InheritDeform = GetBoolean(map, "deform", true);
                            this.linkedMeshes.Add(new LinkedMesh(mesh, GetString(map, "skin", null), slotIndex, parent));
                            return mesh;
                        }
                        float[] numArray = GetFloatArray(map, "uvs", 1f);
                        this.ReadVertices(map, mesh, numArray.Length);
                        mesh.triangles = GetIntArray(map, "triangles");
                        mesh.regionUVs = numArray;
                        mesh.UpdateUVs();
                        if (map.ContainsKey("hull"))
                        {
                            mesh.HullLength = GetInt(map, "hull", 0) * 2;
                        }
                        if (map.ContainsKey("edges"))
                        {
                            mesh.Edges = GetIntArray(map, "edges");
                        }
                        return mesh;
                    }
                    return null;
                }
                case AttachmentType.Path:
                {
                    PathAttachment attachment4 = this.attachmentLoader.NewPathAttachment(skin, name);
                    if (attachment4 != null)
                    {
                        attachment4.closed = GetBoolean(map, "closed", false);
                        attachment4.constantSpeed = GetBoolean(map, "constantSpeed", true);
                        int num2 = GetInt(map, "vertexCount", 0);
                        this.ReadVertices(map, attachment4, num2 << 1);
                        attachment4.lengths = GetFloatArray(map, "lengths", scale);
                        return attachment4;
                    }
                    return null;
                }
                case AttachmentType.Point:
                {
                    PointAttachment attachment5 = this.attachmentLoader.NewPointAttachment(skin, name);
                    if (attachment5 != null)
                    {
                        attachment5.x = GetFloat(map, "x", 0f) * scale;
                        attachment5.y = GetFloat(map, "y", 0f) * scale;
                        attachment5.rotation = GetFloat(map, "rotation", 0f);
                        return attachment5;
                    }
                    return null;
                }
                case AttachmentType.Clipping:
                {
                    ClippingAttachment attachment6 = this.attachmentLoader.NewClippingAttachment(skin, name);
                    if (attachment6 != null)
                    {
                        string slotName = GetString(map, "end", null);
                        if (slotName != null)
                        {
                            SlotData data = skeletonData.FindSlot(slotName);
                            if (data == null)
                            {
                                throw new Exception("Clipping end slot not found: " + slotName);
                            }
                            attachment6.EndSlot = data;
                        }
                        this.ReadVertices(map, attachment6, GetInt(map, "vertexCount", 0) << 1);
                        return attachment6;
                    }
                    return null;
                }
            }
            return null;
        }

        private static void ReadCurve(Dictionary<string, object> valueMap, CurveTimeline timeline, int frameIndex)
        {
            if (valueMap.ContainsKey("curve"))
            {
                object obj2 = valueMap["curve"];
                if (obj2.Equals("stepped"))
                {
                    timeline.SetStepped(frameIndex);
                }
                else
                {
                    List<object> list = obj2 as List<object>;
                    if (list != null)
                    {
                        timeline.SetCurve(frameIndex, (float) list[0], (float) list[1], (float) list[2], (float) list[3]);
                    }
                }
            }
        }

        public SkeletonData ReadSkeletonData(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader", "reader cannot be null.");
            }
            float scale = this.Scale;
            SkeletonData skeletonData = new SkeletonData();
            Dictionary<string, object> dictionary = Json.Deserialize(reader) as Dictionary<string, object>;
            if (dictionary == null)
            {
                throw new Exception("Invalid JSON.");
            }
            if (dictionary.ContainsKey("skeleton"))
            {
                Dictionary<string, object> map = (Dictionary<string, object>) dictionary["skeleton"];
                skeletonData.hash = (string) map["hash"];
                skeletonData.version = (string) map["spine"];
                skeletonData.width = GetFloat(map, "width", 0f);
                skeletonData.height = GetFloat(map, "height", 0f);
                skeletonData.fps = GetFloat(map, "fps", 0f);
                skeletonData.imagesPath = GetString(map, "images", null);
            }
            foreach (Dictionary<string, object> dictionary3 in (List<object>) dictionary["bones"])
            {
                BoneData parent = null;
                if (dictionary3.ContainsKey("parent"))
                {
                    parent = skeletonData.FindBone((string) dictionary3["parent"]);
                    if (parent == null)
                    {
                        throw new Exception("Parent bone not found: " + dictionary3["parent"]);
                    }
                }
                BoneData item = new BoneData(skeletonData.Bones.Count, (string) dictionary3["name"], parent) {
                    length = GetFloat(dictionary3, "length", 0f) * scale,
                    x = GetFloat(dictionary3, "x", 0f) * scale,
                    y = GetFloat(dictionary3, "y", 0f) * scale,
                    rotation = GetFloat(dictionary3, "rotation", 0f),
                    scaleX = GetFloat(dictionary3, "scaleX", 1f),
                    scaleY = GetFloat(dictionary3, "scaleY", 1f),
                    shearX = GetFloat(dictionary3, "shearX", 0f),
                    shearY = GetFloat(dictionary3, "shearY", 0f)
                };
                string str = GetString(dictionary3, "transform", TransformMode.Normal.ToString());
                item.transformMode = (TransformMode) Enum.Parse(typeof(TransformMode), str, true);
                skeletonData.bones.Add(item);
            }
            if (dictionary.ContainsKey("slots"))
            {
                foreach (Dictionary<string, object> dictionary4 in (List<object>) dictionary["slots"])
                {
                    string name = (string) dictionary4["name"];
                    string boneName = (string) dictionary4["bone"];
                    BoneData boneData = skeletonData.FindBone(boneName);
                    if (boneData == null)
                    {
                        throw new Exception("Slot bone not found: " + boneName);
                    }
                    SlotData item = new SlotData(skeletonData.Slots.Count, name, boneData);
                    if (dictionary4.ContainsKey("color"))
                    {
                        string hexString = (string) dictionary4["color"];
                        item.r = ToColor(hexString, 0, 8);
                        item.g = ToColor(hexString, 1, 8);
                        item.b = ToColor(hexString, 2, 8);
                        item.a = ToColor(hexString, 3, 8);
                    }
                    if (dictionary4.ContainsKey("dark"))
                    {
                        string hexString = (string) dictionary4["dark"];
                        item.r2 = ToColor(hexString, 0, 6);
                        item.g2 = ToColor(hexString, 1, 6);
                        item.b2 = ToColor(hexString, 2, 6);
                        item.hasSecondColor = true;
                    }
                    item.attachmentName = GetString(dictionary4, "attachment", null);
                    if (dictionary4.ContainsKey("blend"))
                    {
                        item.blendMode = (BlendMode) Enum.Parse(typeof(BlendMode), (string) dictionary4["blend"], true);
                    }
                    else
                    {
                        item.blendMode = BlendMode.Normal;
                    }
                    skeletonData.slots.Add(item);
                }
            }
            if (dictionary.ContainsKey("ik"))
            {
                foreach (Dictionary<string, object> dictionary5 in (List<object>) dictionary["ik"])
                {
                    IkConstraintData item = new IkConstraintData((string) dictionary5["name"]) {
                        order = GetInt(dictionary5, "order", 0)
                    };
                    foreach (string str6 in (List<object>) dictionary5["bones"])
                    {
                        BoneData data7 = skeletonData.FindBone(str6);
                        if (data7 == null)
                        {
                            throw new Exception("IK constraint bone not found: " + str6);
                        }
                        item.bones.Add(data7);
                    }
                    string boneName = (string) dictionary5["target"];
                    item.target = skeletonData.FindBone(boneName);
                    if (item.target == null)
                    {
                        throw new Exception("Target bone not found: " + boneName);
                    }
                    item.bendDirection = !GetBoolean(dictionary5, "bendPositive", true) ? -1 : 1;
                    item.mix = GetFloat(dictionary5, "mix", 1f);
                    skeletonData.ikConstraints.Add(item);
                }
            }
            if (dictionary.ContainsKey("transform"))
            {
                foreach (Dictionary<string, object> dictionary6 in (List<object>) dictionary["transform"])
                {
                    TransformConstraintData item = new TransformConstraintData((string) dictionary6["name"]) {
                        order = GetInt(dictionary6, "order", 0)
                    };
                    foreach (string str8 in (List<object>) dictionary6["bones"])
                    {
                        BoneData data9 = skeletonData.FindBone(str8);
                        if (data9 == null)
                        {
                            throw new Exception("Transform constraint bone not found: " + str8);
                        }
                        item.bones.Add(data9);
                    }
                    string boneName = (string) dictionary6["target"];
                    item.target = skeletonData.FindBone(boneName);
                    if (item.target == null)
                    {
                        throw new Exception("Target bone not found: " + boneName);
                    }
                    item.local = GetBoolean(dictionary6, "local", false);
                    item.relative = GetBoolean(dictionary6, "relative", false);
                    item.offsetRotation = GetFloat(dictionary6, "rotation", 0f);
                    item.offsetX = GetFloat(dictionary6, "x", 0f) * scale;
                    item.offsetY = GetFloat(dictionary6, "y", 0f) * scale;
                    item.offsetScaleX = GetFloat(dictionary6, "scaleX", 0f);
                    item.offsetScaleY = GetFloat(dictionary6, "scaleY", 0f);
                    item.offsetShearY = GetFloat(dictionary6, "shearY", 0f);
                    item.rotateMix = GetFloat(dictionary6, "rotateMix", 1f);
                    item.translateMix = GetFloat(dictionary6, "translateMix", 1f);
                    item.scaleMix = GetFloat(dictionary6, "scaleMix", 1f);
                    item.shearMix = GetFloat(dictionary6, "shearMix", 1f);
                    skeletonData.transformConstraints.Add(item);
                }
            }
            if (dictionary.ContainsKey("path"))
            {
                foreach (Dictionary<string, object> dictionary7 in (List<object>) dictionary["path"])
                {
                    PathConstraintData item = new PathConstraintData((string) dictionary7["name"]) {
                        order = GetInt(dictionary7, "order", 0)
                    };
                    foreach (string str10 in (List<object>) dictionary7["bones"])
                    {
                        BoneData data11 = skeletonData.FindBone(str10);
                        if (data11 == null)
                        {
                            throw new Exception("Path bone not found: " + str10);
                        }
                        item.bones.Add(data11);
                    }
                    string slotName = (string) dictionary7["target"];
                    item.target = skeletonData.FindSlot(slotName);
                    if (item.target == null)
                    {
                        throw new Exception("Target slot not found: " + slotName);
                    }
                    item.positionMode = (PositionMode) Enum.Parse(typeof(PositionMode), GetString(dictionary7, "positionMode", "percent"), true);
                    item.spacingMode = (SpacingMode) Enum.Parse(typeof(SpacingMode), GetString(dictionary7, "spacingMode", "length"), true);
                    item.rotateMode = (RotateMode) Enum.Parse(typeof(RotateMode), GetString(dictionary7, "rotateMode", "tangent"), true);
                    item.offsetRotation = GetFloat(dictionary7, "rotation", 0f);
                    item.position = GetFloat(dictionary7, "position", 0f);
                    if (item.positionMode == PositionMode.Fixed)
                    {
                        item.position *= scale;
                    }
                    item.spacing = GetFloat(dictionary7, "spacing", 0f);
                    if ((item.spacingMode == SpacingMode.Length) || (item.spacingMode == SpacingMode.Fixed))
                    {
                        item.spacing *= scale;
                    }
                    item.rotateMix = GetFloat(dictionary7, "rotateMix", 1f);
                    item.translateMix = GetFloat(dictionary7, "translateMix", 1f);
                    skeletonData.pathConstraints.Add(item);
                }
            }
            if (dictionary.ContainsKey("skins"))
            {
                foreach (KeyValuePair<string, object> pair in (Dictionary<string, object>) dictionary["skins"])
                {
                    Skin skin = new Skin(pair.Key);
                    foreach (KeyValuePair<string, object> pair2 in (Dictionary<string, object>) pair.Value)
                    {
                        int slotIndex = skeletonData.FindSlotIndex(pair2.Key);
                        foreach (KeyValuePair<string, object> pair3 in (Dictionary<string, object>) pair2.Value)
                        {
                            try
                            {
                                Attachment attachment = this.ReadAttachment((Dictionary<string, object>) pair3.Value, skin, slotIndex, pair3.Key, skeletonData);
                                if (attachment != null)
                                {
                                    skin.AddAttachment(slotIndex, pair3.Key, attachment);
                                }
                            }
                            catch (Exception exception)
                            {
                                object[] objArray1 = new object[] { "Error reading attachment: ", pair3.Key, ", skin: ", skin };
                                throw new Exception(string.Concat(objArray1), exception);
                            }
                        }
                    }
                    skeletonData.skins.Add(skin);
                    if (skin.name == "default")
                    {
                        skeletonData.defaultSkin = skin;
                    }
                }
            }
            int num3 = 0;
            int count = this.linkedMeshes.Count;
            while (num3 < count)
            {
                LinkedMesh mesh = this.linkedMeshes[num3];
                Skin skin2 = (mesh.skin != null) ? skeletonData.FindSkin(mesh.skin) : skeletonData.defaultSkin;
                if (skin2 == null)
                {
                    throw new Exception("Slot not found: " + mesh.skin);
                }
                Attachment attachment = skin2.GetAttachment(mesh.slotIndex, mesh.parent);
                if (attachment == null)
                {
                    throw new Exception("Parent mesh not found: " + mesh.parent);
                }
                mesh.mesh.ParentMesh = (MeshAttachment) attachment;
                mesh.mesh.UpdateUVs();
                num3++;
            }
            this.linkedMeshes.Clear();
            if (dictionary.ContainsKey("events"))
            {
                foreach (KeyValuePair<string, object> pair4 in (Dictionary<string, object>) dictionary["events"])
                {
                    Dictionary<string, object> map = (Dictionary<string, object>) pair4.Value;
                    EventData item = new EventData(pair4.Key) {
                        Int = GetInt(map, "int", 0),
                        Float = GetFloat(map, "float", 0f),
                        String = GetString(map, "string", string.Empty)
                    };
                    skeletonData.events.Add(item);
                }
            }
            if (dictionary.ContainsKey("animations"))
            {
                foreach (KeyValuePair<string, object> pair5 in (Dictionary<string, object>) dictionary["animations"])
                {
                    try
                    {
                        this.ReadAnimation((Dictionary<string, object>) pair5.Value, pair5.Key, skeletonData);
                    }
                    catch (Exception exception2)
                    {
                        throw new Exception("Error reading animation: " + pair5.Key, exception2);
                    }
                }
            }
            skeletonData.bones.TrimExcess();
            skeletonData.slots.TrimExcess();
            skeletonData.skins.TrimExcess();
            skeletonData.events.TrimExcess();
            skeletonData.animations.TrimExcess();
            skeletonData.ikConstraints.TrimExcess();
            return skeletonData;
        }

        public SkeletonData ReadSkeletonData(string path)
        {
            using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                SkeletonData data = this.ReadSkeletonData(reader);
                data.name = Path.GetFileNameWithoutExtension(path);
                return data;
            }
        }

        private void ReadVertices(Dictionary<string, object> map, VertexAttachment attachment, int verticesLength)
        {
            attachment.WorldVerticesLength = verticesLength;
            float[] numArray = GetFloatArray(map, "vertices", 1f);
            float scale = this.Scale;
            if (verticesLength == numArray.Length)
            {
                if (scale != 1f)
                {
                    for (int i = 0; i < numArray.Length; i++)
                    {
                        numArray[i] *= scale;
                    }
                }
                attachment.vertices = numArray;
            }
            else
            {
                ExposedList<float> list = new ExposedList<float>((verticesLength * 3) * 3);
                ExposedList<int> list2 = new ExposedList<int>(verticesLength * 3);
                int index = 0;
                int length = numArray.Length;
                while (index < length)
                {
                    int item = (int) numArray[index++];
                    list2.Add(item);
                    int num6 = index + (item * 4);
                    while (index < num6)
                    {
                        list2.Add((int) numArray[index]);
                        list.Add(numArray[index + 1] * this.Scale);
                        list.Add(numArray[index + 2] * this.Scale);
                        list.Add(numArray[index + 3]);
                        index += 4;
                    }
                }
                attachment.bones = list2.ToArray();
                attachment.vertices = list.ToArray();
            }
        }

        private static float ToColor(string hexString, int colorIndex, int expectedLength = 8)
        {
            if (hexString.Length != expectedLength)
            {
                object[] objArray1 = new object[] { "Color hexidecimal length must be ", expectedLength, ", recieved: ", hexString };
                throw new ArgumentException(string.Concat(objArray1), "hexString");
            }
            return (((float) Convert.ToInt32(hexString.Substring(colorIndex * 2, 2), 0x10)) / 255f);
        }

        public float Scale { get; set; }

        internal class LinkedMesh
        {
            internal string parent;
            internal string skin;
            internal int slotIndex;
            internal MeshAttachment mesh;

            public LinkedMesh(MeshAttachment mesh, string skin, int slotIndex, string parent)
            {
                this.mesh = mesh;
                this.skin = skin;
                this.slotIndex = slotIndex;
                this.parent = parent;
            }
        }
    }
}

