namespace Spine.Unity.Modules.AttachmentTools
{
    using Spine;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class AtlasUtilities
    {
        internal const TextureFormat SpineTextureFormat = TextureFormat.RGBA32;
        internal const float DefaultMipmapBias = -0.5f;
        internal const bool UseMipMaps = false;
        internal const float DefaultScale = 0.01f;
        private const int NonrenderingRegion = -1;
        private static Dictionary<AtlasRegion, Texture2D> CachedRegionTextures = new Dictionary<AtlasRegion, Texture2D>();
        private static List<Texture2D> CachedRegionTexturesList = new List<Texture2D>();

        private static void ApplyPMA(this Texture2D texture, bool applyImmediately = true)
        {
            Color[] pixels = texture.GetPixels();
            int index = 0;
            int length = pixels.Length;
            while (index < length)
            {
                Color color = pixels[index];
                float a = color.a;
                color.r *= a;
                color.g *= a;
                color.b *= a;
                pixels[index] = color;
                index++;
            }
            texture.SetPixels(pixels);
            if (applyImmediately)
            {
                texture.Apply();
            }
        }

        public static void ClearCache()
        {
            foreach (Texture2D textured in CachedRegionTexturesList)
            {
                UnityEngine.Object.Destroy(textured);
            }
            CachedRegionTextures.Clear();
            CachedRegionTexturesList.Clear();
        }

        private static Texture2D GetClone(this Texture2D t, bool applyImmediately = true, TextureFormat textureFormat = 4, bool mipmaps = false)
        {
            Color[] colors = t.GetPixels(0, 0, t.width, t.height);
            Texture2D textured = new Texture2D(t.width, t.height, textureFormat, mipmaps);
            textured.SetPixels(colors);
            if (applyImmediately)
            {
                textured.Apply();
            }
            return textured;
        }

        private static Texture2D GetMainTexture(this AtlasRegion region)
        {
            Material rendererObject = region.page.rendererObject as Material;
            return (rendererObject.mainTexture as Texture2D);
        }

        public static void GetRepackedAttachments(List<Attachment> sourceAttachments, List<Attachment> outputAttachments, Material materialPropertySource, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 0x400, int padding = 2, TextureFormat textureFormat = 4, bool mipmaps = false, string newAssetName = "Repacked Attachments", bool clearCache = false, bool useOriginalNonrenderables = true)
        {
            if (sourceAttachments == null)
            {
                throw new ArgumentNullException("sourceAttachments");
            }
            if (outputAttachments == null)
            {
                throw new ArgumentNullException("outputAttachments");
            }
            Dictionary<AtlasRegion, int> dictionary = new Dictionary<AtlasRegion, int>();
            List<int> list = new List<int>();
            List<Texture2D> list2 = new List<Texture2D>();
            List<AtlasRegion> list3 = new List<AtlasRegion>();
            outputAttachments.Clear();
            outputAttachments.AddRange(sourceAttachments);
            int num = 0;
            int num2 = 0;
            int count = sourceAttachments.Count;
            while (num2 < count)
            {
                Attachment a = sourceAttachments[num2];
                if (IsRenderable(a))
                {
                    Attachment clone = a.GetClone(true);
                    AtlasRegion key = clone.GetRegion();
                    if (dictionary.TryGetValue(key, out int num4))
                    {
                        list.Add(num4);
                    }
                    else
                    {
                        list3.Add(key);
                        list2.Add(key.ToTexture(true, TextureFormat.RGBA32, false));
                        dictionary.Add(key, num);
                        list.Add(num);
                        num++;
                    }
                    outputAttachments[num2] = clone;
                }
                else
                {
                    outputAttachments[num2] = !useOriginalNonrenderables ? a.GetClone(true) : a;
                    list.Add(-1);
                }
                num2++;
            }
            Texture2D textured = new Texture2D(maxAtlasSize, maxAtlasSize, textureFormat, mipmaps) {
                mipMapBias = -0.5f,
                anisoLevel = list2[0].anisoLevel,
                name = newAssetName
            };
            Rect[] rectArray = textured.PackTextures(list2.ToArray(), padding, maxAtlasSize);
            Shader shader = (materialPropertySource != null) ? materialPropertySource.shader : Shader.Find("Spine/Skeleton");
            Material m = new Material(shader);
            if (materialPropertySource != null)
            {
                m.CopyPropertiesFromMaterial(materialPropertySource);
                m.shaderKeywords = materialPropertySource.shaderKeywords;
            }
            m.name = newAssetName;
            m.mainTexture = textured;
            AtlasPage page = m.ToSpineAtlasPage();
            page.name = newAssetName;
            List<AtlasRegion> list4 = new List<AtlasRegion>();
            int index = 0;
            int num6 = list3.Count;
            while (index < num6)
            {
                AtlasRegion region2 = list3[index];
                AtlasRegion item = UVRectToAtlasRegion(rectArray[index], region2.name, page, region2.offsetX, region2.offsetY, region2.rotate);
                list4.Add(item);
                index++;
            }
            int num7 = 0;
            int num8 = outputAttachments.Count;
            while (num7 < num8)
            {
                Attachment a = outputAttachments[num7];
                if (IsRenderable(a))
                {
                    a.SetRegion(list4[list[num7]], true);
                }
                num7++;
            }
            if (clearCache)
            {
                ClearCache();
            }
            outputTexture = textured;
            outputMaterial = m;
        }

        public static Skin GetRepackedSkin(this Skin o, string newName, Material materialPropertySource, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 0x400, int padding = 2, TextureFormat textureFormat = 4, bool mipmaps = false, bool useOriginalNonrenderables = true) => 
            o.GetRepackedSkin(newName, materialPropertySource.shader, out outputMaterial, out outputTexture, maxAtlasSize, padding, textureFormat, mipmaps, materialPropertySource, useOriginalNonrenderables, true);

        public static Skin GetRepackedSkin(this Skin o, string newName, Shader shader, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 0x400, int padding = 2, TextureFormat textureFormat = 4, bool mipmaps = false, Material materialPropertySource = null, bool clearCache = false, bool useOriginalNonrenderables = true)
        {
            if (o == null)
            {
                throw new NullReferenceException("Skin was null");
            }
            Dictionary<Skin.AttachmentKeyTuple, Attachment> attachments = o.Attachments;
            Skin skin = new Skin(newName);
            Dictionary<AtlasRegion, int> dictionary2 = new Dictionary<AtlasRegion, int>();
            List<int> list = new List<int>();
            List<Attachment> list2 = new List<Attachment>();
            List<Texture2D> list3 = new List<Texture2D>();
            List<AtlasRegion> list4 = new List<AtlasRegion>();
            int num = 0;
            foreach (KeyValuePair<Skin.AttachmentKeyTuple, Attachment> pair in attachments)
            {
                Skin.AttachmentKeyTuple key = pair.Key;
                Attachment a = pair.Value;
                if (IsRenderable(a))
                {
                    Attachment clone = a.GetClone(true);
                    AtlasRegion region = clone.GetRegion();
                    if (dictionary2.TryGetValue(region, out int num2))
                    {
                        list.Add(num2);
                    }
                    else
                    {
                        list4.Add(region);
                        list3.Add(region.ToTexture(true, TextureFormat.RGBA32, false));
                        dictionary2.Add(region, num);
                        list.Add(num);
                        num++;
                    }
                    list2.Add(clone);
                    skin.AddAttachment(key.slotIndex, key.name, clone);
                }
                else
                {
                    skin.AddAttachment(key.slotIndex, key.name, !useOriginalNonrenderables ? a.GetClone(true) : a);
                }
            }
            Texture2D textured = new Texture2D(maxAtlasSize, maxAtlasSize, textureFormat, mipmaps) {
                mipMapBias = -0.5f,
                anisoLevel = list3[0].anisoLevel,
                name = newName
            };
            Rect[] rectArray = textured.PackTextures(list3.ToArray(), padding, maxAtlasSize);
            Material m = new Material(shader);
            if (materialPropertySource != null)
            {
                m.CopyPropertiesFromMaterial(materialPropertySource);
                m.shaderKeywords = materialPropertySource.shaderKeywords;
            }
            m.name = newName;
            m.mainTexture = textured;
            AtlasPage page = m.ToSpineAtlasPage();
            page.name = newName;
            List<AtlasRegion> list5 = new List<AtlasRegion>();
            int index = 0;
            int count = list4.Count;
            while (index < count)
            {
                AtlasRegion region2 = list4[index];
                AtlasRegion item = UVRectToAtlasRegion(rectArray[index], region2.name, page, region2.offsetX, region2.offsetY, region2.rotate);
                list5.Add(item);
                index++;
            }
            int num5 = 0;
            int num6 = list2.Count;
            while (num5 < num6)
            {
                Attachment a = list2[num5];
                if (IsRenderable(a))
                {
                    a.SetRegion(list5[list[num5]], true);
                }
                num5++;
            }
            if (clearCache)
            {
                ClearCache();
            }
            outputTexture = textured;
            outputMaterial = m;
            return skin;
        }

        private static Rect GetSpineAtlasRect(this AtlasRegion region, bool includeRotate = true)
        {
            if (includeRotate && region.rotate)
            {
                return new Rect((float) region.x, (float) region.y, (float) region.height, (float) region.width);
            }
            return new Rect((float) region.x, (float) region.y, (float) region.width, (float) region.height);
        }

        private static Rect GetUnityRect(this AtlasRegion region) => 
            region.GetSpineAtlasRect(true).SpineUnityFlipRect(region.page.height);

        private static Rect GetUnityRect(this AtlasRegion region, int textureHeight) => 
            region.GetSpineAtlasRect(true).SpineUnityFlipRect(textureHeight);

        private static float InverseLerp(float a, float b, float value) => 
            ((value - a) / (b - a));

        private static bool IsRenderable(Attachment a) => 
            (a is IHasRendererObject);

        private static Rect SpineUnityFlipRect(this Rect rect, int textureHeight)
        {
            rect.y = (textureHeight - rect.y) - rect.height;
            return rect;
        }

        private static Rect TextureRectToUVRect(Rect textureRect, int texWidth, int texHeight)
        {
            textureRect.x = Mathf.InverseLerp(0f, (float) texWidth, textureRect.x);
            textureRect.y = Mathf.InverseLerp(0f, (float) texHeight, textureRect.y);
            textureRect.width = Mathf.InverseLerp(0f, (float) texWidth, textureRect.width);
            textureRect.height = Mathf.InverseLerp(0f, (float) texHeight, textureRect.height);
            return textureRect;
        }

        public static AtlasRegion ToAtlasRegion(this Sprite s, AtlasPage page)
        {
            if (page == null)
            {
                throw new ArgumentNullException("page", "page cannot be null. AtlasPage determines which texture region belongs and how it should be rendered. You can use material.ToSpineAtlasPage() to get a shareable AtlasPage from a Material, or use the sprite.ToAtlasRegion(material) overload.");
            }
            AtlasRegion region = s.ToAtlasRegion(false);
            region.page = page;
            return region;
        }

        internal static AtlasRegion ToAtlasRegion(this Sprite s, bool isolatedTexture = false)
        {
            AtlasRegion region = new AtlasRegion {
                name = s.name,
                index = -1,
                rotate = s.packed && (s.packingRotation != SpritePackingRotation.None)
            };
            Bounds bounds = s.bounds;
            Vector2 min = bounds.min;
            Vector2 max = bounds.max;
            Rect rect = s.rect.SpineUnityFlipRect(s.texture.height);
            region.width = (int) rect.width;
            region.originalWidth = (int) rect.width;
            region.height = (int) rect.height;
            region.originalHeight = (int) rect.height;
            region.offsetX = rect.width * (0.5f - InverseLerp(min.x, max.x, 0f));
            region.offsetY = rect.height * (0.5f - InverseLerp(min.y, max.y, 0f));
            if (isolatedTexture)
            {
                region.u = 0f;
                region.v = 1f;
                region.u2 = 1f;
                region.v2 = 0f;
                region.x = 0;
                region.y = 0;
                return region;
            }
            Texture2D texture = s.texture;
            Rect rect2 = TextureRectToUVRect(s.textureRect, texture.width, texture.height);
            region.u = rect2.xMin;
            region.v = rect2.yMax;
            region.u2 = rect2.xMax;
            region.v2 = rect2.yMin;
            region.x = (int) rect.x;
            region.y = (int) rect.y;
            return region;
        }

        public static AtlasRegion ToAtlasRegion(this Sprite s, Material material)
        {
            AtlasRegion region = s.ToAtlasRegion(false);
            region.page = material.ToSpineAtlasPage();
            return region;
        }

        public static AtlasRegion ToAtlasRegion(this Texture2D t, Material materialPropertySource, float scale = 0.01f) => 
            t.ToAtlasRegion(materialPropertySource.shader, scale, materialPropertySource);

        public static AtlasRegion ToAtlasRegion(this Texture2D t, Shader shader, float scale = 0.01f, Material materialPropertySource = null)
        {
            Material m = new Material(shader);
            if (materialPropertySource != null)
            {
                m.CopyPropertiesFromMaterial(materialPropertySource);
                m.shaderKeywords = materialPropertySource.shaderKeywords;
            }
            m.mainTexture = t;
            AtlasPage page = m.ToSpineAtlasPage();
            float width = t.width;
            float height = t.height;
            AtlasRegion region = new AtlasRegion {
                name = t.name,
                index = -1,
                rotate = false
            };
            Vector2 zero = Vector2.zero;
            Vector2 vector2 = new Vector2(width, height) * scale;
            region.width = (int) width;
            region.originalWidth = (int) width;
            region.height = (int) height;
            region.originalHeight = (int) height;
            region.offsetX = width * (0.5f - InverseLerp(zero.x, vector2.x, 0f));
            region.offsetY = height * (0.5f - InverseLerp(zero.y, vector2.y, 0f));
            region.u = 0f;
            region.v = 1f;
            region.u2 = 1f;
            region.v2 = 0f;
            region.x = 0;
            region.y = 0;
            region.page = page;
            return region;
        }

        public static AtlasRegion ToAtlasRegionPMAClone(this Sprite s, Material materialPropertySource, TextureFormat textureFormat = 4, bool mipmaps = false) => 
            s.ToAtlasRegionPMAClone(materialPropertySource.shader, textureFormat, mipmaps, materialPropertySource);

        public static AtlasRegion ToAtlasRegionPMAClone(this Texture2D t, Material materialPropertySource, TextureFormat textureFormat = 4, bool mipmaps = false) => 
            t.ToAtlasRegionPMAClone(materialPropertySource.shader, textureFormat, mipmaps, materialPropertySource);

        public static AtlasRegion ToAtlasRegionPMAClone(this Sprite s, Shader shader, TextureFormat textureFormat = 4, bool mipmaps = false, Material materialPropertySource = null)
        {
            Material m = new Material(shader);
            if (materialPropertySource != null)
            {
                m.CopyPropertiesFromMaterial(materialPropertySource);
                m.shaderKeywords = materialPropertySource.shaderKeywords;
            }
            Texture2D texture = s.ToTexture(false, textureFormat, mipmaps);
            texture.ApplyPMA(true);
            texture.name = s.name + "-pma-";
            m.name = texture.name + shader.name;
            m.mainTexture = texture;
            AtlasPage page = m.ToSpineAtlasPage();
            AtlasRegion region = s.ToAtlasRegion(true);
            region.page = page;
            return region;
        }

        public static AtlasRegion ToAtlasRegionPMAClone(this Texture2D t, Shader shader, TextureFormat textureFormat = 4, bool mipmaps = false, Material materialPropertySource = null)
        {
            Material m = new Material(shader);
            if (materialPropertySource != null)
            {
                m.CopyPropertiesFromMaterial(materialPropertySource);
                m.shaderKeywords = materialPropertySource.shaderKeywords;
            }
            Texture2D texture = t.GetClone(false, textureFormat, mipmaps);
            texture.ApplyPMA(true);
            texture.name = t.name + "-pma-";
            m.name = t.name + shader.name;
            m.mainTexture = texture;
            AtlasPage page = m.ToSpineAtlasPage();
            AtlasRegion region = texture.ToAtlasRegion(shader, 0.01f, null);
            region.page = page;
            return region;
        }

        public static AtlasPage ToSpineAtlasPage(this Material m)
        {
            AtlasPage page = new AtlasPage {
                rendererObject = m,
                name = m.name
            };
            Texture mainTexture = m.mainTexture;
            if (mainTexture != null)
            {
                page.width = mainTexture.width;
                page.height = mainTexture.height;
            }
            return page;
        }

        public static Sprite ToSprite(this AtlasRegion ar, float pixelsPerUnit = 100f) => 
            Sprite.Create(ar.GetMainTexture(), ar.GetUnityRect(), new Vector2(0.5f, 0.5f), pixelsPerUnit);

        public static Texture2D ToTexture(this AtlasRegion ar, bool applyImmediately = true, TextureFormat textureFormat = 4, bool mipmaps = false)
        {
            CachedRegionTextures.TryGetValue(ar, out Texture2D textured);
            if (textured == null)
            {
                Texture2D mainTexture = ar.GetMainTexture();
                Rect unityRect = ar.GetUnityRect(mainTexture.height);
                int width = (int) unityRect.width;
                int height = (int) unityRect.height;
                textured = new Texture2D(width, height, textureFormat, mipmaps) {
                    name = ar.name
                };
                Color[] colors = mainTexture.GetPixels((int) unityRect.x, (int) unityRect.y, width, height);
                textured.SetPixels(colors);
                CachedRegionTextures.Add(ar, textured);
                CachedRegionTexturesList.Add(textured);
                if (applyImmediately)
                {
                    textured.Apply();
                }
            }
            return textured;
        }

        private static Texture2D ToTexture(this Sprite s, bool applyImmediately = true, TextureFormat textureFormat = 4, bool mipmaps = false)
        {
            Texture2D texture = s.texture;
            Rect textureRect = s.textureRect;
            Color[] colors = texture.GetPixels((int) textureRect.x, (int) textureRect.y, (int) textureRect.width, (int) textureRect.height);
            Texture2D textured2 = new Texture2D((int) textureRect.width, (int) textureRect.height, textureFormat, mipmaps);
            textured2.SetPixels(colors);
            if (applyImmediately)
            {
                textured2.Apply();
            }
            return textured2;
        }

        private static AtlasRegion UVRectToAtlasRegion(Rect uvRect, string name, AtlasPage page, float offsetX, float offsetY, bool rotate)
        {
            int height;
            int width;
            Rect rect2 = UVRectToTextureRect(uvRect, page.width, page.height).SpineUnityFlipRect(page.height);
            int x = (int) rect2.x;
            int y = (int) rect2.y;
            if (rotate)
            {
                height = (int) rect2.height;
                width = (int) rect2.width;
            }
            else
            {
                height = (int) rect2.width;
                width = (int) rect2.height;
            }
            return new AtlasRegion { 
                page = page,
                name = name,
                u = uvRect.xMin,
                u2 = uvRect.xMax,
                v = uvRect.yMax,
                v2 = uvRect.yMin,
                index = -1,
                width = height,
                originalWidth = height,
                height = width,
                originalHeight = width,
                offsetX = offsetX,
                offsetY = offsetY,
                x = x,
                y = y,
                rotate = rotate
            };
        }

        private static Rect UVRectToTextureRect(Rect uvRect, int texWidth, int texHeight)
        {
            uvRect.x *= texWidth;
            uvRect.width *= texWidth;
            uvRect.y *= texHeight;
            uvRect.height *= texHeight;
            return uvRect;
        }
    }
}

