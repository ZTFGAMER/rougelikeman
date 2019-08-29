namespace TableTool
{
    using System;

    public class Stage_Level_chapter6Model : LocalModel<Stage_Level_chapter6, string>
    {
        private const string _Filename = "Stage_Level_chapter6";

        protected override string GetBeanKey(Stage_Level_chapter6 bean) => 
            bean.RoomID;

        protected override string Filename =>
            "Stage_Level_chapter6";
    }
}

