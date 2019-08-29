namespace Spine.Unity
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct MeshGeneratorBuffers
    {
        public int vertexCount;
        public Vector3[] vertexBuffer;
        public Vector2[] uvBuffer;
        public Color32[] colorBuffer;
        public MeshGenerator meshGenerator;
    }
}

