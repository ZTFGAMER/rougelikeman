namespace TableTool
{
    using System;

    public class Stage_Level_chapter9Model : LocalModel<Stage_Level_chapter9, string>
    {
        private const string _Filename = "Stage_Level_chapter9";

        protected override string GetBeanKey(Stage_Level_chapter9 bean) => 
            bean.RoomID;

        protected override string Filename =>
            "Stage_Level_chapter9";
    }
}

