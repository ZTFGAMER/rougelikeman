namespace Spine.Unity
{
    using Spine;
    using System;

    public interface ISkeletonAnimation
    {
        event UpdateBonesDelegate UpdateComplete;

        event UpdateBonesDelegate UpdateLocal;

        event UpdateBonesDelegate UpdateWorld;

        Spine.Skeleton Skeleton { get; }
    }
}

