namespace Spine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    public class Atlas : IEnumerable<AtlasRegion>, IEnumerable
    {
        private readonly List<AtlasPage> pages;
        private List<AtlasRegion> regions;
        private TextureLoader textureLoader;

        public Atlas(List<AtlasPage> pages, List<AtlasRegion> regions)
        {
            this.pages = new List<AtlasPage>();
            this.regions = new List<AtlasRegion>();
            this.pages = pages;
            this.regions = regions;
            this.textureLoader = null;
        }

        public Atlas(TextReader reader, string dir, TextureLoader textureLoader)
        {
            this.pages = new List<AtlasPage>();
            this.regions = new List<AtlasRegion>();
            this.Load(reader, dir, textureLoader);
        }

        public void Dispose()
        {
            if (this.textureLoader != null)
            {
                int num = 0;
                int count = this.pages.Count;
                while (num < count)
                {
                    this.textureLoader.Unload(this.pages[num].rendererObject);
                    num++;
                }
            }
        }

        public AtlasRegion FindRegion(string name)
        {
            int num = 0;
            int count = this.regions.Count;
            while (num < count)
            {
                if (this.regions[num].name == name)
                {
                    return this.regions[num];
                }
                num++;
            }
            return null;
        }

        public void FlipV()
        {
            int num = 0;
            int count = this.regions.Count;
            while (num < count)
            {
                AtlasRegion region = this.regions[num];
                region.v = 1f - region.v;
                region.v2 = 1f - region.v2;
                num++;
            }
        }

        public IEnumerator<AtlasRegion> GetEnumerator() => 
            this.regions.GetEnumerator();

        private void Load(TextReader reader, string imagesDir, TextureLoader textureLoader)
        {
            if (textureLoader == null)
            {
                throw new ArgumentNullException("textureLoader", "textureLoader cannot be null.");
            }
            this.textureLoader = textureLoader;
            string[] tuple = new string[4];
            AtlasPage page = null;
            while (true)
            {
                string str = reader.ReadLine();
                if (str == null)
                {
                    return;
                }
                if (str.Trim().Length == 0)
                {
                    page = null;
                }
                else if (page == null)
                {
                    page = new AtlasPage {
                        name = str
                    };
                    if (ReadTuple(reader, tuple) == 2)
                    {
                        page.width = int.Parse(tuple[0]);
                        page.height = int.Parse(tuple[1]);
                        ReadTuple(reader, tuple);
                    }
                    page.format = (Format) Enum.Parse(typeof(Format), tuple[0], false);
                    ReadTuple(reader, tuple);
                    page.minFilter = (TextureFilter) Enum.Parse(typeof(TextureFilter), tuple[0], false);
                    page.magFilter = (TextureFilter) Enum.Parse(typeof(TextureFilter), tuple[1], false);
                    string str2 = ReadValue(reader);
                    page.uWrap = TextureWrap.ClampToEdge;
                    page.vWrap = TextureWrap.ClampToEdge;
                    switch (str2)
                    {
                        case "x":
                            page.uWrap = TextureWrap.Repeat;
                            break;

                        case "y":
                            page.vWrap = TextureWrap.Repeat;
                            break;

                        case "xy":
                            page.uWrap = page.vWrap = TextureWrap.Repeat;
                            break;
                    }
                    textureLoader.Load(page, Path.Combine(imagesDir, str));
                    this.pages.Add(page);
                }
                else
                {
                    AtlasRegion item = new AtlasRegion {
                        name = str,
                        page = page,
                        rotate = bool.Parse(ReadValue(reader))
                    };
                    ReadTuple(reader, tuple);
                    int num = int.Parse(tuple[0]);
                    int num2 = int.Parse(tuple[1]);
                    ReadTuple(reader, tuple);
                    int num3 = int.Parse(tuple[0]);
                    int num4 = int.Parse(tuple[1]);
                    item.u = ((float) num) / ((float) page.width);
                    item.v = ((float) num2) / ((float) page.height);
                    if (item.rotate)
                    {
                        item.u2 = ((float) (num + num4)) / ((float) page.width);
                        item.v2 = ((float) (num2 + num3)) / ((float) page.height);
                    }
                    else
                    {
                        item.u2 = ((float) (num + num3)) / ((float) page.width);
                        item.v2 = ((float) (num2 + num4)) / ((float) page.height);
                    }
                    item.x = num;
                    item.y = num2;
                    item.width = Math.Abs(num3);
                    item.height = Math.Abs(num4);
                    if (ReadTuple(reader, tuple) == 4)
                    {
                        item.splits = new int[] { int.Parse(tuple[0]), int.Parse(tuple[1]), int.Parse(tuple[2]), int.Parse(tuple[3]) };
                        if (ReadTuple(reader, tuple) == 4)
                        {
                            item.pads = new int[] { int.Parse(tuple[0]), int.Parse(tuple[1]), int.Parse(tuple[2]), int.Parse(tuple[3]) };
                            ReadTuple(reader, tuple);
                        }
                    }
                    item.originalWidth = int.Parse(tuple[0]);
                    item.originalHeight = int.Parse(tuple[1]);
                    ReadTuple(reader, tuple);
                    item.offsetX = int.Parse(tuple[0]);
                    item.offsetY = int.Parse(tuple[1]);
                    item.index = int.Parse(ReadValue(reader));
                    this.regions.Add(item);
                }
            }
        }

        private static int ReadTuple(TextReader reader, string[] tuple)
        {
            string str = reader.ReadLine();
            int index = str.IndexOf(':');
            if (index == -1)
            {
                throw new Exception("Invalid line: " + str);
            }
            int num2 = 0;
            int startIndex = index + 1;
            while (num2 < 3)
            {
                int num4 = str.IndexOf(',', startIndex);
                if (num4 == -1)
                {
                    break;
                }
                tuple[num2] = str.Substring(startIndex, num4 - startIndex).Trim();
                startIndex = num4 + 1;
                num2++;
            }
            tuple[num2] = str.Substring(startIndex).Trim();
            return (num2 + 1);
        }

        private static string ReadValue(TextReader reader)
        {
            string str = reader.ReadLine();
            int index = str.IndexOf(':');
            if (index == -1)
            {
                throw new Exception("Invalid line: " + str);
            }
            return str.Substring(index + 1).Trim();
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.regions.GetEnumerator();
    }
}

