namespace Spine.Unity
{
    using System;
    using UnityEngine;

    public static class SpineMesh
    {
        internal const HideFlags MeshHideflags = (HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor);

        public static Mesh NewSkeletonMesh()
        {
            Mesh mesh = new Mesh();
            mesh.MarkDynamic();
            mesh.name = "Skeleton Mesh";
            mesh.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
            return mesh;
        }
    }
}

