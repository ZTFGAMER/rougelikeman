namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.Serialization;

    [ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent, HelpURL("http://esotericsoftware.com/spine-unity-documentation#Rendering")]
    public class SkeletonRenderer : MonoBehaviour, ISkeletonComponent, IHasSkeletonDataAsset
    {
        public Spine.Unity.SkeletonDataAsset skeletonDataAsset;
        public string initialSkinName;
        public bool initialFlipX;
        public bool initialFlipY;
        [FormerlySerializedAs("submeshSeparators"), SpineSlot("", "", false, true, false)]
        public string[] separatorSlotNames = new string[0];
        [NonSerialized]
        public readonly List<Slot> separatorSlots = new List<Slot>();
        [Range(-0.1f, 0f)]
        public float zSpacing;
        public bool useClipping = true;
        public bool immutableTriangles;
        public bool pmaVertexColors = true;
        public bool clearStateOnDisable;
        public bool tintBlack;
        public bool singleSubmesh;
        [FormerlySerializedAs("calculateNormals")]
        public bool addNormals;
        public bool calculateTangents;
        public bool logErrors;
        public bool disableRenderingOnOverride = true;
        [NonSerialized]
        private readonly Dictionary<Material, Material> customMaterialOverride = new Dictionary<Material, Material>();
        [NonSerialized]
        private readonly Dictionary<Slot, Material> customSlotMaterials = new Dictionary<Slot, Material>();
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        [NonSerialized]
        public bool valid;
        [NonSerialized]
        public Spine.Skeleton skeleton;
        [NonSerialized]
        private readonly SkeletonRendererInstruction currentInstructions = new SkeletonRendererInstruction();
        private readonly MeshGenerator meshGenerator = new MeshGenerator();
        [NonSerialized]
        private readonly MeshRendererBuffers rendererBuffers = new MeshRendererBuffers();

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        private event InstructionDelegate generateMeshOverride;

        public event InstructionDelegate GenerateMeshOverride
        {
            add
            {
                this.generateMeshOverride += value;
                if (this.disableRenderingOnOverride && (this.generateMeshOverride != null))
                {
                    this.Initialize(false);
                    this.meshRenderer.enabled = false;
                }
            }
            remove
            {
                this.generateMeshOverride -= value;
                if (this.disableRenderingOnOverride && (this.generateMeshOverride == null))
                {
                    this.Initialize(false);
                    this.meshRenderer.enabled = true;
                }
            }
        }

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event MeshGeneratorDelegate OnPostProcessVertices;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event SkeletonRendererDelegate OnRebuild;

        public static T AddSpineComponent<T>(GameObject gameObject, Spine.Unity.SkeletonDataAsset skeletonDataAsset) where T: SkeletonRenderer
        {
            T local = gameObject.AddComponent<T>();
            if (skeletonDataAsset != null)
            {
                local.skeletonDataAsset = skeletonDataAsset;
                local.Initialize(false);
            }
            return local;
        }

        public virtual void Awake()
        {
            this.Initialize(false);
        }

        public virtual void ClearState()
        {
            this.meshFilter.sharedMesh = null;
            this.currentInstructions.Clear();
            if (this.skeleton != null)
            {
                this.skeleton.SetToSetupPose();
            }
        }

        public virtual void Initialize(bool overwrite)
        {
            if (!this.valid || overwrite)
            {
                if (this.meshFilter != null)
                {
                    this.meshFilter.sharedMesh = null;
                }
                this.meshRenderer = base.GetComponent<MeshRenderer>();
                if (this.meshRenderer != null)
                {
                    this.meshRenderer.sharedMaterial = null;
                }
                this.currentInstructions.Clear();
                this.rendererBuffers.Clear();
                this.meshGenerator.Begin();
                this.skeleton = null;
                this.valid = false;
                if (this.skeletonDataAsset == null)
                {
                    if (this.logErrors)
                    {
                        UnityEngine.Debug.LogError("Missing SkeletonData asset.", this);
                    }
                }
                else
                {
                    SkeletonData skeletonData = this.skeletonDataAsset.GetSkeletonData(false);
                    if (skeletonData != null)
                    {
                        this.valid = true;
                        this.meshFilter = base.GetComponent<MeshFilter>();
                        this.meshRenderer = base.GetComponent<MeshRenderer>();
                        this.rendererBuffers.Initialize();
                        Spine.Skeleton skeleton = new Spine.Skeleton(skeletonData) {
                            flipX = this.initialFlipX,
                            flipY = this.initialFlipY
                        };
                        this.skeleton = skeleton;
                        if (!string.IsNullOrEmpty(this.initialSkinName) && !string.Equals(this.initialSkinName, "default", StringComparison.Ordinal))
                        {
                            this.skeleton.SetSkin(this.initialSkinName);
                        }
                        this.separatorSlots.Clear();
                        for (int i = 0; i < this.separatorSlotNames.Length; i++)
                        {
                            this.separatorSlots.Add(this.skeleton.FindSlot(this.separatorSlotNames[i]));
                        }
                        this.LateUpdate();
                        if (this.OnRebuild != null)
                        {
                            this.OnRebuild(this);
                        }
                    }
                }
            }
        }

        public virtual void LateUpdate()
        {
            if (this.valid)
            {
                bool generateMeshOverride = this.generateMeshOverride != null;
                if (this.meshRenderer.enabled || generateMeshOverride)
                {
                    bool flag2;
                    SkeletonRendererInstruction currentInstructions = this.currentInstructions;
                    ExposedList<SubmeshInstruction> submeshInstructions = currentInstructions.submeshInstructions;
                    MeshRendererBuffers.SmartMesh nextMesh = this.rendererBuffers.GetNextMesh();
                    if (this.singleSubmesh)
                    {
                        MeshGenerator.GenerateSingleSubmeshInstruction(currentInstructions, this.skeleton, this.skeletonDataAsset.atlasAssets[0].materials[0]);
                        if (this.customMaterialOverride.Count > 0)
                        {
                            MeshGenerator.TryReplaceMaterials(submeshInstructions, this.customMaterialOverride);
                        }
                        MeshGenerator.Settings settings = new MeshGenerator.Settings {
                            pmaVertexColors = this.pmaVertexColors,
                            zSpacing = this.zSpacing,
                            useClipping = this.useClipping,
                            tintBlack = this.tintBlack,
                            calculateTangents = this.calculateTangents,
                            addNormals = this.addNormals
                        };
                        this.meshGenerator.settings = settings;
                        this.meshGenerator.Begin();
                        flag2 = SkeletonRendererInstruction.GeometryNotEqual(currentInstructions, nextMesh.instructionUsed);
                        if (currentInstructions.hasActiveClipping)
                        {
                            this.meshGenerator.AddSubmesh(submeshInstructions.Items[0], flag2);
                        }
                        else
                        {
                            this.meshGenerator.BuildMeshWithArrays(currentInstructions, flag2);
                        }
                    }
                    else
                    {
                        MeshGenerator.GenerateSkeletonRendererInstruction(currentInstructions, this.skeleton, this.customSlotMaterials, this.separatorSlots, generateMeshOverride, this.immutableTriangles);
                        if (this.customMaterialOverride.Count > 0)
                        {
                            MeshGenerator.TryReplaceMaterials(submeshInstructions, this.customMaterialOverride);
                        }
                        if (generateMeshOverride)
                        {
                            this.generateMeshOverride(currentInstructions);
                            if (this.disableRenderingOnOverride)
                            {
                                return;
                            }
                        }
                        flag2 = SkeletonRendererInstruction.GeometryNotEqual(currentInstructions, nextMesh.instructionUsed);
                        MeshGenerator.Settings settings2 = new MeshGenerator.Settings {
                            pmaVertexColors = this.pmaVertexColors,
                            zSpacing = this.zSpacing,
                            useClipping = this.useClipping,
                            tintBlack = this.tintBlack,
                            calculateTangents = this.calculateTangents,
                            addNormals = this.addNormals
                        };
                        this.meshGenerator.settings = settings2;
                        this.meshGenerator.Begin();
                        if (currentInstructions.hasActiveClipping)
                        {
                            this.meshGenerator.BuildMesh(currentInstructions, flag2);
                        }
                        else
                        {
                            this.meshGenerator.BuildMeshWithArrays(currentInstructions, flag2);
                        }
                    }
                    if (this.OnPostProcessVertices != null)
                    {
                        this.OnPostProcessVertices(this.meshGenerator.Buffers);
                    }
                    Mesh mesh = nextMesh.mesh;
                    this.meshGenerator.FillVertexData(mesh);
                    this.rendererBuffers.UpdateSharedMaterials(submeshInstructions);
                    if (flag2)
                    {
                        this.meshGenerator.FillTriangles(mesh);
                        this.meshRenderer.sharedMaterials = this.rendererBuffers.GetUpdatedSharedMaterialsArray();
                    }
                    else if (this.rendererBuffers.MaterialsChangedInLastUpdate())
                    {
                        this.meshRenderer.sharedMaterials = this.rendererBuffers.GetUpdatedSharedMaterialsArray();
                    }
                    this.meshGenerator.FillLateVertexData(mesh);
                    this.meshFilter.sharedMesh = mesh;
                    nextMesh.instructionUsed.Set(currentInstructions);
                }
            }
        }

        public static T NewSpineGameObject<T>(Spine.Unity.SkeletonDataAsset skeletonDataAsset) where T: SkeletonRenderer => 
            AddSpineComponent<T>(new GameObject("New Spine GameObject"), skeletonDataAsset);

        private void OnDestroy()
        {
            this.rendererBuffers.Dispose();
            this.valid = false;
        }

        private void OnDisable()
        {
            if (this.clearStateOnDisable && this.valid)
            {
                this.ClearState();
            }
        }

        public void SetMeshSettings(MeshGenerator.Settings settings)
        {
            this.calculateTangents = settings.calculateTangents;
            this.immutableTriangles = settings.immutableTriangles;
            this.pmaVertexColors = settings.pmaVertexColors;
            this.tintBlack = settings.tintBlack;
            this.useClipping = settings.useClipping;
            this.zSpacing = settings.zSpacing;
            this.meshGenerator.settings = settings;
        }

        public Spine.Unity.SkeletonDataAsset SkeletonDataAsset =>
            this.skeletonDataAsset;

        public Dictionary<Material, Material> CustomMaterialOverride =>
            this.customMaterialOverride;

        public Dictionary<Slot, Material> CustomSlotMaterials =>
            this.customSlotMaterials;

        public Spine.Skeleton Skeleton
        {
            get
            {
                this.Initialize(false);
                return this.skeleton;
            }
        }

        public delegate void InstructionDelegate(SkeletonRendererInstruction instruction);

        public delegate void SkeletonRendererDelegate(SkeletonRenderer skeletonRenderer);
    }
}

