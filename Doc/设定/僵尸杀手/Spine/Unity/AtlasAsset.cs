namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class AtlasAsset : ScriptableObject
    {
        public TextAsset atlasFile;
        public Material[] materials;
        protected Atlas atlas;

        public virtual void Clear()
        {
            this.atlas = null;
        }

        public static AtlasAsset CreateRuntimeInstance(TextAsset atlasText, Material[] materials, bool initialize)
        {
            AtlasAsset asset = ScriptableObject.CreateInstance<AtlasAsset>();
            asset.Reset();
            asset.atlasFile = atlasText;
            asset.materials = materials;
            if (initialize)
            {
                asset.GetAtlas();
            }
            return asset;
        }

        public static AtlasAsset CreateRuntimeInstance(TextAsset atlasText, Texture2D[] textures, Material materialPropertySource, bool initialize)
        {
            char[] separator = new char[] { '\n' };
            string[] strArray = atlasText.text.Replace("\r", string.Empty).Split(separator);
            List<string> list = new List<string>();
            for (int i = 0; i < (strArray.Length - 1); i++)
            {
                if (strArray[i].Trim().Length == 0)
                {
                    list.Add(strArray[i + 1].Trim().Replace(".png", string.Empty));
                }
            }
            Material[] materials = new Material[list.Count];
            int index = 0;
            int count = list.Count;
            while (index < count)
            {
                Material material = null;
                string a = list[index];
                int num4 = 0;
                int length = textures.Length;
                while (num4 < length)
                {
                    if (string.Equals(a, textures[num4].name, StringComparison.OrdinalIgnoreCase))
                    {
                        material = new Material(materialPropertySource) {
                            mainTexture = textures[num4]
                        };
                        break;
                    }
                    num4++;
                }
                if (material == null)
                {
                    throw new ArgumentException("Could not find matching atlas page in the texture array.");
                }
                materials[index] = material;
                index++;
            }
            return CreateRuntimeInstance(atlasText, materials, initialize);
        }

        public static AtlasAsset CreateRuntimeInstance(TextAsset atlasText, Texture2D[] textures, Shader shader, bool initialize)
        {
            if (shader == null)
            {
                shader = Shader.Find("Spine/Skeleton");
            }
            Material materialPropertySource = new Material(shader);
            return CreateRuntimeInstance(atlasText, textures, materialPropertySource, initialize);
        }

        public Mesh GenerateMesh(string name, Mesh mesh, out Material material, float scale = 0.01f)
        {
            AtlasRegion region = this.atlas.FindRegion(name);
            material = null;
            if (region != null)
            {
                if (mesh == null)
                {
                    mesh = new Mesh();
                    mesh.name = name;
                }
                Vector3[] vectorArray = new Vector3[4];
                Vector2[] vectorArray2 = new Vector2[4];
                Color[] colorArray = new Color[] { Color.white, Color.white, Color.white, Color.white };
                int[] numArray = new int[] { 0, 1, 2, 2, 3, 0 };
                float x = ((float) region.width) / -2f;
                float num2 = x * -1f;
                float y = ((float) region.height) / 2f;
                float num4 = y * -1f;
                vectorArray[0] = new Vector3(x, num4, 0f) * scale;
                vectorArray[1] = new Vector3(x, y, 0f) * scale;
                vectorArray[2] = new Vector3(num2, y, 0f) * scale;
                vectorArray[3] = new Vector3(num2, num4, 0f) * scale;
                float u = region.u;
                float v = region.v;
                float num7 = region.u2;
                float num8 = region.v2;
                if (!region.rotate)
                {
                    vectorArray2[0] = new Vector2(u, num8);
                    vectorArray2[1] = new Vector2(u, v);
                    vectorArray2[2] = new Vector2(num7, v);
                    vectorArray2[3] = new Vector2(num7, num8);
                }
                else
                {
                    vectorArray2[0] = new Vector2(num7, num8);
                    vectorArray2[1] = new Vector2(u, num8);
                    vectorArray2[2] = new Vector2(u, v);
                    vectorArray2[3] = new Vector2(num7, v);
                }
                mesh.triangles = new int[0];
                mesh.vertices = vectorArray;
                mesh.uv = vectorArray2;
                mesh.colors = colorArray;
                mesh.triangles = numArray;
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
                material = (Material) region.page.rendererObject;
                return mesh;
            }
            mesh = null;
            return mesh;
        }

        public virtual Atlas GetAtlas()
        {
            if (this.atlasFile == null)
            {
                Debug.LogError("Atlas file not set for atlas asset: " + base.name, this);
                this.Clear();
                return null;
            }
            if ((this.materials == null) || (this.materials.Length == 0))
            {
                Debug.LogError("Materials not set for atlas asset: " + base.name, this);
                this.Clear();
                return null;
            }
            if (this.atlas != null)
            {
                return this.atlas;
            }
            try
            {
                this.atlas = new Atlas(new StringReader(this.atlasFile.text), string.Empty, new MaterialsTextureLoader(this));
                this.atlas.FlipV();
                return this.atlas;
            }
            catch (Exception exception)
            {
                Debug.LogError("Error reading atlas file for atlas asset: " + base.name + "\n" + exception.Message + "\n" + exception.StackTrace, this);
                return null;
            }
        }

        private void Reset()
        {
            this.Clear();
        }

        public bool IsLoaded =>
            (this.atlas != null);
    }
}

