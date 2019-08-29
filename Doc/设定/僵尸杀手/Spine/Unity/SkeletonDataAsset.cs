namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class SkeletonDataAsset : ScriptableObject
    {
        public AtlasAsset[] atlasAssets = new AtlasAsset[0];
        public float scale = 0.01f;
        public TextAsset skeletonJSON;
        [SpineAnimation("", "", false, false)]
        public string[] fromAnimation = new string[0];
        [SpineAnimation("", "", false, false)]
        public string[] toAnimation = new string[0];
        public float[] duration = new float[0];
        public float defaultMix;
        public RuntimeAnimatorController controller;
        private SkeletonData skeletonData;
        private AnimationStateData stateData;

        public void Clear()
        {
            this.skeletonData = null;
            this.stateData = null;
        }

        public static SkeletonDataAsset CreateRuntimeInstance(TextAsset skeletonDataFile, AtlasAsset atlasAsset, bool initialize, float scale = 0.01f)
        {
            AtlasAsset[] atlasAssets = new AtlasAsset[] { atlasAsset };
            return CreateRuntimeInstance(skeletonDataFile, atlasAssets, initialize, scale);
        }

        public static SkeletonDataAsset CreateRuntimeInstance(TextAsset skeletonDataFile, AtlasAsset[] atlasAssets, bool initialize, float scale = 0.01f)
        {
            SkeletonDataAsset asset = ScriptableObject.CreateInstance<SkeletonDataAsset>();
            asset.Clear();
            asset.skeletonJSON = skeletonDataFile;
            asset.atlasAssets = atlasAssets;
            asset.scale = scale;
            if (initialize)
            {
                asset.GetSkeletonData(true);
            }
            return asset;
        }

        public void FillStateData()
        {
            if (this.stateData != null)
            {
                this.stateData.defaultMix = this.defaultMix;
                int index = 0;
                int length = this.fromAnimation.Length;
                while (index < length)
                {
                    if ((this.fromAnimation[index].Length != 0) && (this.toAnimation[index].Length != 0))
                    {
                        this.stateData.SetMix(this.fromAnimation[index], this.toAnimation[index], this.duration[index]);
                    }
                    index++;
                }
            }
        }

        public AnimationStateData GetAnimationStateData()
        {
            if (this.stateData == null)
            {
                this.GetSkeletonData(false);
            }
            return this.stateData;
        }

        internal Atlas[] GetAtlasArray()
        {
            List<Atlas> list = new List<Atlas>(this.atlasAssets.Length);
            for (int i = 0; i < this.atlasAssets.Length; i++)
            {
                AtlasAsset asset = this.atlasAssets[i];
                if (asset != null)
                {
                    Atlas item = asset.GetAtlas();
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
            }
            return list.ToArray();
        }

        public SkeletonData GetSkeletonData(bool quiet)
        {
            if (this.skeletonJSON == null)
            {
                if (!quiet)
                {
                    Debug.LogError("Skeleton JSON file not set for SkeletonData asset: " + base.name, this);
                }
                this.Clear();
                return null;
            }
            if (this.skeletonData == null)
            {
                SkeletonData data;
                Atlas[] atlasArray = this.GetAtlasArray();
                AttachmentLoader attachmentLoader = (atlasArray.Length != 0) ? ((AttachmentLoader) new AtlasAttachmentLoader(atlasArray)) : ((AttachmentLoader) new RegionlessAttachmentLoader());
                float scale = this.scale;
                bool flag = this.skeletonJSON.name.ToLower().Contains(".skel");
                try
                {
                    if (flag)
                    {
                        data = ReadSkeletonData(this.skeletonJSON.bytes, attachmentLoader, scale);
                    }
                    else
                    {
                        data = ReadSkeletonData(this.skeletonJSON.text, attachmentLoader, scale);
                    }
                }
                catch (Exception exception)
                {
                    if (!quiet)
                    {
                        Debug.LogError("Error reading skeleton JSON file for SkeletonData asset: " + base.name + "\n" + exception.Message + "\n" + exception.StackTrace, this);
                    }
                    return null;
                }
                this.InitializeWithData(data);
            }
            return this.skeletonData;
        }

        internal void InitializeWithData(SkeletonData sd)
        {
            this.skeletonData = sd;
            this.stateData = new AnimationStateData(this.skeletonData);
            this.FillStateData();
        }

        internal static SkeletonData ReadSkeletonData(byte[] bytes, AttachmentLoader attachmentLoader, float scale)
        {
            MemoryStream input = new MemoryStream(bytes);
            SkeletonBinary binary = new SkeletonBinary(attachmentLoader) {
                Scale = scale
            };
            return binary.ReadSkeletonData(input);
        }

        internal static SkeletonData ReadSkeletonData(string text, AttachmentLoader attachmentLoader, float scale)
        {
            StringReader reader = new StringReader(text);
            SkeletonJson json = new SkeletonJson(attachmentLoader) {
                Scale = scale
            };
            return json.ReadSkeletonData(reader);
        }

        private void Reset()
        {
            this.Clear();
        }

        public bool IsLoaded =>
            (this.skeletonData != null);
    }
}

