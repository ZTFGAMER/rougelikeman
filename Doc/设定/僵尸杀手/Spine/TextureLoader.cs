namespace Spine
{
    using System;

    public interface TextureLoader
    {
        void Load(AtlasPage page, string path);
        void Unload(object texture);
    }
}

