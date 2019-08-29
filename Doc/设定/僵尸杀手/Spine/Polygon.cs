namespace Spine
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Polygon
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float[] <Vertices>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Count>k__BackingField;

        public Polygon()
        {
            this.Vertices = new float[0x10];
        }

        public float[] Vertices { get; set; }

        public int Count { get; set; }
    }
}

