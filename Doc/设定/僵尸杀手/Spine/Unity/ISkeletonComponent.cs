namespace Spine.Unity
{
    using Spine;

    public interface ISkeletonComponent
    {
        Spine.Unity.SkeletonDataAsset SkeletonDataAsset { get; }

        Spine.Skeleton Skeleton { get; }
    }
}

