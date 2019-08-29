namespace Spine.Unity.Modules
{
    using Spine;
    using Spine.Unity;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [ExecuteInEditMode]
    public class SkeletonRendererCustomMaterials : MonoBehaviour
    {
        public SkeletonRenderer skeletonRenderer;
        [SerializeField]
        protected List<SlotMaterialOverride> customSlotMaterials = new List<SlotMaterialOverride>();
        [SerializeField]
        protected List<AtlasMaterialOverride> customMaterialOverrides = new List<AtlasMaterialOverride>();

        private void OnDisable()
        {
            if (this.skeletonRenderer == null)
            {
                Debug.LogError("skeletonRenderer == null");
            }
            else
            {
                this.RemoveCustomMaterialOverrides();
                this.RemoveCustomSlotMaterials();
            }
        }

        private void OnEnable()
        {
            if (this.skeletonRenderer == null)
            {
                this.skeletonRenderer = base.GetComponent<SkeletonRenderer>();
            }
            if (this.skeletonRenderer == null)
            {
                Debug.LogError("skeletonRenderer == null");
            }
            else
            {
                this.skeletonRenderer.Initialize(false);
                this.SetCustomMaterialOverrides();
                this.SetCustomSlotMaterials();
            }
        }

        private void RemoveCustomMaterialOverrides()
        {
            if (this.skeletonRenderer == null)
            {
                Debug.LogError("skeletonRenderer == null");
            }
            else
            {
                for (int i = 0; i < this.customMaterialOverrides.Count; i++)
                {
                    AtlasMaterialOverride @override = this.customMaterialOverrides[i];
                    if (this.skeletonRenderer.CustomMaterialOverride.TryGetValue(@override.originalMaterial, out Material material) && (material == @override.replacementMaterial))
                    {
                        this.skeletonRenderer.CustomMaterialOverride.Remove(@override.originalMaterial);
                    }
                }
            }
        }

        private void RemoveCustomSlotMaterials()
        {
            if (this.skeletonRenderer == null)
            {
                Debug.LogError("skeletonRenderer == null");
            }
            else
            {
                for (int i = 0; i < this.customSlotMaterials.Count; i++)
                {
                    SlotMaterialOverride @override = this.customSlotMaterials[i];
                    if (!string.IsNullOrEmpty(@override.slotName))
                    {
                        Slot key = this.skeletonRenderer.skeleton.FindSlot(@override.slotName);
                        if (this.skeletonRenderer.CustomSlotMaterials.TryGetValue(key, out Material material) && (material == @override.material))
                        {
                            this.skeletonRenderer.CustomSlotMaterials.Remove(key);
                        }
                    }
                }
            }
        }

        private void SetCustomMaterialOverrides()
        {
            if (this.skeletonRenderer == null)
            {
                Debug.LogError("skeletonRenderer == null");
            }
            else
            {
                for (int i = 0; i < this.customMaterialOverrides.Count; i++)
                {
                    AtlasMaterialOverride @override = this.customMaterialOverrides[i];
                    if (!@override.overrideDisabled)
                    {
                        this.skeletonRenderer.CustomMaterialOverride[@override.originalMaterial] = @override.replacementMaterial;
                    }
                }
            }
        }

        private void SetCustomSlotMaterials()
        {
            if (this.skeletonRenderer == null)
            {
                Debug.LogError("skeletonRenderer == null");
            }
            else
            {
                for (int i = 0; i < this.customSlotMaterials.Count; i++)
                {
                    SlotMaterialOverride @override = this.customSlotMaterials[i];
                    if (!@override.overrideDisabled && !string.IsNullOrEmpty(@override.slotName))
                    {
                        Slot slot = this.skeletonRenderer.skeleton.FindSlot(@override.slotName);
                        this.skeletonRenderer.CustomSlotMaterials[slot] = @override.material;
                    }
                }
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct AtlasMaterialOverride : IEquatable<SkeletonRendererCustomMaterials.AtlasMaterialOverride>
        {
            public bool overrideDisabled;
            public Material originalMaterial;
            public Material replacementMaterial;
            public bool Equals(SkeletonRendererCustomMaterials.AtlasMaterialOverride other) => 
                (((this.overrideDisabled == other.overrideDisabled) && (this.originalMaterial == other.originalMaterial)) && (this.replacementMaterial == other.replacementMaterial));
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct SlotMaterialOverride : IEquatable<SkeletonRendererCustomMaterials.SlotMaterialOverride>
        {
            public bool overrideDisabled;
            [SpineSlot("", "", false, true, false)]
            public string slotName;
            public Material material;
            public bool Equals(SkeletonRendererCustomMaterials.SlotMaterialOverride other) => 
                (((this.overrideDisabled == other.overrideDisabled) && (this.slotName == other.slotName)) && (this.material == other.material));
        }
    }
}

