namespace Spine
{
    using System;

    [Flags]
    public enum TransformMode
    {
        Normal = 0,
        OnlyTranslation = 7,
        NoRotationOrReflection = 1,
        NoScale = 2,
        NoScaleOrReflection = 6
    }
}

