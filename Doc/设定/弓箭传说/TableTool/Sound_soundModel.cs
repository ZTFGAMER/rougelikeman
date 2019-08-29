namespace TableTool
{
    using System;

    public class Sound_soundModel : LocalModel<Sound_sound, int>
    {
        private const string _Filename = "Sound_sound";

        protected override int GetBeanKey(Sound_sound bean) => 
            bean.ID;

        protected override string Filename =>
            "Sound_sound";
    }
}

