namespace Spine.Unity
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class SpineAtlasRegion : PropertyAttribute
    {
        public string atlasAssetField;

        public SpineAtlasRegion(string atlasAssetField = "")
        {
            this.atlasAssetField = atlasAssetField;
        }
    }
}

