namespace GoogleMobileAds.Common
{
    using System;
    using UnityEngine;

    internal class Utils
    {
        public static Texture2D GetTexture2DFromByteArray(byte[] img)
        {
            Texture2D tex = new Texture2D(1, 1);
            if (!tex.LoadImage(img))
            {
                throw new InvalidOperationException("Could not load custom native template\n                        image asset as texture");
            }
            return tex;
        }
    }
}

