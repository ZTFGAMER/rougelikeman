namespace Spine
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class SkeletonBounds
    {
        private ExposedList<Polygon> polygonPool = new ExposedList<Polygon>();
        private float minX;
        private float minY;
        private float maxX;
        private float maxY;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ExposedList<BoundingBoxAttachment> <BoundingBoxes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ExposedList<Polygon> <Polygons>k__BackingField;

        public SkeletonBounds()
        {
            this.BoundingBoxes = new ExposedList<BoundingBoxAttachment>();
            this.Polygons = new ExposedList<Polygon>();
        }

        private void AabbCompute()
        {
            float num = 2.147484E+09f;
            float num2 = 2.147484E+09f;
            float num3 = -2.147484E+09f;
            float num4 = -2.147484E+09f;
            ExposedList<Polygon> polygons = this.Polygons;
            int index = 0;
            int count = polygons.Count;
            while (index < count)
            {
                Polygon polygon = polygons.Items[index];
                float[] vertices = polygon.Vertices;
                int num7 = 0;
                int num8 = polygon.Count;
                while (num7 < num8)
                {
                    float num9 = vertices[num7];
                    float num10 = vertices[num7 + 1];
                    num = Math.Min(num, num9);
                    num2 = Math.Min(num2, num10);
                    num3 = Math.Max(num3, num9);
                    num4 = Math.Max(num4, num10);
                    num7 += 2;
                }
                index++;
            }
            this.minX = num;
            this.minY = num2;
            this.maxX = num3;
            this.maxY = num4;
        }

        public bool AabbContainsPoint(float x, float y) => 
            ((((x >= this.minX) && (x <= this.maxX)) && (y >= this.minY)) && (y <= this.maxY));

        public bool AabbIntersectsSegment(float x1, float y1, float x2, float y2)
        {
            float minX = this.minX;
            float minY = this.minY;
            float maxX = this.maxX;
            float maxY = this.maxY;
            if ((((x1 <= minX) && (x2 <= minX)) || ((y1 <= minY) && (y2 <= minY))) || (((x1 >= maxX) && (x2 >= maxX)) || ((y1 >= maxY) && (y2 >= maxY))))
            {
                return false;
            }
            float num5 = (y2 - y1) / (x2 - x1);
            float num6 = (num5 * (minX - x1)) + y1;
            if ((num6 > minY) && (num6 < maxY))
            {
                return true;
            }
            num6 = (num5 * (maxX - x1)) + y1;
            if ((num6 > minY) && (num6 < maxY))
            {
                return true;
            }
            float num7 = ((minY - y1) / num5) + x1;
            if ((num7 > minX) && (num7 < maxX))
            {
                return true;
            }
            num7 = ((maxY - y1) / num5) + x1;
            return ((num7 > minX) && (num7 < maxX));
        }

        public bool AabbIntersectsSkeleton(SkeletonBounds bounds) => 
            ((((this.minX < bounds.maxX) && (this.maxX > bounds.minX)) && (this.minY < bounds.maxY)) && (this.maxY > bounds.minY));

        public BoundingBoxAttachment ContainsPoint(float x, float y)
        {
            ExposedList<Polygon> polygons = this.Polygons;
            int index = 0;
            int count = polygons.Count;
            while (index < count)
            {
                if (this.ContainsPoint(polygons.Items[index], x, y))
                {
                    return this.BoundingBoxes.Items[index];
                }
                index++;
            }
            return null;
        }

        public bool ContainsPoint(Polygon polygon, float x, float y)
        {
            float[] vertices = polygon.Vertices;
            int count = polygon.Count;
            int index = count - 2;
            bool flag = false;
            for (int i = 0; i < count; i += 2)
            {
                float num4 = vertices[i + 1];
                float num5 = vertices[index + 1];
                if (((num4 < y) && (num5 >= y)) || ((num5 < y) && (num4 >= y)))
                {
                    float num6 = vertices[i];
                    if ((num6 + (((y - num4) / (num5 - num4)) * (vertices[index] - num6))) < x)
                    {
                        flag = !flag;
                    }
                }
                index = i;
            }
            return flag;
        }

        public Polygon GetPolygon(BoundingBoxAttachment attachment)
        {
            int index = this.BoundingBoxes.IndexOf(attachment);
            return ((index != -1) ? this.Polygons.Items[index] : null);
        }

        public BoundingBoxAttachment IntersectsSegment(float x1, float y1, float x2, float y2)
        {
            ExposedList<Polygon> polygons = this.Polygons;
            int index = 0;
            int count = polygons.Count;
            while (index < count)
            {
                if (this.IntersectsSegment(polygons.Items[index], x1, y1, x2, y2))
                {
                    return this.BoundingBoxes.Items[index];
                }
                index++;
            }
            return null;
        }

        public bool IntersectsSegment(Polygon polygon, float x1, float y1, float x2, float y2)
        {
            float[] vertices = polygon.Vertices;
            int count = polygon.Count;
            float num2 = x1 - x2;
            float num3 = y1 - y2;
            float num4 = (x1 * y2) - (y1 * x2);
            float num5 = vertices[count - 2];
            float num6 = vertices[count - 1];
            for (int i = 0; i < count; i += 2)
            {
                float num8 = vertices[i];
                float num9 = vertices[i + 1];
                float num10 = (num5 * num9) - (num6 * num8);
                float num11 = num5 - num8;
                float num12 = num6 - num9;
                float num13 = (num2 * num12) - (num3 * num11);
                float num14 = ((num4 * num11) - (num2 * num10)) / num13;
                if ((((num14 >= num5) && (num14 <= num8)) || ((num14 >= num8) && (num14 <= num5))) && (((num14 >= x1) && (num14 <= x2)) || ((num14 >= x2) && (num14 <= x1))))
                {
                    float num15 = ((num4 * num12) - (num3 * num10)) / num13;
                    if ((((num15 >= num6) && (num15 <= num9)) || ((num15 >= num9) && (num15 <= num6))) && (((num15 >= y1) && (num15 <= y2)) || ((num15 >= y2) && (num15 <= y1))))
                    {
                        return true;
                    }
                }
                num5 = num8;
                num6 = num9;
            }
            return false;
        }

        public void Update(Skeleton skeleton, bool updateAabb)
        {
            ExposedList<BoundingBoxAttachment> boundingBoxes = this.BoundingBoxes;
            ExposedList<Polygon> polygons = this.Polygons;
            ExposedList<Slot> slots = skeleton.slots;
            int count = slots.Count;
            boundingBoxes.Clear(true);
            int index = 0;
            int num3 = polygons.Count;
            while (index < num3)
            {
                this.polygonPool.Add(polygons.Items[index]);
                index++;
            }
            polygons.Clear(true);
            for (int i = 0; i < count; i++)
            {
                Slot slot = slots.Items[i];
                BoundingBoxAttachment item = slot.attachment as BoundingBoxAttachment;
                if (item != null)
                {
                    boundingBoxes.Add(item);
                    Polygon polygon = null;
                    int num5 = this.polygonPool.Count;
                    if (num5 > 0)
                    {
                        polygon = this.polygonPool.Items[num5 - 1];
                        this.polygonPool.RemoveAt(num5 - 1);
                    }
                    else
                    {
                        polygon = new Polygon();
                    }
                    polygons.Add(polygon);
                    int worldVerticesLength = item.worldVerticesLength;
                    polygon.Count = worldVerticesLength;
                    if (polygon.Vertices.Length < worldVerticesLength)
                    {
                        polygon.Vertices = new float[worldVerticesLength];
                    }
                    item.ComputeWorldVertices(slot, polygon.Vertices);
                }
            }
            if (updateAabb)
            {
                this.AabbCompute();
            }
            else
            {
                this.minX = -2.147484E+09f;
                this.minY = -2.147484E+09f;
                this.maxX = 2.147484E+09f;
                this.maxY = 2.147484E+09f;
            }
        }

        public ExposedList<BoundingBoxAttachment> BoundingBoxes { get; private set; }

        public ExposedList<Polygon> Polygons { get; private set; }

        public float MinX
        {
            get => 
                this.minX;
            set => 
                (this.minX = value);
        }

        public float MinY
        {
            get => 
                this.minY;
            set => 
                (this.minY = value);
        }

        public float MaxX
        {
            get => 
                this.maxX;
            set => 
                (this.maxX = value);
        }

        public float MaxY
        {
            get => 
                this.maxY;
            set => 
                (this.maxY = value);
        }

        public float Width =>
            (this.maxX - this.minX);

        public float Height =>
            (this.maxY - this.minY);
    }
}

