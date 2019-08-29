namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode, RequireComponent(typeof(CanvasRenderer), typeof(RectTransform)), DisallowMultipleComponent, AddComponentMenu("Spine/SkeletonGraphic (Unity UI Canvas)")]
    public class SkeletonGraphic : MaskableGraphic, ISkeletonComponent, IAnimationStateComponent, ISkeletonAnimation, IHasSkeletonDataAsset
    {
        public Spine.Unity.SkeletonDataAsset skeletonDataAsset;
        [SpineSkin("", "skeletonDataAsset", true, false)]
        public string initialSkinName = "default";
        public bool initialFlipX;
        public bool initialFlipY;
        [SpineAnimation("", "skeletonDataAsset", true, false)]
        public string startingAnimation;
        public bool startingLoop;
        public float timeScale = 1f;
        public bool freeze;
        public bool unscaledTime;
        private Texture overrideTexture;
        protected Spine.Skeleton skeleton;
        protected Spine.AnimationState state;
        [SerializeField]
        protected Spine.Unity.MeshGenerator meshGenerator = new Spine.Unity.MeshGenerator();
        private DoubleBuffered<MeshRendererBuffers.SmartMesh> meshBuffers;
        private SkeletonRendererInstruction currentInstructions = new SkeletonRendererInstruction();

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event MeshGeneratorDelegate OnPostProcessVertices;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event UpdateBonesDelegate UpdateComplete;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event UpdateBonesDelegate UpdateLocal;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event UpdateBonesDelegate UpdateWorld;

        public static SkeletonGraphic AddSkeletonGraphicComponent(GameObject gameObject, Spine.Unity.SkeletonDataAsset skeletonDataAsset)
        {
            SkeletonGraphic graphic = gameObject.AddComponent<SkeletonGraphic>();
            if (skeletonDataAsset != null)
            {
                graphic.skeletonDataAsset = skeletonDataAsset;
                graphic.Initialize(false);
            }
            return graphic;
        }

        protected override void Awake()
        {
            base.Awake();
            if (!this.IsValid)
            {
                this.Initialize(false);
                this.Rebuild(CanvasUpdate.PreRender);
            }
        }

        public void Clear()
        {
            this.skeleton = null;
            base.get_canvasRenderer().Clear();
        }

        public Mesh GetLastMesh() => 
            this.meshBuffers.GetCurrent().mesh;

        public void Initialize(bool overwrite)
        {
            if ((!this.IsValid || overwrite) && (this.skeletonDataAsset != null))
            {
                Spine.SkeletonData skeletonData = this.skeletonDataAsset.GetSkeletonData(false);
                if ((skeletonData != null) && ((this.skeletonDataAsset.atlasAssets.Length > 0) && (this.skeletonDataAsset.atlasAssets[0].materials.Length > 0)))
                {
                    this.state = new Spine.AnimationState(this.skeletonDataAsset.GetAnimationStateData());
                    if (this.state == null)
                    {
                        this.Clear();
                    }
                    else
                    {
                        Spine.Skeleton skeleton = new Spine.Skeleton(skeletonData) {
                            flipX = this.initialFlipX,
                            flipY = this.initialFlipY
                        };
                        this.skeleton = skeleton;
                        this.meshBuffers = new DoubleBuffered<MeshRendererBuffers.SmartMesh>();
                        base.get_canvasRenderer().SetTexture(this.get_mainTexture());
                        if (!string.IsNullOrEmpty(this.initialSkinName))
                        {
                            this.skeleton.SetSkin(this.initialSkinName);
                        }
                        if (!string.IsNullOrEmpty(this.startingAnimation))
                        {
                            this.state.SetAnimation(0, this.startingAnimation, this.startingLoop);
                            this.Update(0f);
                        }
                    }
                }
            }
        }

        public void LateUpdate()
        {
            if (!this.freeze)
            {
                this.UpdateMesh();
            }
        }

        public static SkeletonGraphic NewSkeletonGraphicGameObject(Spine.Unity.SkeletonDataAsset skeletonDataAsset, Transform parent)
        {
            SkeletonGraphic graphic = AddSkeletonGraphicComponent(new GameObject("New Spine GameObject"), skeletonDataAsset);
            if (parent != null)
            {
                graphic.transform.SetParent(parent, false);
            }
            return graphic;
        }

        public override void Rebuild(CanvasUpdate update)
        {
            base.Rebuild(update);
            if (!base.get_canvasRenderer().cull && (update == CanvasUpdate.PreRender))
            {
                this.UpdateMesh();
            }
        }

        public virtual void Update()
        {
            if (!this.freeze)
            {
                this.Update(!this.unscaledTime ? Time.deltaTime : Time.unscaledDeltaTime);
            }
        }

        public virtual void Update(float deltaTime)
        {
            if (this.IsValid)
            {
                deltaTime *= this.timeScale;
                this.skeleton.Update(deltaTime);
                this.state.Update(deltaTime);
                this.state.Apply(this.skeleton);
                if (this.UpdateLocal != null)
                {
                    this.UpdateLocal(this);
                }
                this.skeleton.UpdateWorldTransform();
                if (this.UpdateWorld != null)
                {
                    this.UpdateWorld(this);
                    this.skeleton.UpdateWorldTransform();
                }
                if (this.UpdateComplete != null)
                {
                    this.UpdateComplete(this);
                }
            }
        }

        public void UpdateMesh()
        {
            if (this.IsValid)
            {
                this.skeleton.SetColor(this.get_color());
                MeshRendererBuffers.SmartMesh next = this.meshBuffers.GetNext();
                SkeletonRendererInstruction currentInstructions = this.currentInstructions;
                Spine.Unity.MeshGenerator.GenerateSingleSubmeshInstruction(currentInstructions, this.skeleton, this.get_material());
                bool updateTriangles = SkeletonRendererInstruction.GeometryNotEqual(currentInstructions, next.instructionUsed);
                this.meshGenerator.Begin();
                if (currentInstructions.hasActiveClipping)
                {
                    this.meshGenerator.AddSubmesh(currentInstructions.submeshInstructions.Items[0], updateTriangles);
                }
                else
                {
                    this.meshGenerator.BuildMeshWithArrays(currentInstructions, updateTriangles);
                }
                if (base.get_canvas() != null)
                {
                    this.meshGenerator.ScaleVertexData(base.get_canvas().referencePixelsPerUnit);
                }
                if (this.OnPostProcessVertices != null)
                {
                    this.OnPostProcessVertices(this.meshGenerator.Buffers);
                }
                Mesh mesh = next.mesh;
                this.meshGenerator.FillVertexData(mesh);
                if (updateTriangles)
                {
                    this.meshGenerator.FillTrianglesSingle(mesh);
                }
                this.meshGenerator.FillLateVertexData(mesh);
                base.get_canvasRenderer().SetMesh(mesh);
                next.instructionUsed.Set(currentInstructions);
            }
        }

        public Spine.Unity.SkeletonDataAsset SkeletonDataAsset =>
            this.skeletonDataAsset;

        public Texture OverrideTexture
        {
            get => 
                this.overrideTexture;
            set
            {
                this.overrideTexture = value;
                base.get_canvasRenderer().SetTexture(this.get_mainTexture());
            }
        }

        public override Texture mainTexture
        {
            get
            {
                if (this.overrideTexture != null)
                {
                    return this.overrideTexture;
                }
                return ((this.skeletonDataAsset != null) ? this.skeletonDataAsset.atlasAssets[0].materials[0].mainTexture : null);
            }
        }

        public Spine.Skeleton Skeleton
        {
            get => 
                this.skeleton;
            internal set => 
                (this.skeleton = value);
        }

        public Spine.SkeletonData SkeletonData =>
            ((this.skeleton != null) ? this.skeleton.data : null);

        public bool IsValid =>
            (this.skeleton != null);

        public Spine.AnimationState AnimationState =>
            this.state;

        public Spine.Unity.MeshGenerator MeshGenerator =>
            this.meshGenerator;
    }
}

