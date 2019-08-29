namespace Spine.Unity.Modules
{
    using Spine;
    using Spine.Unity;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Rendering;

    [ExecuteInEditMode, HelpURL("https://github.com/pharan/spine-unity-docs/blob/master/SkeletonRenderSeparator.md")]
    public class SkeletonRenderSeparator : MonoBehaviour
    {
        public const int DefaultSortingOrderIncrement = 5;
        [SerializeField]
        protected Spine.Unity.SkeletonRenderer skeletonRenderer;
        private MeshRenderer mainMeshRenderer;
        public bool copyPropertyBlock = true;
        [Tooltip("Copies MeshRenderer flags into each parts renderer")]
        public bool copyMeshRendererFlags = true;
        public List<SkeletonPartsRenderer> partsRenderers = new List<SkeletonPartsRenderer>();
        private MaterialPropertyBlock copiedBlock;

        public void AddPartsRenderer(int sortingOrderIncrement = 5)
        {
            int sortingLayerID = 0;
            int num2 = 0;
            if (this.partsRenderers.Count > 0)
            {
                SkeletonPartsRenderer renderer = this.partsRenderers[this.partsRenderers.Count - 1];
                MeshRenderer renderer2 = renderer.MeshRenderer;
                sortingLayerID = renderer2.sortingLayerID;
                num2 = renderer2.sortingOrder + sortingOrderIncrement;
            }
            SkeletonPartsRenderer item = SkeletonPartsRenderer.NewPartsRendererGameObject(this.skeletonRenderer.transform, this.partsRenderers.Count.ToString());
            this.partsRenderers.Add(item);
            MeshRenderer meshRenderer = item.MeshRenderer;
            meshRenderer.sortingLayerID = sortingLayerID;
            meshRenderer.sortingOrder = num2;
        }

        public static SkeletonRenderSeparator AddToSkeletonRenderer(Spine.Unity.SkeletonRenderer skeletonRenderer, int sortingLayerID = 0, int extraPartsRenderers = 0, int sortingOrderIncrement = 5, int baseSortingOrder = 0, bool addMinimumPartsRenderers = true)
        {
            if (skeletonRenderer == null)
            {
                Debug.Log("Tried to add SkeletonRenderSeparator to a null SkeletonRenderer reference.");
                return null;
            }
            SkeletonRenderSeparator separator = skeletonRenderer.gameObject.AddComponent<SkeletonRenderSeparator>();
            separator.skeletonRenderer = skeletonRenderer;
            skeletonRenderer.Initialize(false);
            int num = extraPartsRenderers;
            if (addMinimumPartsRenderers)
            {
                num = (extraPartsRenderers + skeletonRenderer.separatorSlots.Count) + 1;
            }
            Transform parent = skeletonRenderer.transform;
            List<SkeletonPartsRenderer> partsRenderers = separator.partsRenderers;
            for (int i = 0; i < num; i++)
            {
                SkeletonPartsRenderer item = SkeletonPartsRenderer.NewPartsRendererGameObject(parent, i.ToString());
                MeshRenderer meshRenderer = item.MeshRenderer;
                meshRenderer.sortingLayerID = sortingLayerID;
                meshRenderer.sortingOrder = baseSortingOrder + (i * sortingOrderIncrement);
                partsRenderers.Add(item);
            }
            return separator;
        }

        private void HandleRender(SkeletonRendererInstruction instruction)
        {
            int count = this.partsRenderers.Count;
            if (count > 0)
            {
                if (this.copyPropertyBlock)
                {
                    this.mainMeshRenderer.GetPropertyBlock(this.copiedBlock);
                }
                MeshGenerator.Settings settings = new MeshGenerator.Settings {
                    addNormals = this.skeletonRenderer.addNormals,
                    calculateTangents = this.skeletonRenderer.calculateTangents,
                    immutableTriangles = false,
                    pmaVertexColors = this.skeletonRenderer.pmaVertexColors,
                    tintBlack = this.skeletonRenderer.tintBlack,
                    useClipping = true,
                    zSpacing = this.skeletonRenderer.zSpacing
                };
                ExposedList<SubmeshInstruction> submeshInstructions = instruction.submeshInstructions;
                SubmeshInstruction[] items = submeshInstructions.Items;
                int num2 = submeshInstructions.Count - 1;
                int num3 = 0;
                SkeletonPartsRenderer renderer = this.partsRenderers[num3];
                int index = 0;
                int startSubmesh = 0;
                while (index <= num2)
                {
                    if (items[index].forceSeparate || (index == num2))
                    {
                        renderer.MeshGenerator.settings = settings;
                        if (this.copyPropertyBlock)
                        {
                            renderer.SetPropertyBlock(this.copiedBlock);
                        }
                        renderer.RenderParts(instruction.submeshInstructions, startSubmesh, index + 1);
                        startSubmesh = index + 1;
                        num3++;
                        if (num3 >= count)
                        {
                            break;
                        }
                        renderer = this.partsRenderers[num3];
                    }
                    index++;
                }
                while (num3 < count)
                {
                    this.partsRenderers[num3].ClearMesh();
                    num3++;
                }
            }
        }

        private void OnDisable()
        {
            if (this.skeletonRenderer != null)
            {
                this.skeletonRenderer.GenerateMeshOverride -= new Spine.Unity.SkeletonRenderer.InstructionDelegate(this.HandleRender);
                foreach (SkeletonPartsRenderer renderer in this.partsRenderers)
                {
                    renderer.ClearMesh();
                }
            }
        }

        private void OnEnable()
        {
            if (this.skeletonRenderer != null)
            {
                if (this.copiedBlock == null)
                {
                    this.copiedBlock = new MaterialPropertyBlock();
                }
                this.mainMeshRenderer = this.skeletonRenderer.GetComponent<MeshRenderer>();
                this.skeletonRenderer.GenerateMeshOverride -= new Spine.Unity.SkeletonRenderer.InstructionDelegate(this.HandleRender);
                this.skeletonRenderer.GenerateMeshOverride += new Spine.Unity.SkeletonRenderer.InstructionDelegate(this.HandleRender);
                if (this.copyMeshRendererFlags)
                {
                    LightProbeUsage lightProbeUsage = this.mainMeshRenderer.lightProbeUsage;
                    bool receiveShadows = this.mainMeshRenderer.receiveShadows;
                    ReflectionProbeUsage reflectionProbeUsage = this.mainMeshRenderer.reflectionProbeUsage;
                    ShadowCastingMode shadowCastingMode = this.mainMeshRenderer.shadowCastingMode;
                    MotionVectorGenerationMode motionVectorGenerationMode = this.mainMeshRenderer.motionVectorGenerationMode;
                    Transform probeAnchor = this.mainMeshRenderer.probeAnchor;
                    for (int i = 0; i < this.partsRenderers.Count; i++)
                    {
                        SkeletonPartsRenderer renderer = this.partsRenderers[i];
                        if (renderer != null)
                        {
                            MeshRenderer meshRenderer = renderer.MeshRenderer;
                            meshRenderer.lightProbeUsage = lightProbeUsage;
                            meshRenderer.receiveShadows = receiveShadows;
                            meshRenderer.reflectionProbeUsage = reflectionProbeUsage;
                            meshRenderer.shadowCastingMode = shadowCastingMode;
                            meshRenderer.motionVectorGenerationMode = motionVectorGenerationMode;
                            meshRenderer.probeAnchor = probeAnchor;
                        }
                    }
                }
            }
        }

        public Spine.Unity.SkeletonRenderer SkeletonRenderer
        {
            get => 
                this.skeletonRenderer;
            set
            {
                if (this.skeletonRenderer != null)
                {
                    this.skeletonRenderer.GenerateMeshOverride -= new Spine.Unity.SkeletonRenderer.InstructionDelegate(this.HandleRender);
                }
                this.skeletonRenderer = value;
                base.enabled = false;
            }
        }
    }
}

