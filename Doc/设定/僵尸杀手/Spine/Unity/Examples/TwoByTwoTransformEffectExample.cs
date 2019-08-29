namespace Spine.Unity.Examples
{
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class TwoByTwoTransformEffectExample : MonoBehaviour
    {
        public Vector2 xAxis = new Vector2(1f, 0f);
        public Vector2 yAxis = new Vector2(0f, 1f);
        private SkeletonRenderer skeletonRenderer;

        private void OnDisable()
        {
            if (this.skeletonRenderer != null)
            {
                this.skeletonRenderer.OnPostProcessVertices -= new MeshGeneratorDelegate(this.ProcessVertices);
                Debug.Log("2x2 Transform Effect Disabled.");
            }
        }

        private void OnEnable()
        {
            this.skeletonRenderer = base.GetComponent<SkeletonRenderer>();
            if (this.skeletonRenderer != null)
            {
                this.skeletonRenderer.OnPostProcessVertices -= new MeshGeneratorDelegate(this.ProcessVertices);
                this.skeletonRenderer.OnPostProcessVertices += new MeshGeneratorDelegate(this.ProcessVertices);
                Debug.Log("2x2 Transform Effect Enabled.");
            }
        }

        private void ProcessVertices(MeshGeneratorBuffers buffers)
        {
            if (base.enabled)
            {
                int vertexCount = buffers.vertexCount;
                Vector3[] vertexBuffer = buffers.vertexBuffer;
                Vector3 vector = new Vector3();
                for (int i = 0; i < vertexCount; i++)
                {
                    Vector3 vector3 = vertexBuffer[i];
                    vector.x = (this.xAxis.x * vector3.x) + (this.yAxis.x * vector3.y);
                    vector.y = (this.xAxis.y * vector3.x) + (this.yAxis.y * vector3.y);
                    vertexBuffer[i] = vector;
                }
            }
        }
    }
}

