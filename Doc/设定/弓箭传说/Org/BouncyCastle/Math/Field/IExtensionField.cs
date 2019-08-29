namespace Org.BouncyCastle.Math.Field
{
    using System;

    public interface IExtensionField : IFiniteField
    {
        IFiniteField Subfield { get; }

        int Degree { get; }
    }
}

