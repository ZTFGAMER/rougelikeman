namespace Dxx
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu]
    public class DxxSpriteAtlas : ScriptableObject
    {
        public string tag;
        public List<Sprite> sprites = new List<Sprite>();
        private Dictionary<string, int> nameToIndex = new Dictionary<string, int>();

        public Sprite GetSprite(string spriteName)
        {
            this.Initialize();
            spriteName = spriteName.ToLower();
            if (this.nameToIndex.ContainsKey(spriteName))
            {
                return this.sprites[this.nameToIndex[spriteName]];
            }
            return null;
        }

        private void Initialize()
        {
            if (this.nameToIndex.Count == 0)
            {
                this.nameToIndex.Clear();
                for (int i = 0; i < this.sprites.Count; i++)
                {
                    this.nameToIndex.Add(this.sprites[i].name.ToLower(), i);
                }
            }
        }
    }
}

