namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Runtime.InteropServices;

    public class SpineBone : SpineAttributeBase
    {
        public SpineBone(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false)
        {
            base.startsWith = startsWith;
            base.dataField = dataField;
            base.includeNone = includeNone;
            base.fallbackToTextField = fallbackToTextField;
        }

        public static Bone GetBone(string boneName, SkeletonRenderer renderer) => 
            renderer.skeleton?.FindBone(boneName);

        public static BoneData GetBoneData(string boneName, SkeletonDataAsset skeletonDataAsset) => 
            skeletonDataAsset.GetSkeletonData(true).FindBone(boneName);
    }
}

