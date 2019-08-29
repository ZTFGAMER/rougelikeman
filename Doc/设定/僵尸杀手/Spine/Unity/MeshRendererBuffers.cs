namespace Spine.Unity
{
    using Spine;
    using System;
    using UnityEngine;

    public class MeshRendererBuffers : IDisposable
    {
        private DoubleBuffered<SmartMesh> doubleBufferedMesh;
        internal readonly ExposedList<Material> submeshMaterials = new ExposedList<Material>();
        internal Material[] sharedMaterials = new Material[0];

        public void Clear()
        {
            this.sharedMaterials = new Material[0];
            this.submeshMaterials.Clear(true);
        }

        public void Dispose()
        {
            if (this.doubleBufferedMesh != null)
            {
                this.doubleBufferedMesh.GetNext().Dispose();
                this.doubleBufferedMesh.GetNext().Dispose();
                this.doubleBufferedMesh = null;
            }
        }

        public SmartMesh GetNextMesh() => 
            this.doubleBufferedMesh.GetNext();

        public Material[] GetUpdatedSharedMaterialsArray()
        {
            if (this.submeshMaterials.Count == this.sharedMaterials.Length)
            {
                this.submeshMaterials.CopyTo(this.sharedMaterials);
            }
            else
            {
                this.sharedMaterials = this.submeshMaterials.ToArray();
            }
            return this.sharedMaterials;
        }

        public void Initialize()
        {
            if (this.doubleBufferedMesh != null)
            {
                this.doubleBufferedMesh.GetNext().Clear();
                this.doubleBufferedMesh.GetNext().Clear();
                this.submeshMaterials.Clear(true);
            }
            else
            {
                this.doubleBufferedMesh = new DoubleBuffered<SmartMesh>();
            }
        }

        public bool MaterialsChangedInLastUpdate()
        {
            int count = this.submeshMaterials.Count;
            Material[] sharedMaterials = this.sharedMaterials;
            if (count != sharedMaterials.Length)
            {
                return true;
            }
            Material[] items = this.submeshMaterials.Items;
            for (int i = 0; i < count; i++)
            {
                if (!object.ReferenceEquals(items[i], sharedMaterials[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateSharedMaterials(ExposedList<SubmeshInstruction> instructions)
        {
            int count = instructions.Count;
            if (count > this.submeshMaterials.Items.Length)
            {
                Array.Resize<Material>(ref this.submeshMaterials.Items, count);
            }
            this.submeshMaterials.Count = count;
            Material[] items = this.submeshMaterials.Items;
            SubmeshInstruction[] instructionArray = instructions.Items;
            for (int i = 0; i < count; i++)
            {
                items[i] = instructionArray[i].material;
            }
        }

        public class SmartMesh : IDisposable
        {
            public Mesh mesh = SpineMesh.NewSkeletonMesh();
            public SkeletonRendererInstruction instructionUsed = new SkeletonRendererInstruction();

            public void Clear()
            {
                this.mesh.Clear();
                this.instructionUsed.Clear();
            }

            public void Dispose()
            {
                if (this.mesh != null)
                {
                    UnityEngine.Object.Destroy(this.mesh);
                }
                this.mesh = null;
            }
        }
    }
}

