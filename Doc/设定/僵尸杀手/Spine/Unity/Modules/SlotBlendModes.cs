namespace Spine.Unity.Modules
{
    using Spine;
    using Spine.Unity;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class SlotBlendModes : MonoBehaviour
    {
        private static Dictionary<MaterialTexturePair, Material> materialTable;
        public Material multiplyMaterialSource;
        public Material screenMaterialSource;
        private Texture2D texture;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <Applied>k__BackingField;

        public void Apply()
        {
            this.GetTexture();
            if (this.texture != null)
            {
                SkeletonRenderer component = base.GetComponent<SkeletonRenderer>();
                if (component != null)
                {
                    Dictionary<Slot, Material> customSlotMaterials = component.CustomSlotMaterials;
                    foreach (Slot slot in component.Skeleton.Slots)
                    {
                        switch (slot.data.blendMode)
                        {
                            case BlendMode.Multiply:
                                if (this.multiplyMaterialSource != null)
                                {
                                    customSlotMaterials[slot] = GetMaterialFor(this.multiplyMaterialSource, this.texture);
                                }
                                break;

                            case BlendMode.Screen:
                                if (this.screenMaterialSource != null)
                                {
                                    customSlotMaterials[slot] = GetMaterialFor(this.screenMaterialSource, this.texture);
                                }
                                break;
                        }
                    }
                    this.Applied = true;
                    component.LateUpdate();
                }
            }
        }

        internal static Material GetMaterialFor(Material materialSource, Texture2D texture)
        {
            if ((materialSource == null) || (texture == null))
            {
                return null;
            }
            Dictionary<MaterialTexturePair, Material> materialTable = MaterialTable;
            MaterialTexturePair key = new MaterialTexturePair {
                material = materialSource,
                texture2D = texture
            };
            if (!materialTable.TryGetValue(key, out Material material))
            {
                material = new Material(materialSource) {
                    name = "(Clone)" + texture.name + "-" + materialSource.name,
                    mainTexture = texture
                };
                materialTable[key] = material;
            }
            return material;
        }

        public void GetTexture()
        {
            if (this.texture == null)
            {
                SkeletonRenderer component = base.GetComponent<SkeletonRenderer>();
                if (component != null)
                {
                    SkeletonDataAsset skeletonDataAsset = component.skeletonDataAsset;
                    if (skeletonDataAsset != null)
                    {
                        AtlasAsset asset2 = skeletonDataAsset.atlasAssets[0];
                        if (asset2 != null)
                        {
                            Material material = asset2.materials[0];
                            if (material != null)
                            {
                                this.texture = material.mainTexture as Texture2D;
                            }
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (this.Applied)
            {
                this.Remove();
            }
        }

        public void Remove()
        {
            this.GetTexture();
            if (this.texture != null)
            {
                SkeletonRenderer component = base.GetComponent<SkeletonRenderer>();
                if (component != null)
                {
                    Dictionary<Slot, Material> customSlotMaterials = component.CustomSlotMaterials;
                    foreach (Slot slot in component.Skeleton.Slots)
                    {
                        Material material = null;
                        switch (slot.data.blendMode)
                        {
                            case BlendMode.Multiply:
                                if (customSlotMaterials.TryGetValue(slot, out material) && object.ReferenceEquals(material, GetMaterialFor(this.multiplyMaterialSource, this.texture)))
                                {
                                    customSlotMaterials.Remove(slot);
                                }
                                break;

                            case BlendMode.Screen:
                                if (customSlotMaterials.TryGetValue(slot, out material) && object.ReferenceEquals(material, GetMaterialFor(this.screenMaterialSource, this.texture)))
                                {
                                    customSlotMaterials.Remove(slot);
                                }
                                break;
                        }
                    }
                    this.Applied = false;
                    if (component.valid)
                    {
                        component.LateUpdate();
                    }
                }
            }
        }

        private void Start()
        {
            if (!this.Applied)
            {
                this.Apply();
            }
        }

        internal static Dictionary<MaterialTexturePair, Material> MaterialTable
        {
            get
            {
                if (materialTable == null)
                {
                    materialTable = new Dictionary<MaterialTexturePair, Material>();
                }
                return materialTable;
            }
        }

        public bool Applied { get; private set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct MaterialTexturePair
        {
            public Texture2D texture2D;
            public Material material;
        }
    }
}

