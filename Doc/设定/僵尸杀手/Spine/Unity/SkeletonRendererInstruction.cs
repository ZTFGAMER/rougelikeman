namespace Spine.Unity
{
    using Spine;
    using System;

    public class SkeletonRendererInstruction
    {
        public bool immutableTriangles;
        public readonly ExposedList<SubmeshInstruction> submeshInstructions = new ExposedList<SubmeshInstruction>();
        public bool hasActiveClipping;
        public int rawVertexCount = -1;
        public readonly ExposedList<Attachment> attachments = new ExposedList<Attachment>();

        public void Clear()
        {
            this.attachments.Clear(false);
            this.rawVertexCount = -1;
            this.hasActiveClipping = false;
            this.submeshInstructions.Clear(false);
        }

        public void Dispose()
        {
            this.attachments.Clear(true);
        }

        public static bool GeometryNotEqual(SkeletonRendererInstruction a, SkeletonRendererInstruction b)
        {
            if (a.hasActiveClipping || b.hasActiveClipping)
            {
                return true;
            }
            if (a.rawVertexCount != b.rawVertexCount)
            {
                return true;
            }
            if (a.immutableTriangles != b.immutableTriangles)
            {
                return true;
            }
            int count = b.attachments.Count;
            if (a.attachments.Count != count)
            {
                return true;
            }
            int num2 = a.submeshInstructions.Count;
            int num3 = b.submeshInstructions.Count;
            if (num2 != num3)
            {
                return true;
            }
            SubmeshInstruction[] items = a.submeshInstructions.Items;
            SubmeshInstruction[] instructionArray2 = b.submeshInstructions.Items;
            Attachment[] attachmentArray = a.attachments.Items;
            Attachment[] attachmentArray2 = b.attachments.Items;
            for (int i = 0; i < count; i++)
            {
                if (!object.ReferenceEquals(attachmentArray[i], attachmentArray2[i]))
                {
                    return true;
                }
            }
            for (int j = 0; j < num3; j++)
            {
                SubmeshInstruction instruction = items[j];
                SubmeshInstruction instruction2 = instructionArray2[j];
                if (((instruction.rawVertexCount != instruction2.rawVertexCount) || (instruction.startSlot != instruction2.startSlot)) || (((instruction.endSlot != instruction2.endSlot) || (instruction.rawTriangleCount != instruction2.rawTriangleCount)) || (instruction.rawFirstVertexIndex != instruction2.rawFirstVertexIndex)))
                {
                    return true;
                }
            }
            return false;
        }

        public void Set(SkeletonRendererInstruction other)
        {
            this.immutableTriangles = other.immutableTriangles;
            this.hasActiveClipping = other.hasActiveClipping;
            this.rawVertexCount = other.rawVertexCount;
            this.attachments.Clear(false);
            this.attachments.GrowIfNeeded(other.attachments.Capacity);
            this.attachments.Count = other.attachments.Count;
            other.attachments.CopyTo(this.attachments.Items);
            this.submeshInstructions.Clear(false);
            this.submeshInstructions.GrowIfNeeded(other.submeshInstructions.Capacity);
            this.submeshInstructions.Count = other.submeshInstructions.Count;
            other.submeshInstructions.CopyTo(this.submeshInstructions.Items);
        }

        public void SetWithSubset(ExposedList<SubmeshInstruction> instructions, int startSubmesh, int endSubmesh)
        {
            int num = 0;
            ExposedList<SubmeshInstruction> submeshInstructions = this.submeshInstructions;
            submeshInstructions.Clear(false);
            int newSize = endSubmesh - startSubmesh;
            submeshInstructions.Resize(newSize);
            SubmeshInstruction[] items = submeshInstructions.Items;
            SubmeshInstruction[] instructionArray2 = instructions.Items;
            for (int i = 0; i < newSize; i++)
            {
                SubmeshInstruction instruction = instructionArray2[startSubmesh + i];
                items[i] = instruction;
                this.hasActiveClipping |= instruction.hasClipping;
                items[i].rawFirstVertexIndex = num;
                num += instruction.rawVertexCount;
            }
            this.rawVertexCount = num;
            int startSlot = instructionArray2[startSubmesh].startSlot;
            int endSlot = instructionArray2[endSubmesh - 1].endSlot;
            this.attachments.Clear(false);
            int num6 = endSlot - startSlot;
            this.attachments.Resize(num6);
            Attachment[] attachmentArray = this.attachments.Items;
            Slot[] slotArray = instructionArray2[0].skeleton.drawOrder.Items;
            for (int j = 0; j < num6; j++)
            {
                attachmentArray[j] = slotArray[startSlot + j].attachment;
            }
        }
    }
}

