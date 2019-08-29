namespace Spine.Unity.Modules
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(UnityEngine.MeshRenderer), typeof(UnityEngine.MeshFilter))]
    public class SkeletonPartsRenderer : MonoBehaviour
    {
        private Spine.Unity.MeshGenerator meshGenerator;
        private UnityEngine.MeshRenderer meshRenderer;
        private UnityEngine.MeshFilter meshFilter;
        private MeshRendererBuffers buffers;
        private SkeletonRendererInstruction currentInstructions = new SkeletonRendererInstruction();

        public void ClearMesh()
        {
            this.LazyIntialize();
            this.meshFilter.sharedMesh = null;
        }

        private void LazyIntialize()
        {
            if (this.buffers == null)
            {
                this.buffers = new MeshRendererBuffers();
                this.buffers.Initialize();
                if (this.meshGenerator == null)
                {
                    this.meshGenerator = new Spine.Unity.MeshGenerator();
                    this.meshFilter = base.GetComponent<UnityEngine.MeshFilter>();
                    this.meshRenderer = base.GetComponent<UnityEngine.MeshRenderer>();
                    this.currentInstructions.Clear();
                }
            }
        }

        public static SkeletonPartsRenderer NewPartsRendererGameObject(Transform parent, string name)
        {
            Type[] components = new Type[] { typeof(UnityEngine.MeshFilter), typeof(UnityEngine.MeshRenderer) };
            GameObject obj2 = new GameObject(name, components);
            obj2.transform.SetParent(parent, false);
            return obj2.AddComponent<SkeletonPartsRenderer>();
        }

        public void RenderParts(ExposedList<SubmeshInstruction> instructions, int startSubmesh, int endSubmesh)
        {
            this.LazyIntialize();
            MeshRendererBuffers.SmartMesh nextMesh = this.buffers.GetNextMesh();
            this.currentInstructions.SetWithSubset(instructions, startSubmesh, endSubmesh);
            bool updateTriangles = SkeletonRendererInstruction.GeometryNotEqual(this.currentInstructions, nextMesh.instructionUsed);
            SubmeshInstruction[] items = this.currentInstructions.submeshInstructions.Items;
            this.meshGenerator.Begin();
            if (this.currentInstructions.hasActiveClipping)
            {
                for (int i = 0; i < this.currentInstructions.submeshInstructions.Count; i++)
                {
                    this.meshGenerator.AddSubmesh(items[i], updateTriangles);
                }
            }
            else
            {
                this.meshGenerator.BuildMeshWithArrays(this.currentInstructions, updateTriangles);
            }
            this.buffers.UpdateSharedMaterials(this.currentInstructions.submeshInstructions);
            Mesh mesh = nextMesh.mesh;
            if (this.meshGenerator.VertexCount <= 0)
            {
                updateTriangles = false;
                mesh.Clear();
            }
            else
            {
                this.meshGenerator.FillVertexData(mesh);
                if (updateTriangles)
                {
                    this.meshGenerator.FillTriangles(mesh);
                    this.meshRenderer.sharedMaterials = this.buffers.GetUpdatedSharedMaterialsArray();
                }
                else if (this.buffers.MaterialsChangedInLastUpdate())
                {
                    this.meshRenderer.sharedMaterials = this.buffers.GetUpdatedSharedMaterialsArray();
                }
            }
            this.meshGenerator.FillLateVertexData(mesh);
            this.meshFilter.sharedMesh = mesh;
            nextMesh.instructionUsed.Set(this.currentInstructions);
        }

        public void SetPropertyBlock(MaterialPropertyBlock block)
        {
            this.LazyIntialize();
            this.meshRenderer.SetPropertyBlock(block);
        }

        public Spine.Unity.MeshGenerator MeshGenerator
        {
            get
            {
                this.LazyIntialize();
                return this.meshGenerator;
            }
        }

        public UnityEngine.MeshRenderer MeshRenderer
        {
            get
            {
                this.LazyIntialize();
                return this.meshRenderer;
            }
        }

        public UnityEngine.MeshFilter MeshFilter
        {
            get
            {
                this.LazyIntialize();
                return this.meshFilter;
            }
        }
    }
}

