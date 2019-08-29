namespace Spine.Unity.Examples
{
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class JitterEffectExample : MonoBehaviour
    {
        [Range(0f, 0.8f)]
        public float jitterMagnitude = 0.2f;
        private SkeletonRenderer skeletonRenderer;

        private void OnDisable()
        {
            if (this.skeletonRenderer != null)
            {
                this.skeletonRenderer.OnPostProcessVertices -= new MeshGeneratorDelegate(this.ProcessVertices);
                Debug.Log("Jitter Effect Disabled.");
            }
        }

        private void OnEnable()
        {
            this.skeletonRenderer = base.GetComponent<SkeletonRenderer>();
            if (this.skeletonRenderer != null)
            {
                this.skeletonRenderer.OnPostProcessVertices -= new MeshGeneratorDelegate(this.ProcessVertices);
                this.skeletonRenderer.OnPostProcessVertices += new MeshGeneratorDelegate(this.ProcessVertices);
                Debug.Log("Jitter Effect Enabled.");
            }
        }

        private void ProcessVertices(MeshGeneratorBuffers buffers)
        {
            if (base.enabled)
            {
                int vertexCount = buffers.vertexCount;
                Vector3[] vertexBuffer = buffers.vertexBuffer;
                for (int i = 0; i < vertexCount; i++)
                {
                    vertexBuffer[i] += UnityEngine.Random.insideUnitCircle * this.jitterMagnitude;
                }
            }
        }
    }
}

