namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct SubmeshInstruction
    {
        public Skeleton skeleton;
        public int startSlot;
        public int endSlot;
        public Material material;
        public bool forceSeparate;
        public int preActiveClippingSlotSource;
        public int rawTriangleCount;
        public int rawVertexCount;
        public int rawFirstVertexIndex;
        public bool hasClipping;
        public int SlotCount =>
            (this.endSlot - this.startSlot);
        public override string ToString() => 
            $"[SubmeshInstruction: slots {this.startSlot} to {(this.endSlot - 1)}. (Material){((this.material != null) ? this.material.name : "<none>")}. preActiveClippingSlotSource:{this.preActiveClippingSlotSource}]";
    }
}

