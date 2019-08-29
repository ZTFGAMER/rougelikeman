namespace Spine
{
    using System;

    public interface IConstraint : IUpdatable
    {
        int Order { get; }
    }
}

